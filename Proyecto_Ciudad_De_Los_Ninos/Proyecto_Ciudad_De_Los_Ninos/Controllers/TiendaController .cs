using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Proyecto_Ciudad_De_Los_Ninos.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;
using API_Ciudad_De_Los_Ninos.Models;
using Microsoft.AspNetCore.Authorization;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    [Authorize(Policy = "Rol15")]
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
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
            {
                // Contar los productos en el carrito
                var ticketCounts = await _context.Tickete
                    .Where(t => t.id_usuario == userId)
                    .GroupBy(t => t.id_usuario)
                    .Select(g => new { UserId = g.Key, Count = g.Count() })
                    .FirstOrDefaultAsync();

                ViewBag.TicketCount = ticketCounts?.Count ?? 0;

                // Calcular el total del precio de los productos en el carrito
                var ticketTotal = await _context.Tickete
                    .Where(t => t.id_usuario == userId)
                    .Join(_context.Inventario_Higiene_Personal,
                        t => t.id_inventario_higiene_personal,
                        p => p.Id,
                        (t, p) => new { t, p })
                    .SumAsync(tp => tp.p.precio_unitario);

                ViewBag.TicketTotal = ticketTotal;
            }
            else
            {
                ViewBag.TicketCount = 0;
                ViewBag.TicketTotal = 0.0m;
            }

            return View(productos);
        }


        public IActionResult OrdenRecibida()
        {
            return View();
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

            var existingItem = await _context.Tickete
                .FirstOrDefaultAsync(t => t.id_usuario == userId && t.id_inventario_higiene_personal == idProducto);

            if (existingItem != null)
            {
                return RedirectToAction(nameof(Index));
            }

            var productoInventario = await _context.Inventario_Higiene_Personal.FindAsync(idProducto);
            if (productoInventario == null)
            {
                return NotFound("Producto no encontrado en el inventario.");
            }

            if (productoInventario.cantidad_disponible <= 0)
            {
                return BadRequest("El producto no está disponible en el inventario.");
            }

            var tickete = new Tickete
            {
                id_usuario = userId,
                id_inventario_higiene_personal = idProducto,
                tickete = 1
            };

            _context.Tickete.Add(tickete);

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

            var emailBody = "<html><body>";
            emailBody += "<h2 style='color: #007bff;'>Gracias por su Orden!</h2>";
            emailBody += "<p>Aquí está el resumen de su carrito:</p>";
            emailBody += "<table style='width: 100%; border-collapse: collapse;'>";
            emailBody += "<tr><th style='border: 1px solid #ddd; padding: 8px;'>Producto</th><th style='border: 1px solid #ddd; padding: 8px;'>Cantidad</th><th style='border: 1px solid #ddd; padding: 8px;'>Precio Unitario</th><th style='border: 1px solid #ddd; padding: 8px;'>Total</th></tr>";

            decimal totalCompra = 0;

            foreach (var item in productosCarrito)
            {
                var registroCompra = new RegistroCompra
                {
                    TicketeId = item.Id,
                    UserId = userId,
                    Inventario_HigieneId = item.id_inventario_higiene_personal, 
                    estado = "Sin entregar"
                };
                decimal precioUnitario = item.inventario_Higiene_Personal.precio_unitario ?? 0m;
                decimal subtotal = precioUnitario * item.tickete;
                totalCompra += subtotal;

                emailBody += $"<tr><td style='border: 1px solid #ddd; padding: 8px;'>{item.inventario_Higiene_Personal.nombre_producto}</td>";
                emailBody += $"<td style='border: 1px solid #ddd; padding: 8px; text-align: center;'>{item.tickete}</td>";
                emailBody += $"<td style='border: 1px solid #ddd; padding: 8px; text-align: right;'>${precioUnitario.ToString("0.00")}</td>";
                emailBody += $"<td style='border: 1px solid #ddd; padding: 8px; text-align: right;'>${subtotal.ToString("0.00")}</td></tr>";

                _context.RegistroCompra.Add(registroCompra);
            }

            emailBody += "</table>";
            emailBody += $"<p style='text-align: right; margin-top: 20px;'><strong>Total de la Orden: ${totalCompra.ToString("0.00")}</strong></p>";
            emailBody += "</body></html>";

            var clienteNombre = User.Identity.Name;
            var pdfBytes = GeneratePDF(productosCarrito, clienteNombre);

            string mimeType = "application/pdf";
            string attachmentFileName = "ResumenCompra.pdf";

            _emailService.SendEmailWithAttachment(userEmailClaim, "Resumen de su Carrito de Compras", emailBody, pdfBytes, attachmentFileName, mimeType);

            await _context.SaveChangesAsync();

            _context.Tickete.RemoveRange(productosCarrito);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(OrdenRecibida));
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
        int tableWidth = (int)(page.Width - 2 * margin);
        const int tableStartX = margin;
        const int tableStartY = 100;
        const int cellHeight = 20;
        int yPos = margin;

        // Title
        gfx.DrawString("Factura de Orden", fontTitle, XBrushes.Black, new XRect(margin, yPos, page.Width - 2 * margin, 30), XStringFormats.TopCenter);
        yPos += 40;

        // Client and Date
        gfx.DrawString($"Cliente: {clienteNombre}", fontBody, XBrushes.Black, new XRect(margin, yPos, page.Width - 2 * margin, 20), XStringFormats.TopLeft);
        yPos += 20;
        gfx.DrawString($"Fecha: {DateTime.Now:dd/MM/yyyy}", fontBody, XBrushes.Black, new XRect(margin, yPos, page.Width - 2 * margin, 20), XStringFormats.TopLeft);
        yPos += 40;

        // Table headers
        gfx.DrawRectangle(XPens.Black, tableStartX, yPos, tableWidth, cellHeight);
        gfx.DrawString("Producto", fontHeader, XBrushes.Black, new XRect(tableStartX + 10, yPos + 5, 150, cellHeight), XStringFormats.TopLeft);
        gfx.DrawString("Cantidad", fontHeader, XBrushes.Black, new XRect(tableStartX + 160, yPos + 5, 70, cellHeight), XStringFormats.TopLeft);
        gfx.DrawString("Precio Unitario", fontHeader, XBrushes.Black, new XRect(tableStartX + 230, yPos + 5, 100, cellHeight), XStringFormats.TopLeft);
        gfx.DrawString("Total", fontHeader, XBrushes.Black, new XRect(tableStartX + 330, yPos + 5, 70, cellHeight), XStringFormats.TopLeft);
        gfx.DrawString("Tickete ID", fontHeader, XBrushes.Black, new XRect(tableStartX + 400, yPos + 5, page.Width - 2 * margin - 400, cellHeight), XStringFormats.TopLeft);
        yPos += cellHeight;

        decimal grandTotal = 0;

        // Table rows
        foreach (var item in productosCarrito)
        {
            var producto = item.inventario_Higiene_Personal.nombre_producto;
            var cantidad = item.tickete.ToString();
            var precioUnitario = item.inventario_Higiene_Personal.precio_unitario ?? 0m;
            var total = precioUnitario * item.tickete;
            var ticketeId = item.Id.ToString();

            gfx.DrawRectangle(XPens.Black, tableStartX, yPos, tableWidth, cellHeight);
            gfx.DrawString(producto, fontBody, XBrushes.Black, new XRect(tableStartX + 10, yPos + 5, 150, cellHeight), XStringFormats.TopLeft);
            gfx.DrawString(cantidad, fontBody, XBrushes.Black, new XRect(tableStartX + 160, yPos + 5, 70, cellHeight), XStringFormats.TopLeft);
            gfx.DrawString($"${precioUnitario:0.00}", fontBody, XBrushes.Black, new XRect(tableStartX + 230, yPos + 5, 100, cellHeight), XStringFormats.TopLeft);
            gfx.DrawString($"${total:0.00}", fontBody, XBrushes.Black, new XRect(tableStartX + 330, yPos + 5, 70, cellHeight), XStringFormats.TopLeft);
            gfx.DrawString(ticketeId, fontBody, XBrushes.Black, new XRect(tableStartX + 400, yPos + 5, page.Width - 2 * margin - 400, cellHeight), XStringFormats.TopLeft);
            yPos += cellHeight;

            grandTotal += total;
        }

        // Total amount
        gfx.DrawString($"Total General: ${grandTotal:0.00}", fontHeader, XBrushes.Black, new XRect(tableStartX, yPos + 10, tableWidth, cellHeight), XStringFormats.TopLeft);

        pdf.Save(stream, false);
        return stream.ToArray();
    }
}

    }
}
