using API_Ciudad_De_Los_Ninos.Models;
using ClosedXML.Excel;
using CsvHelper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Ciudad_De_Los_Ninos.Models;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Proyecto_Ciudad_De_Los_Ninos.Controllers
{
    [Authorize(Policy = "Rol134")]
    public class ExpedientesController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ExpedientesController(ApplicationDBContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(string searchString, int? searchAge)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentAgeFilter"] = searchAge;

            var expedientes = from e in _context.Expedientes
                              where e.estado == "Activo" // Filtrar solo por estado "Activo"
                              select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                expedientes = expedientes.Where(e => e.nombre_joven.Contains(searchString));
            }

            return View(await expedientes.ToListAsync());
        }
        public async Task<IActionResult> Desactivado()
        {
            var expedientesDesactivados = await _context.Expedientes
                .Where(e => e.estado == "Desactivado") // Filtrar por estado desactivado
                .ToListAsync();

            return View(expedientesDesactivados);
        }

        // GET: Expedientes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var expediente = await _context.Expedientes
                .Include(e => e.Joven)

                .FirstOrDefaultAsync(m => m.Id == id);

            if (expediente == null)
            {
                return NotFound();
            }

            return View(expediente);
        }

        // GET: Expedientes/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Jovenes"] = new SelectList(_context.Jovenes, "Id", "nombre");
            return View();
        }

        // POST: Expedientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,id_joven,nombre_joven,fecha_ingreso,tutor_legal,antecedentes_medicos,historial_academico,notas_adicionales")] Expedientes expediente)
        {
            if (ModelState.IsValid)
            {
                // Verificar si ya existe un expediente para el id_joven
                var existingExpediente = await _context.Expedientes.FirstOrDefaultAsync(e => e.id_joven == expediente.id_joven);

                if (existingExpediente != null)
                {
                    ModelState.AddModelError("id_joven", "Ya existe un expediente para este joven.");
                    ViewData["Jovenes"] = new SelectList(_context.Jovenes, "Id", "nombre", expediente.id_joven);
                    return View(expediente);
                }

                // Si no existe, agregar el expediente al contexto y guardar los cambios
                _context.Add(expediente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Jovenes"] = new SelectList(_context.Jovenes, "Id", "nombre", expediente.id_joven);
            return View(expediente);
        }
        // GET: Expedientes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var expediente = await _context.Expedientes.FindAsync(id);
            if (expediente == null)
            {
                return NotFound();
            }

            ViewData["Jovenes"] = new SelectList(_context.Jovenes, "Id", "nombre", expediente.id_joven);
            return View(expediente);
        }

        // POST: Expedientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,id_joven,nombre_joven,edad,fecha_ingreso,direccion,telefono_contacto,tutor_legal,antecedentes_medicos,historial_academico,notas_adicionales")] Expedientes expediente)
        {
            if (id != expediente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expediente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpedienteExists(expediente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Jovenes"] = new SelectList(_context.Jovenes, "Id", "nombre", expediente.id_joven);
            return View(expediente);
        }

        // GET: Expedientes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var expediente = await _context.Expedientes
                .Include(e => e.Joven)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (expediente == null)
            {
                return NotFound();
            }

            return View(expediente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var expediente = await _context.Expedientes.FindAsync(id);
                if (expediente == null)
                {
                    return RedirectToAction(nameof(Index), new { errorMessage = "El expediente no se encontró." });
                }

                // Cambia el estado a "Desactivado"
                expediente.estado = "Desactivado";

                // Actualiza el registro en la base de datos
                _context.Update(expediente);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                var errorViewModel = new ErrorViewModel
                {
                    StatusCode = 500,
                    Message = "Ocurrió un error al intentar desactivar el expediente.",
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                };

                return View("Error", errorViewModel);
            }
        }



        private bool ExpedienteExists(int id)
        {
            return _context.Expedientes.Any(e => e.Id == id);
        }


        /// Método para exportar a PDF con diseño mejorado
        public IActionResult ExportToPdf()
        {
            var expedientes = _context.Expedientes.ToList();

            using (var stream = new MemoryStream())
            {
                // Crear documento PDF
                Document pdfDoc = new Document(PageSize.A4);
                PdfWriter.GetInstance(pdfDoc, stream).CloseStream = false;
                pdfDoc.Open();

                // Cargar fuentes personalizadas
                var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                var titleFont = new Font(baseFont, 18, Font.BOLD, BaseColor.BLACK);
                var headerFont = new Font(baseFont, 12, Font.BOLD, BaseColor.WHITE);
                var cellFont = new Font(baseFont, 12, Font.NORMAL, BaseColor.BLACK);

                // Agregar logo
                var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logo.png");
                var logo = Image.GetInstance(logoPath);
                logo.ScaleToFit(100f, 100f);
                logo.Alignment = Image.ALIGN_LEFT;
                pdfDoc.Add(logo);

                // Agregar título con fecha dinámica
                var title = new Paragraph("Lista de Expedientes", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f
                };
                pdfDoc.Add(title);

                // Agregar mensaje introductorio
                var intro = new Paragraph("A continuación se presenta un reporte de los expedientes disponibles actualmente. Fecha de generación: " + DateTime.Now.ToShortDateString(), cellFont)
                {
                    SpacingAfter = 10f
                };
                pdfDoc.Add(intro);

                // Crear la tabla con estilos
                PdfPTable table = new PdfPTable(4) // Cambié a 4 columnas ya que "Nombre del Joven", "Fecha de Ingreso", "Tutor Legal" y "Estado"
                {
                    WidthPercentage = 100,
                    SpacingBefore = 10f,
                    SpacingAfter = 20f
                };
                table.SetWidths(new float[] { 2f, 1.5f, 2f, 2f }); // Ajusté los anchos de las columnas

                // Agregar encabezado de la tabla
                var headers = new string[] { "Nombre del Joven", "Fecha de Ingreso", "Tutor Legal", "Estado" };
                foreach (var header in headers)
                {
                    var cell = new PdfPCell(new Phrase(header, headerFont))
                    {
                        BackgroundColor = new BaseColor(0, 121, 241),
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 5
                    };
                    table.AddCell(cell);
                }

                // Agregar filas a la tabla
                foreach (var exp in expedientes)
                {
                    table.AddCell(new PdfPCell(new Phrase(exp.nombre_joven, cellFont)) { Padding = 5 });
                    table.AddCell(new PdfPCell(new Phrase(exp.fecha_ingreso.ToShortDateString(), cellFont)) { Padding = 5 });
                    table.AddCell(new PdfPCell(new Phrase(exp.tutor_legal, cellFont)) { Padding = 5 });
                    table.AddCell(new PdfPCell(new Phrase(exp.estado, cellFont)) { Padding = 5 });
                }

                pdfDoc.Add(table);
                pdfDoc.Close();

                // Necesario para evitar el error de flujo cerrado
                stream.Flush();
                stream.Position = 0;

                // Devolver el archivo PDF como una descarga
                return File(stream.ToArray(), "application/pdf", "Expedientes.pdf");
            }
        }


        // Método para exportar a Excel
        public IActionResult ExportToExcel()
        {
            var expedientes = _context.Expedientes.ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Expedientes");

                // Encabezados
                var headers = new List<string[]>
        {
            new string[] { "Nombre del Joven",  "Fecha de Ingreso", "Tutor Legal", "Estado" }
        };

                var rangeHeaders = worksheet.Cell(1, 1).InsertData(headers);
                rangeHeaders.Style.Font.Bold = true;
                rangeHeaders.Style.Fill.BackgroundColor = XLColor.LightGray;

                // Datos
                var data = expedientes.Select(e => new
                {
                    e.nombre_joven,
                    e.fecha_ingreso,
                    e.tutor_legal,
                    e.estado // Estado añadido a los datos
                }).ToList();

                worksheet.Cell(2, 1).InsertData(data);

                // Ajustar anchos de columna automáticamente
                worksheet.Columns().AdjustToContents();

                // Guardar en memoria
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Expedientes.xlsx");
                }
            }
        }

        // Método para exportar a CSV con formato similar al de Excel
        // Método para exportar a CSV con formato similar al de Excel y manejo de caracteres especiales
        public IActionResult ExportToCsv()
        {
            var expedientes = _context.Expedientes.ToList();

            using (var memoryStream = new MemoryStream())
            {
                // Usar StreamWriter con codificación UTF-8 para soportar caracteres especiales
                using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                {
                    // Escribir encabezados
                    streamWriter.WriteLine("Nombre del Joven,Fecha de Ingreso,Tutor Legal,Estado");

                    // Escribir los datos de los expedientes
                    foreach (var exp in expedientes)
                    {
                        streamWriter.WriteLine($"{exp.nombre_joven},{exp.fecha_ingreso.ToShortDateString()},{exp.tutor_legal},{exp.estado}");
                    }
                }

                // Retornar el archivo CSV con codificación UTF-8
                return File(memoryStream.ToArray(), "text/csv", "Expedientes.csv");
            }
        }



        public IActionResult ExportExpedienteToExcel(int id)
        {
            var expediente = _context.Expedientes.FirstOrDefault(e => e.Id == id);

            if (expediente == null)
            {
                return NotFound();
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Expediente");

                // Encabezados
                var headers = new List<string[]>
        {
            new string[] { "Campo", "Valor" }
        };

                var rangeHeaders = worksheet.Cell(1, 1).InsertData(headers);
                rangeHeaders.Style.Font.Bold = true;
                rangeHeaders.Style.Fill.BackgroundColor = XLColor.LightGray;

                // Datos
                var data = new List<string[]>
        {
            new string[] { "Nombre del Joven", expediente.nombre_joven },
            new string[] { "Fecha de Ingreso", expediente.fecha_ingreso.ToShortDateString() },
            new string[] { "Tutor Legal", expediente.tutor_legal },
            new string[] { "Estado", expediente.estado },
            new string[] { "Antecedentes Médicos", expediente.antecedentes_medicos ?? "N/A" },
            new string[] { "Historial Académico", expediente.historial_academico ?? "N/A" },
            new string[] { "Notas Adicionales", string.IsNullOrWhiteSpace(expediente.notas_adicionales) ? "No hay notas" : expediente.notas_adicionales } // Manejo de notas vacías
        };

                // Insertar los datos en la hoja de trabajo
                worksheet.Cell(2, 1).InsertData(data);

                // Ajustar anchos de columna automáticamente
                worksheet.Columns().AdjustToContents();

                // Guardar en memoria
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Expediente_{expediente.nombre_joven}.xlsx");
                }
            }
        }

        public IActionResult ExportExpedienteToPdf(int id)
        {
            var expediente = _context.Expedientes.FirstOrDefault(e => e.Id == id);

            if (expediente == null)
            {
                return NotFound();
            }

            using (var stream = new MemoryStream())
            {
                // Crear documento PDF
                Document pdfDoc = new Document(PageSize.A4);
                PdfWriter.GetInstance(pdfDoc, stream).CloseStream = false;
                pdfDoc.Open();

                // Cargar fuentes personalizadas
                var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                var titleFont = new Font(baseFont, 18, Font.BOLD, BaseColor.BLACK);
                var headerFont = new Font(baseFont, 12, Font.BOLD, BaseColor.WHITE);
                var cellFont = new Font(baseFont, 12, Font.NORMAL, BaseColor.BLACK);

                // Agregar logo
                var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logo.png");
                var logo = Image.GetInstance(logoPath);
                logo.ScaleToFit(100f, 100f);
                logo.Alignment = Image.ALIGN_LEFT;
                pdfDoc.Add(logo);

                // Agregar título con fecha dinámica
                var title = new Paragraph("Detalles del Expediente", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f
                };
                pdfDoc.Add(title);

                // Agregar detalles del expediente
                var details = new List<string[]>
        {
            new string[] { "Nombre del Joven", expediente.nombre_joven },
            new string[] { "Fecha de Ingreso", expediente.fecha_ingreso.ToShortDateString() },
            new string[] { "Tutor Legal", expediente.tutor_legal },
            new string[] { "Estado", expediente.estado },
            new string[] { "Antecedentes Médicos", expediente.antecedentes_medicos ?? "N/A" }, // Campo adicional
            new string[] { "Historial Académico", expediente.historial_academico ?? "N/A" }, // Campo adicional
            new string[] { "Notas Adicionales", expediente.notas_adicionales ?? "N/A" } // Campo adicional
        };

                // Crear tabla
                PdfPTable table = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    SpacingAfter = 10f
                };
                float[] widths = new float[] { 2f, 3f }; // Ajustar el ancho de las columnas
                table.SetWidths(widths);

                // Cabecera de la tabla
                table.AddCell(new PdfPCell(new Phrase("Campo", headerFont)) { BackgroundColor = BaseColor.DARK_GRAY, HorizontalAlignment = Element.ALIGN_CENTER, Padding = 8 });
                table.AddCell(new PdfPCell(new Phrase("Valor", headerFont)) { BackgroundColor = BaseColor.DARK_GRAY, HorizontalAlignment = Element.ALIGN_CENTER, Padding = 8 });

                // Añadir los detalles del expediente a la tabla
                foreach (var row in details)
                {
                    table.AddCell(new PdfPCell(new Phrase(row[0], cellFont)) { Padding = 8, HorizontalAlignment = Element.ALIGN_LEFT });
                    table.AddCell(new PdfPCell(new Phrase(row[1], cellFont)) { Padding = 8, HorizontalAlignment = Element.ALIGN_LEFT });
                }

                // Añadir la tabla al documento
                pdfDoc.Add(table);

                pdfDoc.Close();

                // Necesario para evitar el error de flujo cerrado
                stream.Flush();
                stream.Position = 0;

                return File(stream.ToArray(), "application/pdf", $"Expediente_{expediente.nombre_joven}.pdf");
            }
        }


        public IActionResult ExportExpedienteToCsv(int id)
        {
            var expediente = _context.Expedientes.FirstOrDefault(e => e.Id == id);

            if (expediente == null)
            {
                return NotFound();
            }

            var data = new List<string[]>
    {
        new string[] { "Campo", "Valor" },
        new string[] { "Nombre del Joven", expediente.nombre_joven },
        new string[] { "Fecha de Ingreso", expediente.fecha_ingreso.ToShortDateString() },
        new string[] { "Tutor Legal", expediente.tutor_legal },
        new string[] { "Estado", expediente.estado },
        new string[] { "Antecedentes Médicos", expediente.antecedentes_medicos ?? "N/A" },
        new string[] { "Historial Académico", expediente.historial_academico ?? "N/A" },
        new string[] { "Notas Adicionales", string.IsNullOrWhiteSpace(expediente.notas_adicionales) ? "No hay notas" : expediente.notas_adicionales } 
    };

            var builder = new StringBuilder();
            foreach (var line in data)
            {
                builder.AppendLine(string.Join(",", line.Select(x => $"\"{x.Replace("\"", "\"\"")}\"")));
            }

            var csv = Encoding.UTF8.GetBytes(builder.ToString());

            return File(csv, "text/csv", $"Expediente_{expediente.nombre_joven}.csv");
        }


    }
}