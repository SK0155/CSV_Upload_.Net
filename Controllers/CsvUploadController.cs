using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CsvUpload.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace CsvUpload.Controllers
{
    [Route("api/CsvUpload")]
    [ApiController]
    public class CsvUploadController : ControllerBase
    {
        private readonly CsvService _csvService;

        public CsvUploadController(CsvService csvService)
        {
            _csvService = csvService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            var (success, message) = await _csvService.UploadCsvAsync(file);

            if (!success)
            {
                return BadRequest(message);
            }

            return Ok(message);
        }
    }
}