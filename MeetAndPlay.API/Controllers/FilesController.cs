using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services.FileService;
using MeetAndPlay.Data.Models.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeetAndPlay.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFilesService _filesService;

        public FilesController(IFilesService filesService)
        {
            _filesService = filesService;
        }

        [HttpPost("UploadForm")]
        public async Task<File> UploadFile(IFormFile file)
        {
            var result = await _filesService.UploadFileAsync(file);
            return result;
        }
        
        [HttpPost("UploadBase64")]
        public async Task<File> UploadFile(string base64Source, string filename)
        {
            var fileInfo = new FileInfo {Filename = filename};
            var result = await _filesService.UploadFileAsync(base64Source, fileInfo);
            return result;
        }

        [HttpGet("{fileHash}")]
        public async Task<IActionResult> Get(string fileHash)
        {
            if (!await _filesService.IsFileExistsAsync(fileHash))
            {
                return NotFound($"File with hash {fileHash} not found.");
            }
            
            var fileStream = _filesService.GetFileStream(fileHash);
            var fileMimeType = await _filesService.GetFileMimeTypeAsync(fileHash);
            return File(fileStream, fileMimeType);
        }
    }
}