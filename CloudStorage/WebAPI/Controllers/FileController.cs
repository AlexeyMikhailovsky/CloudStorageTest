using CloudStorage.WebAPI;
using CloudStorage.WebAPI.DataSource;
using CloudStorage.WebAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Data.Sqlite;
using System.Net.Http.Headers;

namespace CloudStorage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        IDataSource dataSource;
        private readonly ILogger<FileController> _logger;

        public FileController(ILogger<FileController> logger)
        {
            _logger = logger;
            dataSource = new SQLDataSource();
        }

        [HttpGet]
        [Route("getlist")]
        public async Task<IActionResult> GetList()
        {
            try
            {
                return Ok(dataSource.GetFiles());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("upload")]
        public async Task<IActionResult> Upload([FromQuery] int id)
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                var folderName = Path.Combine("Resources", "Files");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    FileInfo fileInfo = new FileInfo(fullPath);
                    DateTime dt = fileInfo.CreationTime;
                    MyFile myFile = new MyFile();
                    myFile.IdFolder = id;
                    myFile.Location = dbPath;
                    myFile.Name = fileName;
                    myFile.CreationDate = dt.ToString();
                    myFile.UploadDate = DateTime.Now.ToString();
                    myFile.Type = file.ContentType;
                    myFile.Size = fileInfo.Length.ToString();
                    return Ok(dataSource.UploadFile(myFile));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet, DisableRequestSizeLimit]
        [Route("download")]
        public async Task<IActionResult> Download([FromQuery] string fileUrl)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileUrl);
            if (!System.IO.File.Exists(filePath))
                return NotFound();
            var memory = new MemoryStream();
            await using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filePath), filePath);
        }

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        [HttpGet, DisableRequestSizeLimit]
        [Route("getFiles")]
        public async Task<IActionResult> GetFiles()
        {
            try
            {
                var folderName = Path.Combine("Resources", "Files");
                var pathToRead = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var files = Directory.EnumerateFiles(pathToRead)
                    .Select(fullPath => Path.Combine(folderName, Path.GetFileName(fullPath)));
                return Ok(new { files });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet, DisableRequestSizeLimit]
        [Route("getFile")]
        public async Task<IActionResult> GetFile([FromQuery] int id)
        {
            try
            {
                return Ok(dataSource.GetFile(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet, DisableRequestSizeLimit]
        [Route("getFilesFromFolder")]
        public async Task<IActionResult> GetFilesFromFolder([FromQuery] int id)
        {
            try
            {
                return Ok(dataSource.GetFilesFromFolder(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete]
        [Route("deleteFile")]
        public async Task<IActionResult> DeleteFile([FromQuery] int id)
        {
            try
            {
                return Ok(dataSource.DeleteFile(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}