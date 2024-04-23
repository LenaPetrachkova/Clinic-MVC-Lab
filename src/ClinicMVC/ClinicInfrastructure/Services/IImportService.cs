using ClinicDomain.Model;
using DocumentFormat.OpenXml.Vml.Office;

namespace ClinicInfrastructure.Services
{
    public interface IImportService<TEntity>
         where TEntity : Entity

    {
        Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken);
    }

}
