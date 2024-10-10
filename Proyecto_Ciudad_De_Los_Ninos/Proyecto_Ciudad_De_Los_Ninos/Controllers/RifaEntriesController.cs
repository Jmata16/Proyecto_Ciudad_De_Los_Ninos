using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using Proyecto_Ciudad_De_Los_Ninos.Models;
using API_Ciudad_De_Los_Ninos.Models;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    public class RifaEntriesController : Controller
    {
        private readonly ApplicationDBContext _context;
        private const decimal TicketPrice = 1500;

        public RifaEntriesController(ApplicationDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var today = DateTime.Now;
            ViewBag.Rifas = _context.Rifas
                .Where(r => r.FechaRifa > today)
                .ToList();
            return View();
        }


        [HttpGet]
        public JsonResult GetAvailableNumbers(int rifaId)
        {
            try
            {
                var purchasedNumbers = _context.RifaEntries
                    .Where(e => e.RifaId == rifaId)
                    .Select(e => e.NumeroComprado)
                    .ToList();

                var allNumbers = Enumerable.Range(1, 100).ToList();
                var availableNumbers = allNumbers.Except(purchasedNumbers).ToList();

                return Json(availableNumbers);
            }
            catch (Exception)
            {
                return Json(new { error = "Error al cargar los datos de los números." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(RifaEntry entry, string SelectedNumbers)
        {
            if (ModelState.IsValid)
            {
                var numbers = SelectedNumbers.Split(',').Select(num => int.Parse(num)).ToList();

                if (numbers.Count > 5)
                {
                    ModelState.AddModelError("", "No puedes seleccionar más de 5 números.");
                    ViewBag.Rifas = _context.Rifas.ToList();
                    return View(entry);
                }

                foreach (var number in numbers)
                {
                    var newEntry = new RifaEntry
                    {
                        Nombre = entry.Nombre,
                        Apellido = entry.Apellido,
                        Correo = entry.Correo,
                        NumeroComprado = number,
                        FechaCompra = DateTime.Now,
                        RifaId = entry.RifaId
                    };
                    _context.RifaEntries.Add(newEntry);
                }
                await _context.SaveChangesAsync();

                // Obtener información de la rifa
                var rifa = _context.Rifas.FirstOrDefault(r => r.RifaId == entry.RifaId);
                if (rifa == null)
                {
                    return NotFound();
                }

                // Calcular el total
                var totalCost = numbers.Count * TicketPrice;

                // Generar PDF
                var pdfBytes = GeneratePDF(numbers, entry.Nombre, entry.Apellido, rifa.FechaRifa, rifa.Premio, totalCost);

                // Guardar PDF en el servidor
                var fileName = $"ResumenCompra_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdfs", fileName);
                await System.IO.File.WriteAllBytesAsync(filePath, pdfBytes);

                return RedirectToAction("Confirmacion", new { fileName, totalCost });
            }
            ViewBag.Rifas = _context.Rifas.ToList();
            return View(entry);
        }

        public IActionResult Confirmacion(string fileName, decimal totalCost)
        {
            ViewBag.FileName = fileName;
            ViewBag.TotalCost = totalCost; // Pasar el total al ViewBag
            return View();
        }

        public IActionResult DescargarPdf(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdfs", fileName);

            if (System.IO.File.Exists(filePath))
            {
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/pdf", fileName);
            }

            return RedirectToAction("Index");
        }

        private byte[] GeneratePDF(List<int> numbers, string clienteNombre, string clienteApellido, DateTime rifaDate, string rifaName, decimal totalCost)
        {
            using (var stream = new MemoryStream())
            {
                var pdf = new PdfDocument();
                var page = pdf.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var fontTitle = new XFont("Arial", 24, XFontStyle.Bold);
                var fontSubtitle = new XFont("Arial", 18, XFontStyle.Bold);
                var fontBody = new XFont("Arial", 12, XFontStyle.Regular);
                var fontFooter = new XFont("Arial", 14, XFontStyle.Bold);

                const int margin = 40;
                int tableWidth = (int)(page.Width - 2 * margin);
                const int tableStartX = margin;
                const int cellHeight = 30;
                int yPos = margin;

                // Cargar y dibujar el logo
                var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logo.png");
                XImage logo = XImage.FromFile(logoPath);
                gfx.DrawImage(logo, margin, yPos, 150, 100);  // Ajustar tamaño y posición según necesites
                yPos += 120;  // Ajustar posición vertical después de añadir el logo

                // Título del PDF con fondo colorido
                gfx.DrawRectangle(XBrushes.LightGray, margin, yPos, page.Width - 2 * margin, 40);
                gfx.DrawString("Confirmación de Compra", fontTitle, XBrushes.Black, new XRect(margin, yPos, page.Width - 2 * margin, 40), XStringFormats.Center);
                yPos += 50;

                // Información del cliente
                gfx.DrawString($"Cliente: {clienteNombre} {clienteApellido}", fontSubtitle, XBrushes.Black, new XRect(margin, yPos, page.Width - 2 * margin, 30), XStringFormats.TopLeft);
                yPos += 35;

                // Información de la rifa
                gfx.DrawString($"Rifa: {rifaName}", fontSubtitle, XBrushes.Black, new XRect(margin, yPos, page.Width - 2 * margin, 30), XStringFormats.TopLeft);
                yPos += 30;
                gfx.DrawString($"Fecha de la Rifa: {rifaDate:dd/MM/yyyy}", fontSubtitle, XBrushes.Black, new XRect(margin, yPos, page.Width - 2 * margin, 30), XStringFormats.TopLeft);
                yPos += 50;

                // Encabezado de la tabla con fondo colorido
                gfx.DrawRectangle(XBrushes.LightBlue, tableStartX, yPos, tableWidth, cellHeight);
                gfx.DrawString("Número", fontBody, XBrushes.Black, new XRect(tableStartX + 10, yPos + 5, tableWidth - 20, cellHeight), XStringFormats.TopLeft);
                yPos += cellHeight;

                // Filas de la tabla
                foreach (var number in numbers)
                {
                    gfx.DrawRectangle(XPens.Black, tableStartX, yPos, tableWidth, cellHeight);
                    gfx.DrawString(number.ToString(), fontBody, XBrushes.Black, new XRect(tableStartX + 10, yPos + 5, tableWidth - 20, cellHeight), XStringFormats.TopLeft);
                    yPos += cellHeight;
                }

                // Línea final para cerrar el boleto
                gfx.DrawLine(XPens.Black, margin, yPos + 10, page.Width - margin, yPos + 10);

                // Mostrar el costo total con símbolo ₡
                yPos += 20; // Ajustar espacio antes del costo total
                gfx.DrawString($"Costo Total: ₡{totalCost:N0}", fontFooter, XBrushes.Black, new XRect(margin, yPos, page.Width - 2 * margin, 30), XStringFormats.TopLeft);

                pdf.Save(stream, false);
                return stream.ToArray();
            }
        }
    }
}
