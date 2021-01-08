using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MeetAndPlay.Data.Models.Commons;
using MeetAndPlay.Data.Models.Files;
using MeetAndPlay.Web.Options;
using MeetAndPlay.Web.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace MeetAndPlay.Web.Services
{
    public class FileViewModelsService
    {
        private readonly IMapper _mapper;
        private readonly ApiInfo _apiInfo;

        public FileViewModelsService(IMapper mapper, IOptions<ApiInfo> apiInfo)
        {
            _mapper = mapper;
            _apiInfo = apiInfo.Value;
        }

        public FileViewModel CreateFromDomain(File file)
        {
            var fileVm = _mapper.Map<FileViewModel>(file);
            fileVm.FileLink = _apiInfo.Address + fileVm.FileLink;
            return fileVm;
        }

        public Dictionary<FileViewModel, FileViewModel> CreateCompressedImageToImageMapping(
            ICollection<FileWithCompressedCopy> domainMapping)
        {
            return domainMapping
                .ToDictionary(k => CreateFromDomain(k.CompressedFile), 
                    v => CreateFromDomain(v.File));
        }
    }
}