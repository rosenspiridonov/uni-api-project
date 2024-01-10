using DataReader.Models;
using DataReader.Services.OrganizationService;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static DataReader.Constants;

namespace DataReader.Controllers
{
    public class OrganizationController : ApiController
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet("{organizationId}")]
        [Authorize]
        public async Task<IActionResult> GetAsync(string organizationId)
        {
            var model = await _organizationService.GetAsync(organizationId);
            if (model is null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("top-3")]
        public async Task<IActionResult> GetTop3()
        {
            var model = await _organizationService.GetTop3BiggestOrganizations();
            if (model is null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet("industry-employees")]
        [Authorize]
        public async Task<IActionResult> GetIndustryEmployees()
        {
            var model = await _organizationService.GetNumberOfEmployeesForEachIndustry();
            if (model is null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpPost("upload")]
        [Authorize]
        public async Task<IActionResult> UploadAsync([FromBody] List<OrganizationModel> data)
        {
            var succeeded = await _organizationService.UploadAsync(data);
            if (!succeeded)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("{organizationId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteAsync(string organizationId)
        {
            var succeeded = await _organizationService.DeleteAsync(organizationId);
            if (!succeeded)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
