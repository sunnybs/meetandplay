using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Data.Models.Files;

namespace MeetAndPlay.Web.Services
{
    public interface IApiClient 
    {
        Task<File> UploadFileAsync(string base64Source, string filename);
        Task<string> GetRandomLobbyPictureLinkAsync();
        Task<string> GetRandomPlayerPictureLinkAsync();
        Task<string> GetRandomOfferPictureLinkAsync();
    }
}