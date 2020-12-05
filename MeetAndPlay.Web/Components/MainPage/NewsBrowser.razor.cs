using System;
using System.Collections.Generic;
using MeetAndPlay.Data.DTO;
using Microsoft.AspNetCore.Components;

namespace MeetAndPlay.Web.Components
{
    public class NewsBrowserComponent : ComponentBase
    {
        public IEnumerable<ShortNewsDto[]> GetLastNewsGroups()
        {
            return new List<ShortNewsDto[]>()
            {
                new[]
                {
                    new ShortNewsDto
                    {
                        Title = "Турнир по манчкину",
                        ShortDescription = "",
                        Date = DateTime.Now,
                        Poster =
                            "https://kudago.com/media/thumbs/xl/images/list/a1/c6/a1c6fa06d22cae6240cb1e246eeeef22.jpg"
                    },
                    new ShortNewsDto
                    {
                        Title = "Турнир по строению пирамидок",
                        ShortDescription = "",
                        Date = DateTime.Now,
                        Poster =
                            "https://kudago.com/media/thumbs/xl/images/list/99/e5/99e5bf9efd0188cd0afff1d183a68211.jpg"
                    },
                }
            };
        }
    }
}