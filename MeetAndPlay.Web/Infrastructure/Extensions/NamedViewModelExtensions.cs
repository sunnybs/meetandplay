using System.Collections.Generic;
using System.Linq;
using MeetAndPlay.Data.DTO;
using MeetAndPlay.Web.ViewModels;

namespace MeetAndPlay.Web.Infrastructure.Extensions
{
    public static class NamedViewModelExtensions
    {
        public static NamedViewModel[] ToViewModels(this IEnumerable<NamedEntityDto> source)
        {
            return source.Select(NamedViewModel.CreateFrom).ToArray();
        } 
    }
}