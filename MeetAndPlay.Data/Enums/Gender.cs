using System.ComponentModel.DataAnnotations;

namespace MeetAndPlay.Data.Enums
{
    public enum Gender
    {
        [Display(Name = "Мужчина")]
        Male,
        [Display(Name = "Женщина")]
        Female
    }
}