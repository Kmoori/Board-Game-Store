using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Security;

namespace Mvc3ToolsUpdateWeb_Default.Models
{

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Jelenlegi jelszó")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A {0} legalább {2} karakter hosszúnak kell lennie.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Új jelszó")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Jelszó megerősítése")]
        [Compare("NewPassword", ErrorMessage = "Az új és a megerősített jelszó nem egyezik meg!")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        //+ ID
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Felhasználónév")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Jelszó")]
        public string Password { get; set; }

        [Display(Name = "Bejelentkezve maradok")]
        public bool RememberMe { get; set; }

        public LogOnModel()
        {

        }

        public LogOnModel(string U, string pw)
        {
            UserName = U;
            Password = pw;
        }
    }



    public class RegisterModel
    {
        [Required]
        [Display(Name = "Felhasználónév")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail cím")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A {0} legalább {2} karakter hosszúnak kell lennie.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Jelszó")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Jelszó megerősítése")]
        [Compare("Password", ErrorMessage = "Az új és a megerősített jelszó nem egyezik meg!")]
        public string ConfirmPassword { get; set; }
    }
}
