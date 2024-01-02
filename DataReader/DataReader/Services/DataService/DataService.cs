using DataReader.Data;
using DataReader.Models;
using DataReader.Services.OrganizationService;

namespace DataReader.Services.DataService
{
    public class DataService : IDataService
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrganizationService _organizationService;

        public DataService(ApplicationDbContext context, IOrganizationService organizationService)
        {
            _context = context;
            _organizationService = organizationService;
        }

        public async Task<bool> UploadAsync(List<OrganizationModel> model)
        {
            foreach (var organization in model) 
            {
                var doesExist = await _organizationService.ExistsAsync(organization.OrganizationId);
                if (doesExist)
                {
                    await _organizationService.UpdateAsync(organization);
                }
                else
                {
                    await _organizationService.CreateAsync(organization);
                }
            }

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

    }
}
