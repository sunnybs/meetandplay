using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using File = MeetAndPlay.Data.Models.Files.File;

namespace MeetAndPlay.Core.Abstraction.Services
{
    public interface IFilesService
    {
        public Task<File> UploadFileAsync(Stream fileStream);
        public Task<File> UploadFileAsync(IFormFile file);
        public Task<File> UploadFileAsync(string base64String);
        public Stream GetFileStream(string fileHash);
    }
}