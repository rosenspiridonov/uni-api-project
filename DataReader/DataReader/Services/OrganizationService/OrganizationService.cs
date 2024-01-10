using DataReader.Data;
using DataReader.Data.Entities;
using DataReader.Models;

using Microsoft.EntityFrameworkCore;

namespace DataReader.Services.OrganizationService
{
    public class OrganizationService : IOrganizationService
    {
        private readonly ApplicationDbContext _context;

        private List<Country> _preloadedCountries;
        private List<Industry> _preloadedIndustries;

        public OrganizationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrganizationModel> GetAsync(string id)
        {
            var model = await _context.Organizations
                .Where(x => x.Id == id && !x.IsDeleted)
                .Select(x => new OrganizationModel()
                {
                    Name = x.Name,
                    Website = x.Website,
                    Country = x.Country.Name,
                    Description = x.Description,
                    Founded = x.Founded,
                    Industry = x.Industry.Name,
                    NumberOfEmployees = x.NumberOfEmployees,
                })
                .SingleOrDefaultAsync();

            return model;
        }

        #region Stats

        public async Task<IEnumerable<OrganizationModel>> GetTop3BiggestOrganizations()
        {
            var model = await _context.Organizations
                .Where(x => !x.IsDeleted)
                .Select(x => new OrganizationModel()
                {
                    Name = x.Name,
                    Website = x.Website,
                    Country = x.Country.Name,
                    Description = x.Description,
                    Founded = x.Founded,
                    Industry = x.Industry.Name,
                    NumberOfEmployees = x.NumberOfEmployees,
                })
                .OrderByDescending(x => x.NumberOfEmployees)
                .Take(3)
                .ToListAsync();

            return model;
        }

        public async Task<IEnumerable<IndustryEmployeesModel>> GetNumberOfEmployeesForEachIndustry()
        {
            var model = await _context.Organizations
                .Where(x => !x.IsDeleted)
                .GroupBy(x => x.Industry.Name)
                .Select(g => new IndustryEmployeesModel
                {
                    Industry = g.Key,
                    NumberOfEmployees = g.Sum(o => o.NumberOfEmployees),
                })
                .ToListAsync();

            return model;
        }

        #endregion

        public async Task<bool> UploadAsync(List<OrganizationModel> model)
        {
            await PreloadDataAsync();

            var organizationIds = model.Select(m => m.OrganizationId).ToList();
            var existingOrganizations = await _context
                .Organizations
                .Where(o => organizationIds.Contains(o.Id))
                .Select(o => o.Id)
                .ToListAsync();

            var organizationsToCreate = new List<Organization>();
            var organizationsToUpdate = new List<Organization>();

            foreach (var organization in model)
            {
                var entity = new Organization();
                await SetEntityValuesAsync(organization, entity);

                if (existingOrganizations.Contains(organization.OrganizationId))
                {
                    organizationsToUpdate.Add(entity);
                }
                else
                {
                    organizationsToCreate.Add(entity);
                }
            }

            await _context.Organizations.AddRangeAsync(organizationsToCreate);
            _context.Organizations.UpdateRange(organizationsToUpdate);

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _context.Organizations.FindAsync(id);
            if (entity is null)
            {
                return false;
            }

            entity.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task PreloadDataAsync()
        {
            _preloadedCountries = await _context.Countries.ToListAsync();
            _preloadedIndustries = await _context.Industries.ToListAsync();
        }

        private async Task SetEntityValuesAsync(OrganizationModel model, Organization entity)
        {
            entity.Id = model.OrganizationId;
            entity.Name = model.Name;
            entity.Website = model.Website;
            entity.Description = model.Description;
            entity.Founded = model.Founded;
            entity.NumberOfEmployees = model.NumberOfEmployees;

            var country = _preloadedCountries.FirstOrDefault(c => c.Name == model.Country);
            if (country == null)
            {
                country = new Country() { Name = model.Country };
                _preloadedCountries.Add(country);
                await _context.Countries.AddAsync(country);
            }

            entity.Country = country;

            var industry = _preloadedIndustries.FirstOrDefault(i => i.Name == model.Industry);
            if (industry == null)
            {
                industry = new Industry() { Name = model.Industry };
                _preloadedIndustries.Add(industry);
                await _context.Industries.AddAsync(industry);
            }

            entity.Industry = industry;
        }
    }
}
