using DataReader.Models;
using DataReader.Services.DataService;
using DataReader.Services.OrganizationService;

using Microsoft.AspNetCore.Mvc;

namespace DataReader.Controllers
{
    public class OrganizationController : ApiController
    {
        /* TODO
         * - DB Entities
         * - Endpoints for the data - CRUD
         * - Services - CRUD
         * - Generate JSON file daily - Cron job
        */
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet("{organizationId}")]
        public async Task<IActionResult> GetAsync(string organizationId)
        {
            var model = await _organizationService.GetAsync(organizationId);

            if (model is null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadAsync([FromBody] List<OrganizationModel> data)
        {
            var succeeded = await _organizationService.UploadAsync(data);
            if (!succeeded)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
