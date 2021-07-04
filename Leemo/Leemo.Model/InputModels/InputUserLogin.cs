using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Leemo.Model.InputModels
{
    public class InputUserLogin
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password length should be minimum 6.")]
        public string Password { get; set; }
        [Obsolete]
        public Guid? CompanyLocationID { get; set; }
    }
}
