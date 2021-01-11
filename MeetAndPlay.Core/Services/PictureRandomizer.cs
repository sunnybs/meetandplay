using System;
using System.IO;
using MeetAndPlay.Core.Abstraction.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace MeetAndPlay.Core.Services
{
    public class PictureRandomizer : IPictureRandomizer
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _http;

        public PictureRandomizer(IWebHostEnvironment env, IHttpContextAccessor http)
        {
            _env = env;
            _http = http;
        }
        
        public string GetRandomLobbyPictureLink()
        {
            return GetRandomPicture("lobbies");
        }
        
        public string GetRandomPlayerPictureLink()
        {
            return GetRandomPicture("players");
        }

        public string GetRandomOfferPictureLink()
        {
            return GetRandomPicture("offers");
        }

        private string GetRandomPicture(string directory)
        {
            var path = _env.WebRootPath + "/images/" + directory;
            var files = Directory.GetFiles(path);
            var rand = new Random();
            var index = rand.Next(0, files.Length - 1);

            var currentHost = _http.HttpContext.Request.Scheme + @":\\" +  _http.HttpContext.Request.Host.Value;
            return files[index]
                .Replace(_env.WebRootPath, currentHost)
                .Replace("/", "\\");
        }
    }
}