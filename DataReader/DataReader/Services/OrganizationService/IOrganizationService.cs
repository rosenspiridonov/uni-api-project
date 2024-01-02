using DataReader.Models;

namespace DataReader.Services.OrganizationService
{
    public interface IOrganizationService
    {
        Task<bool> ExistsAsync(string id);

        Task CreateAsync(OrganizationModel model);

        Task UpdateAsync(OrganizationModel model);
    }
}
