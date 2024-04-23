using ClinicDomain.Model;

namespace ClinicInfrastructure.Services
{
    public interface IExportService<TEntity>
       where TEntity : Entity
    {
        Task WriteToAsync(Stream stream, CancellationToken cancellationToken);
    }

}
