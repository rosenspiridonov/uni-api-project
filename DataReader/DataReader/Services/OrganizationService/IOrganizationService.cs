using DataReader.Models;

namespace DataReader.Services.OrganizationService
{
    public interface IOrganizationService
    {
        Task<OrganizationModel> GetAsync(string id);

        Task<IEnumerable<OrganizationModel>> GetTop3BiggestOrganizations();

        Task<IEnumerable<IndustryEmployeesModel>> GetNumberOfEmployeesForEachIndustry();

        Task<bool> UploadAsync(List<OrganizationModel> model);

        Task<bool> DeleteAsync(string id);
    }
}
