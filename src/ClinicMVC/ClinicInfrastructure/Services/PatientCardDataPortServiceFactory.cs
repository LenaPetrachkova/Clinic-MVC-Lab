using ClinicDomain.Model;
using ClinicInfrastructure.Services;
using Microsoft.AspNetCore.Identity;

namespace ClinicInfrastructure.Services
{
    public class PatientCardDataPortServiceFactory : IDataPortServiceFactory<PatientCard>
    {
        private readonly ClinicContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PatientCardDataPortServiceFactory(ClinicContext context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public IImportService<PatientCard> GetImportService(string contentType)
        {
            if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new PatientListImportService(_context, _userManager, _httpContextAccessor);
            }
            throw new NotImplementedException($"No import service implemented for movies with content type {contentType}");
        }

        public IExportService<PatientCard> GetExportService(string contentType)
        {
            if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new PatientCardExportService(_context);
            }
            throw new NotImplementedException($"No export service implemented for movies with content type {contentType}");
        }
    }
}
