using System;
using System.Linq;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Data.DTO;

namespace MeetAndPlay.Web.Services
{
    public class MockReadService : IReadService
    {
        public readonly NamedEntityDto[] Resources = 
        {
            new () {Id = Guid.NewGuid(), Name = "Тестовые данные 1"},
            new () {Id = Guid.NewGuid(), Name = "Тестовые данные 2"},
            new () {Id = Guid.NewGuid(), Name = "Тестовые данные 3"},
            new () {Id = Guid.NewGuid(), Name = "Тестовые данные 4"},
            new () {Id = Guid.NewGuid(), Name = "Тестовые данные 5"},
            new () {Id = Guid.NewGuid(), Name = "Тестовые данные 6"},
        };
        
        public async Task<CountArray<NamedEntityDto>> GetResultsLikeAsync(ReadFilterDto filter)
        {
            var filtered = Resources.Where(r =>
                r.Name.Contains(filter.Query, StringComparison.InvariantCultureIgnoreCase)).ToArray();

            return new CountArray<NamedEntityDto>(filtered, Resources.Length);
        }

        public async Task<CountArray<NamedEntityDto>> GetAllAsync()
        {
            return new (Resources, Resources.Length);
        }
    }
}