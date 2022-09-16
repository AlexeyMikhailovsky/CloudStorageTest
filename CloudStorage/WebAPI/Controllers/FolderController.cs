using CloudStorage.Controllers;
using CloudStorage.WebAPI.DataSource;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CloudStorage.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FolderController : ControllerBase
    {
        IDataSource dataSource;
        private readonly ILogger<FolderController> _logger;

        public FolderController(ILogger<FolderController> logger)
        {
            _logger = logger;
            dataSource = new SQLDataSource();
        }

        [HttpGet]
        [Route("getfolder")]
        public async Task<IActionResult> GetFolder([FromQuery] int id)
        {
            try
            {
                return Ok(dataSource.GetFolder(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete]
        [Route("deletefolder")]
        public async Task<IActionResult> DeleteFolder([FromQuery] int id)
        {
            try
            {
                return Ok(dataSource.DeleteFolder(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPut]
        [Route("updatefolder")]
        public async Task<IActionResult> UpdateFolder([FromQuery] int id,[FromQuery] string name)
        {
            try
            {
                return Ok(dataSource.UpdateFolder(id,name));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost]
        [Route("AddDepartment")]
        public async Task<IActionResult> AddFolder([FromQuery] int id, [FromQuery] string name)
        {
            try
            {
                return Ok(dataSource.CreateFolder(id, name));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
