using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClinicDomain.Model;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace ClinicInfrastructure.Services
{
    public class PatientListImportService : IImportService<PatientCard>
    {
        private readonly ClinicContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PatientListImportService(ClinicContext context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanRead)
            {
                throw new ArgumentException("Дані не можуть бути прочитані", nameof(stream));
            }

            // Отримати HttpContext поточного запиту
            var httpContext = _httpContextAccessor.HttpContext;

            // Отримати поточного користувача
            var currentUser = await _userManager.GetUserAsync(httpContext.User);

            // Перевірити, чи користувач залогінений
            if (currentUser != null)
            {
                // Отримати ідентифікатор клініки поточного користувача
                int clinicId = currentUser.ClinicId;

                using (XLWorkbook workBook = new XLWorkbook(stream))
                {
                    foreach (IXLWorksheet worksheet in workBook.Worksheets)
                    {
                        foreach (var row in worksheet.RowsUsed().Skip(1))
                        {
                            await AddPatientCardAsync(row, cancellationToken, clinicId);
                        }
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        private async Task AddPatientCardAsync(IXLRow row, CancellationToken cancellationToken, int clinicId)
        {
            PatientCard patientCard = new PatientCard();
            patientCard.LastName = GetLastName(row);
            patientCard.FirstName = GetFirstName(row);
            patientCard.FatherName = GetFatherName(row);
            patientCard.PhoneNumber = GetPhoneNumber(row);
            patientCard.DateOfBirth = GetDateOfBirth(row.Cell(5));
            patientCard.AddInfo = GetAddInfo(row);
            patientCard.Allergy = GetAllergy(row);
            patientCard.ChronicDisease = GetChronicDisease(row);
            patientCard.Diseases = GetDisease(row);

            var cellValue = row.Cell(10).Value;
            if (!string.IsNullOrEmpty(cellValue.ToString()))
            {
                var socialGroup = cellValue.ToString();

                var discount = await _context.Discounts.FirstOrDefaultAsync(d => d.SocialGroup == socialGroup, cancellationToken);
                if (discount is null)
                {
                    discount = new Discount();
                    discount.SocialGroup = socialGroup;
                    _context.Discounts.Add(discount);
                }

                patientCard.Discount = discount;
            }

            patientCard.ClinicId = clinicId;

            _context.PatientCards.Add(patientCard);
            await _context.SaveChangesAsync(cancellationToken);
        }


        private static string GetFirstName(IXLRow row)
        {
            return row.Cell(2).Value.ToString();
        }
        private static string GetLastName(IXLRow row)
        {
            return row.Cell(1).Value.ToString();
        }
        private static string GetFatherName(IXLRow row)
        {
            return row.Cell(3).Value.ToString();
        }
        private static string GetPhoneNumber(IXLRow row)
        {
            return row.Cell(4).Value.ToString();
        }
        private static string GetAddInfo(IXLRow row)
        {
            return row.Cell(6).Value.ToString();
        }
        private static string GetAllergy(IXLRow row)
        {
            return row.Cell(7).Value.ToString();
        }
        private static string GetChronicDisease(IXLRow row)
        {
            return row.Cell(8).Value.ToString();
        }
        private static string GetDisease(IXLRow row)
        {
            return row.Cell(9).Value.ToString();
        }

        private DateOnly GetDateOfBirth(IXLCell cell)
        {
            if (!(cell.IsEmpty() || cell.Value.ToString() == string.Empty) && DateTime.TryParse(cell.Value.ToString(), out DateTime date))
            {
                return DateOnly.FromDateTime(date);
            }
            return default;
        }
    }
}
