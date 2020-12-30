using MeetAndPlay.Data.DTO;
using MeetAndPlay.Data.Interfaces;
using MeetAndPlay.Data.Models;

namespace MeetAndPlay.Web.ViewModels
{
    public class NamedViewModel : BaseEntity, INamedEntity
    {
        public string Name { get; set; }

        public static NamedViewModel CreateFrom(NamedEntityDto dto)
        {
            return new NamedViewModel
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }
    }
}