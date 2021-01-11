using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Core.Abstraction.Services.FileService;
using MeetAndPlay.Data.DTO.Files;
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
        private readonly IPictureRandomizer _pictureRandomizer;

        public FilesController(IFilesService filesService, IPictureRandomizer pictureRandomizer)
        {
            _filesService = filesService;
            _pictureRandomizer = pictureRandomizer;
        }

        [DisableRequestSizeLimit]
        [HttpPost("UploadForm")]
        public async Task<File> UploadFile(IFormFile file)
        {
            var result = await _filesService.UploadFileAsync(file);
            return result;
        }
        
        [DisableRequestSizeLimit]
        [HttpPost("UploadBase64")]
        public async Task<File> UploadFile([FromBody]Base64UploadRequest uploadRequest)
        {
            var fileInfo = new FileInfo {Filename = uploadRequest.Filename};
            var result = await _filesService.UploadFileAsync(uploadRequest.Base64Source, fileInfo);
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

        [HttpGet("Lobbies/RandomPicture")]
        public string GetRandomLobbyPicture()
        {
            return _pictureRandomizer.GetRandomLobbyPictureLink();
        }
        
        [HttpGet("Offers/RandomPicture")]
        public string GetRandomOfferPicture()
        {
            return _pictureRandomizer.GetRandomOfferPictureLink();
        }
        
        [HttpGet("Players/RandomPicture")]
        public string GetPlayersOfferPicture()
        {
            return _pictureRandomizer.GetRandomPlayerPictureLink();
        }
    }
}