using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Leemo.Model.InputModels
{
    public class InputChangePassword
    {
        //[Required]
        //[EmailAddress]
        [Display(Name = "Email/Username", Prompt = "Email/Username")]
        [MaxLength(200, ErrorMessage = "Email/Username can be max 200 characters long.")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Old Password", Prompt = "Old Password")]
        public string OldPassword { get; set; }



        [Required]
        [Display(Name = "New Password", Prompt = "New Password")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Password must meet the following requirements :")]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "Confirm Password", Prompt = "Confirm Password")]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

        [NotMapped]
        public string FirstLogin { get; set; }

        [NotMapped]
        public Guid CompanyId { get; set; }

        [NotMapped]
        public Guid CompanyLocationId { get; set; }

    }
}
