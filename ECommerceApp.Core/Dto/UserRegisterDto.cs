using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Core.Dto
{
    public class UserRegisterDto
    {
        [Required,EmailAddress]
        public string Email { get; set; } = String.Empty;

        [Required, StringLength(100,MinimumLength =6)]
        public string Password { get; set; } = String.Empty;

        [Compare("Password",ErrorMessage ="Şifre Eşleşemedi.")]
        public string PasswordConfirm { get; set; } = String.Empty;
    }
}
