using System;
using System.ComponentModel.DataAnnotations;
using IdentityServerAspNetIdentity.Models;

namespace IdentityServerHost.Quickstart.UI.Register
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        
        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
        
        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        
        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        
        [Required]
        [Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }
        
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }
        
        [Required]
        [Display(Name = "Ваш пол")]
        public Gender Gender { get; set; }

    }
}