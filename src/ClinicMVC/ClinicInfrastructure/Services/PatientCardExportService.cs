using ClinicDomain.Model;
using ClinicInfrastructure.Services;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClinicInfrastructure.Services
{
    public class PatientCardExportService : IExportService<PatientCard>
    {
        private readonly ClinicContext _context;

        public PatientCardExportService(ClinicContext context)
        {
            _context = context;
        }

        public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanWrite)
            {
                throw new ArgumentException("Input stream is not writable");
            }

            var patients = await _context.PatientCards
                .Include(p => p.Discount)
                .ToListAsync(cancellationToken);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(RootWorksheetName);
                WriteHeader(worksheet);
                WritePatients(worksheet, patients);
                workbook.SaveAs(stream);
            }
        }

        private static void WriteHeader(IXLWorksheet worksheet)
        {
            for (int columnIndex = 0; columnIndex < HeaderNames.Count; columnIndex++)
            {
                worksheet.Cell(1, columnIndex + 1).Value = HeaderNames[columnIndex];
            }
            worksheet.Row(1).Style.Font.Bold = true;
        }

        private void WritePatient(IXLWorksheet worksheet, PatientCard patient, int rowIndex)
        {
            var columnIndex = 1;
            worksheet.Cell(rowIndex, columnIndex++).Value = patient.LastName;
            worksheet.Cell(rowIndex, columnIndex++).Value = patient.FirstName;
            worksheet.Cell(rowIndex, columnIndex++).Value = patient.FatherName;
            worksheet.Cell(rowIndex, columnIndex++).Value = patient.PhoneNumber;
            worksheet.Cell(rowIndex, columnIndex++).Value = patient.DateOfBirth.ToString(); 
            worksheet.Cell(rowIndex, columnIndex++).Value = patient.AddInfo;
            worksheet.Cell(rowIndex, columnIndex++).Value = patient.Allergy;
            worksheet.Cell(rowIndex, columnIndex++).Value = patient.ChronicDisease;
            worksheet.Cell(rowIndex, columnIndex++).Value = patient.Diseases;
            worksheet.Cell(rowIndex, columnIndex++).Value = patient.Discount?.SocialGroup ?? ""; 
        }

        private void WritePatients(IXLWorksheet worksheet, ICollection<PatientCard> patients)
        {
            int rowIndex = 2;
            foreach (var patient in patients)
            {
                WritePatient(worksheet, patient, rowIndex);
                rowIndex++;
            }
        }

        private const string RootWorksheetName = "Patients";

        private static readonly IReadOnlyList<string> HeaderNames = new string[]
        {
            "Прізвище",
            "Ім'я",
            "По-батькові",
            "Номер телефону",
            "Дата народження",
            "Додаткова інформація",
            "Алергії",
            "Хронічні хвороби",
            "Хвороби",
            "Соціальна група"
        };
    }
}
