using DataReader.Models;

namespace DataReader.Services.DataService
{
    public interface IDataService
    {
        Task<bool> UploadAsync(List<OrganizationModel> model);
    }
}
