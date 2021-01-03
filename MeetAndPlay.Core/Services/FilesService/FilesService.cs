using System.IO;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Core.Infrastructure;
using Microsoft.AspNetCore.Http;
using File = MeetAndPlay.Data.Models.Files.File;

namespace MeetAndPlay.Core.Services.FilesService
{
    public class FilesService : IFilesService
    {
        public FilesService(MNPContext context)
        {
            
        }


        public Task<File> UploadFileAsync(Stream fileStream)
        {
            throw new System.NotImplementedException();
        }

        public Task<File> UploadFileAsync(IFormFile file)
        {
            throw new System.NotImplementedException();
        }

        public Task<File> UploadFileAsync(string base64String)
        {
            throw new System.NotImplementedException();
        }

        public Stream GetFileStream(string fileHash)
        {
            throw new System.NotImplementedException();
        }
    }
}