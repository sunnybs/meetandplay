using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MeetAndPlay.Data.DTO.Files;
using MeetAndPlay.Data.Models.Files;
using Radzen;

namespace MeetAndPlay.Web.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _client;

        public ApiClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<File> UploadFileAsync(string base64Source, string filename)
        {
            var request = new Base64UploadRequest
            {
                Base64Source = base64Source,
                Filename = filename
            };

            var response = await _client.PostAsJsonAsync("/Files/UploadBase64", request);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"File upload failed, Status code: {response.StatusCode}");
            }
            var file = await response.ReadAsync<File>();
            return file;
        }
    }
}