using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using File = MeetAndPlay.Data.Models.Files.File;

namespace MeetAndPlay.Core.Abstraction.Services.FileService
{
    public interface IFilesService
    {
        Task<File> UploadFileAsync(Stream fileStream, FileInfo fileInfo, bool needToSaveChanges = true);
        Task<File> UploadFileAsync(IFormFile file, bool needToSaveChanges = true);
        Task<File> UploadFileAsync(string base64String, FileInfo fileInfo, bool needToSaveChanges = true);
        Task<bool> IsFileExistsAsync(string fileHash);
        Task<File> GetFileAsync(string fileHash);
        Task<string> GetFileMimeTypeAsync(string fileHash);
        Stream GetFileStream(string fileHash);
    }
}