using MeetAndPlay.Data.Interfaces;
using MeetAndPlay.Data.Models;

namespace MeetAndPlay.Data.DTO
{
    public class NamedEntityDto : BaseEntity, INamedEntity
    {
        public string Name { get; set; }
    }
}