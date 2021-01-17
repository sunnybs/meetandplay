using System;
using System.ComponentModel.DataAnnotations;
using MeetAndPlay.Data.Enums;

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
        
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        
        [Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }
        
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }
        
        [Required]
        [Display(Name = "Ваш пол")]
        public Gender Gender { get; set; }

    }
}