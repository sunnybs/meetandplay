using System.Threading.Tasks;
using MeetAndPlay.Data.DTO;

namespace MeetAndPlay.Core.Abstraction.Services
{
    public interface IReadService
    {
        public Task<CountArray<NamedEntityDto>> GetResultsLikeAsync(ReadFilterDto filter);
        public Task<CountArray<NamedEntityDto>> GetAllAsync();
    }
}