using System.Collections.Generic;

namespace MeetAndPlay.Web.ViewModels
{
    public class EditProfileViewModel
    {
        public string About { get; set; }
        public IList<FileViewModel> Avatar { get; set; } = new List<FileViewModel>();
        public IList<FileViewModel> CompressedAvatar { get; set; } = new List<FileViewModel>();
    }
}