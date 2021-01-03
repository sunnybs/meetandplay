using System.Threading.Tasks;
using MeetAndPlay.Data.DTO;

namespace MeetAndPlay.Core.Abstraction.Services
{
    public interface INamedEntitiesService
    {
        public Task<CountArray<NamedEntityDto>> GetNamedEntitiesAsync(ReadFilterDto filter);
    }
}