using AutoMapper;
using MeetAndPlay.Data.Models.Files;
using MeetAndPlay.Data.Models.Offers;
using MeetAndPlay.Web.ViewModels;

namespace MeetAndPlay.Web.Mapper
{
    public class LobbyProfile : Profile
    {
        public LobbyProfile()
        {
            CreateMap<Lobby, AddLobbyViewModel>().ReverseMap();
            CreateMap<File, FileViewModel>().ReverseMap();
        }
    }
}