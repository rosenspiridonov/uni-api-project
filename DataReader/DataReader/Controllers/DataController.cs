using DataReader.Models;
using DataReader.Services.DataService;

using Microsoft.AspNetCore.Mvc;

namespace DataReader.Controllers
{
    public class DataController : ApiController
    {
        /* TODO
         * - DB Entities
         * - Endpoints for the data - CRUD
         * - Services - CRUD
         * - Generate JSON file daily - Cron job
        */
        private readonly IDataService _dataService;

        public DataController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadAsync([FromBody] List<OrganizationModel> data)
        {
            var succeeded = await _dataService.UploadAsync(data);
            if (!succeeded)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
