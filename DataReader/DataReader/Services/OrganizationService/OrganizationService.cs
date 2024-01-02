using DataReader.Data;
using DataReader.Data.Entities;
using DataReader.Models;

using Microsoft.EntityFrameworkCore;

namespace DataReader.Services.OrganizationService
{
    public class OrganizationService : IOrganizationService
    {
        private readonly ApplicationDbContext _context;

        public OrganizationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(string id) => await _context.Organizations.AnyAsync(x => x.Id == id);

        public async Task CreateAsync(OrganizationModel model)
        {
            var entity = new Organization() { Id = model.OrganizationId };
            await SetEntityValuesAsync(model, entity);

            await _context.Organizations.AddAsync(entity);
        }

        public async Task UpdateAsync(OrganizationModel model)
        {
            var entity = await _context.Organizations.FindAsync(model.OrganizationId);
            await SetEntityValuesAsync(model, entity);

            _context.Organizations.Update(entity);
        }

        private async Task SetEntityValuesAsync(OrganizationModel model, Organization? entity)
        {
            entity.Name = model.Name;
            entity.Website = model.Website;
            entity.Country = (await GetCountryAsync(model.Country)) ?? new Country() { Name = model.Country };
            entity.Description = model.Description;
            entity.Founded = model.Founded;
            entity.Industry = (await GetIndustryAsync(model.Country)) ?? new Industry() { Name = model.Industry };
            entity.NumberOfEmployees = model.NumberOfEmployees;
        }

        private async Task<Country> GetCountryAsync(string name) => await _context.Countries.FirstOrDefaultAsync(x => x.Name == name);

        private async Task<Industry> GetIndustryAsync(string name) => await _context.Industries.FirstOrDefaultAsync(x => x.Name == name);
    }
}
