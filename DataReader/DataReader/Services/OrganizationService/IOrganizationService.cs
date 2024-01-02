using DataReader.Models;

namespace DataReader.Services.OrganizationService
{
    public interface IOrganizationService
    {
        Task<bool> UploadAsync(List<OrganizationModel> model);
    }
}
