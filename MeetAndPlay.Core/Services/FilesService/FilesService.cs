using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Core.Abstraction.Services.FileService;
using MeetAndPlay.Core.Infrastructure;
using MeetAndPlay.Core.Infrastructure.Exceptions;
using MeetAndPlay.Core.Infrastructure.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using File = MeetAndPlay.Data.Models.Files.File;
using FileInfo = MeetAndPlay.Core.Abstraction.Services.FileService.FileInfo;

namespace MeetAndPlay.Core.Services.FilesService
{
    public class FilesService : IFilesService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<FilesService> _logger;
        private readonly MNPContext _mnpContext;

        public FilesService(MNPContext mnpContext, IWebHostEnvironment environment, ILogger<FilesService> logger)
        {
            _mnpContext = mnpContext;
            _environment = environment;
            _logger = logger;
        }

        public async Task<File> UploadFileAsync(IFormFile file, bool needToSaveChanges = true)
        {
            var stream = file.OpenReadStream();
            var fileInfo = new FileInfo
            {
                Filename = file.FileName,
                MimeType = file.ContentType
            };
            return await UploadFileAsync(stream, fileInfo, needToSaveChanges);
        }

        public async Task<bool> IsFileExistsAsync(string fileHash)
        {
            return await _mnpContext.Files.AnyAsync(f => f.Hash == fileHash);
        }

        public Stream GetFileStream(string fileHash)
        {
            var filePath = BuildFilePath(fileHash);
            if (!System.IO.File.Exists(filePath))
                throw new NotFoundException($"File with hash {fileHash} not found");
            
            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }

        public async Task<File> GetFileAsync(string fileHash)
        {
            var file = await _mnpContext.Files.SingleOrDefaultAsync(f => f.Hash == fileHash);
            if (file != null) 
                return file;
            
            throw new NotFoundException($"File with hash {fileHash} not found");
        }
        
        public async Task<string> GetFileMimeTypeAsync(string fileHash)
        {
            var file = await _mnpContext.Files.SingleOrDefaultAsync(f => f.Hash == fileHash);
            if (file != null) 
                return file.MimeType;
            
            _logger.LogInformation($"File with hash {fileHash} not found");
            return "application/octet-stream";

        }

        public async Task<File> UploadFileAsync(Stream stream, FileInfo fileInfo, bool needToSaveChanges = true)
        {
            EnsureArgumentsAreValid(stream, fileInfo);
            var fileHash = fileInfo.Hash;
            if (fileHash.IsNullOrWhiteSpace()) fileHash = ComputeMd5Hash(stream);
            var fileInDb = await _mnpContext.Files.AsNoTracking().SingleOrDefaultAsync(f => f.Hash == fileHash);
            if (fileInDb != null)
            {
                _logger.LogInformation($"File with hash {fileHash} already uploaded.");
                return fileInDb;
            }

            var mimeType = fileInfo.MimeType ?? MimeTypes.GetMimeType(fileInfo.Extension);
            var filePath = BuildFilePath(fileHash);
            var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            await stream.CopyToAsync(fileStream);
            await fileStream.DisposeAsync();

            var file = new File
            {
                CreationDate = DateTime.Now,
                FileLink = "/Files/" + fileHash,
                Filename = fileInfo.Filename ?? fileHash,
                Hash = fileHash,
                MimeType = mimeType
            };

            if (needToSaveChanges)
            {
                await _mnpContext.AddAsync(file);
                await _mnpContext.SaveChangesAsync();
            }
            
            return file;
        }

        private static void EnsureArgumentsAreValid(Stream stream, FileInfo fileInfo)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (fileInfo == null)
                throw new ArgumentNullException(nameof(fileInfo));
            if (!fileInfo.MimeType.IsNullOrWhiteSpace()) 
                return;
            if (fileInfo.Filename.IsNullOrWhiteSpace() && fileInfo.Extension.IsNullOrWhiteSpace())
                throw new ArgumentException("Filename and extension are null.");
            if (!fileInfo.Extension.IsNullOrWhiteSpace()) 
                return;
            fileInfo.Extension = Path.GetExtension(fileInfo.Filename);
            if (fileInfo.Extension.IsNullOrWhiteSpace())
                throw new ArgumentException("After parsing from filename extension is still empty.");
        }

        public async Task<File> UploadFileAsync(string base64String, FileInfo fileInfo, bool needToSaveChanges = true)
        {
            var stream = GetStreamFromBase64File(base64String);
            var mimeType = GetMimeTypeFromBase64File(base64String);
            fileInfo ??= new FileInfo();
            fileInfo.MimeType = mimeType;
            return await UploadFileAsync(stream, fileInfo, needToSaveChanges);
        }

        private string ComputeMd5Hash(Stream stream)
        {
            using var md5 = MD5.Create();
            var byteHashValue = md5.ComputeHash(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return ToHex(byteHashValue);
        }

        private string ToHex(byte[] bytes)
        {
            return string.Concat(bytes.Select(x => x.ToString("x2")));
        }

        private string BuildFilePath(string fileHash)
        {
            if (!Directory.Exists(_environment.WebRootPath + "/uploads"))
                Directory.CreateDirectory(_environment.WebRootPath + "/uploads");
            
            return _environment.WebRootPath + "/uploads/" + fileHash;
        }

        private string GetMimeTypeFromBase64File(string base64String)
        {
            // формат data:[<тип данных>][;base64],<данные>
            var startPosition = 5;
            var endPosition = base64String.IndexOf(';');
            var mimeTypeLength = endPosition - startPosition;
            var result = base64String.Substring(startPosition, mimeTypeLength);

            return result;
        }

        private MemoryStream GetStreamFromBase64File(string base64String)
        {
            // позиция разделителя data:[<тип данных>][;base64],<данные>
            var splitterPosition = base64String.IndexOf(',');
            var bytes = Convert.FromBase64String(base64String.Substring(splitterPosition + 1));
            var stream = new MemoryStream(bytes);
            return stream;
        }
    }
}