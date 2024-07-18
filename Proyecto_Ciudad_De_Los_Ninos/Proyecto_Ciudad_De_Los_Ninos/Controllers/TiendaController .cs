using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;
using Proyecto_Ciudad_De_Los_Ninos.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    public class TiendaController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly EmailService _emailService;

        public TiendaController(ApplicationDBContext context)
        {
            _context = context;
            _emailService = new EmailService();
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _context.Inventario_Higiene_Personal.ToListAsync();
            return View(productos);
        }

        public async Task<IActionResult> GetImage(int id)
        {
            var inventario_Higiene_Personal = await _context.Inventario_Higiene_Personal.FirstOrDefaultAsync(i => i.Id == id);
            if (inventario_Higiene_Personal == null || inventario_Higiene_Personal.imagen == null)
            {
                return NotFound();
            }
            return File(inventario_Higiene_Personal.imagen, "image/png");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarAlCarrito(int idProducto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return NotFound("Usuario no encontrado o ID de usuario no válido.");
            }

            // Verificar si el producto ya está en el carrito del usuario
            var existingItem = await _context.Tickete
                .FirstOrDefaultAsync(t => t.id_usuario == userId && t.id_inventario_higiene_personal == idProducto);

            if (existingItem != null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Obtener el producto del inventario
            var productoInventario = await _context.Inventario_Higiene_Personal.FindAsync(idProducto);
            if (productoInventario == null)
            {
                return NotFound("Producto no encontrado en el inventario.");
            }

            // Verificar disponibilidad
            if (productoInventario.cantidad_disponible <= 0)
            {
                return BadRequest("El producto no está disponible en el inventario.");
            }

            // Agregar al carrito
            var tickete = new Tickete
            {
                id_usuario = userId,
                id_inventario_higiene_personal = idProducto,
                tickete = 1
            };

            _context.Tickete.Add(tickete);

            // Restar la cantidad del inventario
            productoInventario.cantidad_disponible--;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest("Error al agregar el producto al carrito. Inténtelo de nuevo más tarde.");
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Carrito()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return NotFound("Usuario no encontrado o ID de usuario no válido.");
            }

            var productosCarrito = await _context.Tickete
                .Include(t => t.inventario_Higiene_Personal)
                .Where(t => t.id_usuario == userId)
                .ToListAsync();

            return View(productosCarrito);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnviarCorreo()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return NotFound("Usuario no encontrado o ID de usuario no válido.");
            }

            var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmailClaim))
            {
                return NotFound("Correo del usuario no encontrado.");
            }

            var productosCarrito = await _context.Tickete
               .Include(t => t.inventario_Higiene_Personal)
               .Where(t => t.id_usuario == userId)
               .ToListAsync();

            if (!productosCarrito.Any())
            {
                return BadRequest("El carrito está vacío.");
            }

            // Construir el cuerpo del correo
            var emailBody = "<html><body>";
            emailBody += "<h2 style='color: #007bff;'>Gracias por su compra!</h2>";
            emailBody += "<p>Aquí está el resumen de su carrito:</p>";
            emailBody += "<table style='width: 100%; border-collapse: collapse;'>";
            emailBody += "<tr><th style='border: 1px solid #ddd; padding: 8px;'>Producto</th><th style='border: 1px solid #ddd; padding: 8px;'>Cantidad</th><th style='border: 1px solid #ddd; padding: 8px;'>Precio Unitario</th><th style='border: 1px solid #ddd; padding: 8px;'>Total</th></tr>";

            decimal totalCompra = 0;

            foreach (var item in productosCarrito)
            {
                decimal precioUnitario = item.inventario_Higiene_Personal.precio_unitario ?? 0m;
                decimal subtotal = precioUnitario * item.tickete;
                totalCompra += subtotal;

                emailBody += $"<tr><td style='border: 1px solid #ddd; padding: 8px;'>{item.inventario_Higiene_Personal.nombre_producto}</td>";
                emailBody += $"<td style='border: 1px solid #ddd; padding: 8px; text-align: center;'>{item.tickete}</td>";
                emailBody += $"<td style='border: 1px solid #ddd; padding: 8px; text-align: right;'>${precioUnitario.ToString("0.00")}</td>";
                emailBody += $"<td style='border: 1px solid #ddd; padding: 8px; text-align: right;'>${subtotal.ToString("0.00")}</td></tr>";

                // Eliminar el producto del carrito después de procesarlo
                _context.Tickete.Remove(item);
            }

            emailBody += "</table>";
            emailBody += $"<p style='text-align: right; margin-top: 20px;'><strong>Total de la compra: ${totalCompra.ToString("0.00")}</strong></p>";
            emailBody += "</body></html>";

            // Generar y enviar el PDF como adjunto
            // Generar y enviar el PDF como adjunto
            // Llamada a GeneratePDF en el método EnviarCorreo
            var clienteNombre = User.Identity.Name; // Obtener el nombre del cliente
            var pdfBytes = GeneratePDF(productosCarrito, clienteNombre);


            // Definir el tipo MIME del archivo adjunto (application/pdf en este caso)
            string mimeType = "application/pdf";
            string attachmentFileName = "ResumenCompra.pdf";

            // Enviar correo electrónico con el PDF adjunto
            _emailService.SendEmailWithAttachment(userEmailClaim, "Resumen de su Carrito de Compras", emailBody, pdfBytes, attachmentFileName, mimeType);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        private byte[] GeneratePDF(List<Tickete> productosCarrito, string clienteNombre)
        {
            using (var stream = new MemoryStream())
            {
                var pdf = new PdfDocument();
                var page = pdf.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var fontTitle = new XFont("Arial", 18, XFontStyle.Bold);
                var fontHeader = new XFont("Arial", 12, XFontStyle.Bold);
                var fontBody = new XFont("Arial", 10, XFontStyle.Regular);

                const int margin = 40;
                var yPos = margin;

                // Header
                gfx.DrawString("Factura de Compra", fontTitle, XBrushes.Black, new XRect(margin, yPos, page.Width - 2 * margin, 30), XStringFormats.TopCenter);
                yPos += 40;

                // Cliente
                gfx.DrawString($"Cliente: {clienteNombre}", fontBody, XBrushes.Black, new XRect(margin, yPos, page.Width - 2 * margin, 20), XStringFormats.TopLeft);
                yPos += 20;

                // Fecha
                gfx.DrawString($"Fecha: {DateTime.Now.ToString("dd/MM/yyyy")}", fontBody, XBrushes.Black, new XRect(margin, yPos, page.Width - 2 * margin, 20), XStringFormats.TopLeft);
                yPos += 20;

                // Table headers
                gfx.DrawRectangle(XPens.Black, margin, yPos, page.Width - 2 * margin, 30);
                gfx.DrawString("Producto", fontHeader, XBrushes.Black, new XRect(margin + 10, yPos + 10, 200, 20), XStringFormats.TopLeft);
                gfx.DrawString("Cantidad", fontHeader, XBrushes.Black, new XRect(margin + 210, yPos + 10, 100, 20), XStringFormats.TopLeft);
                gfx.DrawString("Precio Unitario", fontHeader, XBrushes.Black, new XRect(margin + 310, yPos + 10, 100, 20), XStringFormats.TopLeft);
                gfx.DrawString("Total", fontHeader, XBrushes.Black, new XRect(margin + 410, yPos + 10, page.Width - 2 * margin - 410, 20), XStringFormats.TopLeft);
                yPos += 40;

                // Table content
                decimal totalGeneral = 0m;

                foreach (var producto in productosCarrito)
                {
                    var nombreProducto = producto.inventario_Higiene_Personal.nombre_producto;
                    var cantidad = producto.tickete;
                    var precioUnitario = producto.inventario_Higiene_Personal.precio_unitario ?? 0m;
                    var subtotalProducto = cantidad * precioUnitario;

                    // Draw row
                    gfx.DrawRectangle(XPens.Black, margin, yPos, page.Width - 2 * margin, 20);
                    gfx.DrawString(nombreProducto, fontBody, XBrushes.Black, new XRect(margin + 10, yPos + 2, 200, 20), XStringFormats.TopLeft);
                    gfx.DrawString(cantidad.ToString(), fontBody, XBrushes.Black, new XRect(margin + 210, yPos + 2, 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString(precioUnitario.ToString("0.00"), fontBody, XBrushes.Black, new XRect(margin + 310, yPos + 2, 100, 20), XStringFormats.TopLeft);
                    gfx.DrawString(subtotalProducto.ToString("0.00"), fontBody, XBrushes.Black, new XRect(margin + 410, yPos + 2, page.Width - 2 * margin - 410, 20), XStringFormats.TopLeft);

                    yPos += 20;
                    totalGeneral += subtotalProducto;
                }

                // Total general
                yPos += 10;
                gfx.DrawString($"Total: ${totalGeneral.ToString("0.00")}", fontHeader, XBrushes.Black, new XRect(margin, yPos, page.Width - 2 * margin, 30), XStringFormats.TopLeft);

                pdf.Save(stream, false);
                return stream.ToArray();
            }
        }






    }
}