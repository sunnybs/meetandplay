using System.ComponentModel.DataAnnotations;

namespace IdentityServerAspNetIdentity.Models
{
    public enum Gender
    {
        [Display(Name = "Мужчина")]Male,
        [Display(Name = "Женщина")]Female
    }
}