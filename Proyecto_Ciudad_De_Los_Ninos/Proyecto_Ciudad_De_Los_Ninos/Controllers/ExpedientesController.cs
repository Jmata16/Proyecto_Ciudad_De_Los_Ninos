using System;
using System.IO;
using ClosedXML.Excel;
using CsvHelper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Ciudad_De_Los_Ninos.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Ciudad_De_Los_Ninos.Models;
using System.Drawing.Printing;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Authorization;

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


        // Método de búsqueda
        public async Task<IActionResult> Index(string searchString, int? searchAge)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentAgeFilter"] = searchAge;

            var expedientes = from e in _context.Expedientes
                              select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                expedientes = expedientes.Where(e => e.nombre_joven.Contains(searchString));
            }

            if (searchAge != null)
            {
                expedientes = expedientes.Where(e => e.edad == searchAge);
            }

            return View(await expedientes.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("Id,id_joven,nombre_joven,edad,fecha_ingreso,direccion,telefono_contacto,tutor_legal,antecedentes_medicos,historial_academico,notas_adicionales")] Expedientes expediente)
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

        // POST: Expedientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expediente = await _context.Expedientes.FindAsync(id);
            _context.Expedientes.Remove(expediente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
                PdfPTable table = new PdfPTable(6)
                {
                    WidthPercentage = 100,
                    SpacingBefore = 10f,
                    SpacingAfter = 20f
                };
                table.SetWidths(new float[] { 2f, 1f, 1.5f, 2f, 2f, 2f });

                // Agregar encabezado de la tabla
                var headers = new string[] { "Nombre del Joven", "Edad", "Fecha de Ingreso", "Dirección", "Teléfono de Contacto", "Tutor Legal" };
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
                    table.AddCell(new PdfPCell(new Phrase(exp.edad.ToString(), cellFont)) { Padding = 5 });
                    table.AddCell(new PdfPCell(new Phrase(exp.fecha_ingreso.ToShortDateString(), cellFont)) { Padding = 5 });
                    table.AddCell(new PdfPCell(new Phrase(exp.direccion, cellFont)) { Padding = 5 });
                    table.AddCell(new PdfPCell(new Phrase(exp.telefono_contacto, cellFont)) { Padding = 5 });
                    table.AddCell(new PdfPCell(new Phrase(exp.tutor_legal, cellFont)) { Padding = 5 });
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
            new string[] { "Nombre del Joven", "Edad", "Fecha de Ingreso", "Dirección", "Teléfono de Contacto", "Tutor Legal" }
        };

                var rangeHeaders = worksheet.Cell(1, 1).InsertData(headers);
                rangeHeaders.Style.Font.Bold = true;
                rangeHeaders.Style.Fill.BackgroundColor = XLColor.LightGray;

                // Datos
                var data = expedientes.Select(e => new
                {
                    e.nombre_joven,
                    e.edad,
                    e.fecha_ingreso,
                    e.direccion,
                    e.telefono_contacto,
                    e.tutor_legal
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

        // Método para exportar a CSV
        public IActionResult ExportToCsv()
        {
            var expedientes = _context.Expedientes.ToList();
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteHeader<Expedientes>();
                    csvWriter.NextRecord();
                    foreach (var exp in expedientes)
                    {
                        csvWriter.WriteRecord(exp);
                        csvWriter.NextRecord();
                    }
                }
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
            new string[] { "Edad", expediente.edad.ToString() },
            new string[] { "Fecha de Ingreso", expediente.fecha_ingreso.ToShortDateString() },
            new string[] { "Dirección", expediente.direccion },
            new string[] { "Teléfono de Contacto", expediente.telefono_contacto },
            new string[] { "Tutor Legal", expediente.tutor_legal }
        };

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

                // Agregar mensaje introductorio
                var intro = new Paragraph($"Detalles del expediente para: {expediente.nombre_joven}", cellFont)
                {
                    SpacingAfter = 10f
                };
                pdfDoc.Add(intro);

                // Crear la tabla con estilos
                PdfPTable table = new PdfPTable(2)
                {
                    WidthPercentage = 60,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    SpacingBefore = 10f,
                    SpacingAfter = 20f
                };
                table.SetWidths(new float[] { 1f, 3f });

                // Agregar filas a la tabla
                table.AddCell(new PdfPCell(new Phrase("Nombre del Joven:", headerFont)) { Padding = 5, BackgroundColor = new BaseColor(0, 121, 241) });
                table.AddCell(new PdfPCell(new Phrase(expediente.nombre_joven, cellFont)) { Padding = 5 });

                table.AddCell(new PdfPCell(new Phrase("Edad:", headerFont)) { Padding = 5, BackgroundColor = new BaseColor(0, 121, 241) });
                table.AddCell(new PdfPCell(new Phrase(expediente.edad.ToString(), cellFont)) { Padding = 5 });

                table.AddCell(new PdfPCell(new Phrase("Fecha de Ingreso:", headerFont)) { Padding = 5, BackgroundColor = new BaseColor(0, 121, 241) });
                table.AddCell(new PdfPCell(new Phrase(expediente.fecha_ingreso.ToShortDateString(), cellFont)) { Padding = 5 });

                table.AddCell(new PdfPCell(new Phrase("Dirección:", headerFont)) { Padding = 5, BackgroundColor = new BaseColor(0, 121, 241) });
                table.AddCell(new PdfPCell(new Phrase(expediente.direccion, cellFont)) { Padding = 5 });

                table.AddCell(new PdfPCell(new Phrase("Teléfono de Contacto:", headerFont)) { Padding = 5, BackgroundColor = new BaseColor(0, 121, 241) });
                table.AddCell(new PdfPCell(new Phrase(expediente.telefono_contacto, cellFont)) { Padding = 5 });

                table.AddCell(new PdfPCell(new Phrase("Tutor Legal:", headerFont)) { Padding = 5, BackgroundColor = new BaseColor(0, 121, 241) });
                table.AddCell(new PdfPCell(new Phrase(expediente.tutor_legal, cellFont)) { Padding = 5 });

                pdfDoc.Add(table);
                pdfDoc.Close();

                // Necesario para evitar el error de flujo cerrado
                stream.Flush();
                stream.Position = 0;

                // Devolver el archivo PDF como una descarga
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
        new string[] { "Edad", expediente.edad.ToString() },
        new string[] { "Fecha de Ingreso", expediente.fecha_ingreso.ToShortDateString() },
        new string[] { "Dirección", expediente.direccion },
        new string[] { "Teléfono de Contacto", expediente.telefono_contacto },
        new string[] { "Tutor Legal", expediente.tutor_legal }
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

