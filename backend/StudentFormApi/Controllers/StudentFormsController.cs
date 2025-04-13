using Microsoft.AspNetCore.Mvc;
using StudentFormApi.Data;
using StudentFormApi.Models;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
// Adaugă folosirea bibliotecii pentru generarea PDF; de exemplu, DinkToPdf
using DinkToPdf;
using DinkToPdf.Contracts;

namespace StudentFormApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentFormsController : ControllerBase
    {
        private readonly StudentFormContext _context;
        // Serviciu pentru generarea PDF
        private readonly IConverter _converter;

        public StudentFormsController(StudentFormContext context, IConverter converter)
        {
            _context = context;
            _converter = converter;
        }

        // POST: api/studentforms
        [HttpPost]
        public async Task<IActionResult> PostStudentForm([FromBody] StudentForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            form.DataSubmisiei = DateTime.Now;
            _context.StudentForms.Add(form);
            await _context.SaveChangesAsync();

            // Generare PDF folosind DinkToPdf
            var globalSettings = new GlobalSettings
            {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Portrait,
                DocumentTitle = "Fișa Studentului",
                Out = null  // nu salvăm pe disc, îl vom returna
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = $"<h1>Fișa Studentului</h1>" +
                              $"<p><strong>Nume:</strong> {form.Nume}</p>" +
                              $"<p><strong>Prenume:</strong> {form.Prenume}</p>" +
                              $"<p><strong>Facultate:</strong> {form.Facultate}</p>" +
                              $"<p><strong>Motivație:</strong> {form.Motivatie}</p>" +
                              $"<p><strong>Data submiterii:</strong> {form.DataSubmisiei}</p>" +
                              $"<p>Semnat electronic</p>",
                WebSettings = { DefaultEncoding = "utf-8" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            byte[] pdfData = _converter.Convert(pdf);

            // Returnăm fișierul PDF ca download
            return File(pdfData, "application/pdf", "FisăStudent.pdf");
        }

        // GET: api/studentforms
        [HttpGet]
        public async Task<IActionResult> GetStudentForms()
        {
            var forms = await _context.StudentForms.ToListAsync();
            return Ok(forms);
        }

        // GET: api/studentforms/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentForm(int id)
        {
            var form = await _context.StudentForms.FindAsync(id);
            if (form == null)
            {
                return NotFound();
            }
            return Ok(form);
        }

        // PUT: api/studentforms/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentForm(int id, [FromBody] StudentForm form)
        {
            if (id != form.Id)
                return BadRequest();

            _context.Entry(form).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _context.StudentForms.FindAsync(id) == null)
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        // DELETE: api/studentforms/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentForm(int id)
        {
            var form = await _context.StudentForms.FindAsync(id);
            if (form == null)
                return NotFound();

            _context.StudentForms.Remove(form);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
