using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Leemo.Model.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using TPSS.Common;

namespace Leemo.Model.InputModels
{
    public class InputUpdateUser
    {
        public Guid Id { get; set; }
        [EmailAddress]
        [Required(ErrorMessage ="Please enter UserName.")]
        [MaxLength(200, ErrorMessage = "Email/Username can be max 200 characters long.")]
        [Display(Name = "Email/Username", Prompt = "example@email.com")]
        public string UserName { get; set; }
        public Boolean IsActive { get; set; }
        public Nullable<Boolean> isFirstLogin { get; set; }
        [Required(ErrorMessage = "Please choose Profile.")]
        public IList<Guid> profiles { get; set; }
        public InputUpdateUserProfile userProfile { get; set; }
        //public UserAddress userAddress { get; set; }
        
        [NotMapped]
        public string returnFrom { get; set; }
    }

    public class InputUpdateUserProfile
    {
        [Required(ErrorMessage = "Please enter First Name.")]
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Only Alphabets are allowed")]
        [Display(Name = "First Name", Prompt = "First Name")]
        [MaxLength(50, ErrorMessage = " First Name can be max 17 characters long.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Last Name.")]
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Only Alphabets are allowed")]
        [Display(Name = "Last Name", Prompt = "Last Name")]
        [MaxLength(50, ErrorMessage = " Last Name can be max 17 characters long.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please choose Role.")]
        public Guid DesignationId { get; set; }

        [NotMapped]
        public string DesignaionName { get; set; }

        //[ValidateNullAttribute]
        [Required(AllowEmptyStrings =  true)]
        public Nullable<Guid> ReportingToUserId { get; set; }
        [Display(Name = "Description", Prompt = "Description Here")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please enter Date of Birth.")]

        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Alias", Prompt = "Alias")]
        [MaxLength(20, ErrorMessage = "Alias can be max 50 characters long")]
        public string Alias { get; set; }

        [Phone]
        [MaxLength(20, ErrorMessage = "Phone Number can be max 50 characters long")]
        [MinLength(5, ErrorMessage = "Phone Number can be min 5 characters long")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid Mobile Number")]
        [Display(Name = "Phone", Prompt = "Phone")]
        public string Phone { get; set; }

        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "No white space allowed")]
        [MaxLength(50, ErrorMessage = " CountryCode can be max 50 characters long.")]
        public string CountryCode { get; set; }

        [NotMapped]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid CountryCode Number")]
        public string CountryCodeNumber { get; set; }

        [NotMapped]
        [Phone]
        [MaxLength(15, ErrorMessage = " Mobile Number can be max 15 characters long")]
        [MinLength(5, ErrorMessage = "Mobile Number can be min 5 characters long")]
        [Required(ErrorMessage = "Please enter Mobile Number.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid Mobile Number")]
        [Display(Name = "Mobile Number", Prompt = "Mobile Number")]
        public string MobileNumber { get; set; }


        [Phone]
        [MaxLength(20, ErrorMessage = "Mobile Number can be max 20 characters long")]
        //[Required(ErrorMessage = "Please enter Mobile Number.")]
        [RegularExpression(@"^[0-9]+(-[0-9]+)+$", ErrorMessage = "Invalid Mobile Number")]
        public string Mobile { get; set; }

        [RegularExpression(@".*\S+.*", ErrorMessage = "No white space allowed")]
        [MaxLength(200, ErrorMessage = " Website can be max 200 characters long.")]
        [Display(Name = "Website", Prompt = "Website")]
        public string Website { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid Fax Number")]
        [Display(Name = "Fax", Prompt = "Fax")]
        [MaxLength(20, ErrorMessage = " Fax can be max 20 characters long.")]
        public string Fax { get; set; }
        //[Required]
        [MaxLength(100, ErrorMessage = "Language can be max 100 characters long.")]
        [Display(Name = "Language", Prompt = "Language")]
        public string Language { get; set; }
        //[Required]
        [Display(Name = "Country Locale", Prompt = "Country Locale")]
        [MaxLength(20, ErrorMessage = "CountryLocale can be max 20 characters long.")]
        public string CountryLocale { get; set; }
        //[Required]
        [MaxLength(20, ErrorMessage = "DateFormat can be max 20 characters long.")]
        [Display(Name = "Date Format", Prompt = "DD/MM/YYYY")]
        public string DateFormat { get; set; }
        //[Required]
        [MaxLength(20, ErrorMessage = "TimeFormat can be max 20 characters long.")]
        [Display(Name = "Time Format", Prompt = "HH:MM XM")]
        public string TimeFormat { get; set; }
        //[Required]
        [MaxLength(20, ErrorMessage = "TimeZone can be max 20 characters long.")]
        [Display(Name = "Time Zone", Prompt = "Time Zone")]
        public string TimeZone { get; set; }

        public Guid CompanyId { get; set; }
        [NotMapped]
        //[Required(ErrorMessage = "Please select Day")]
        public string DOBDay { get; set; }
        [NotMapped]
        //[Required(ErrorMessage = "Please select Month")]
        public string DOBMonth { get; set; }
        [NotMapped]
        //[Required(ErrorMessage = "Please select Year")]
        public string DOBYear{ get; set; }

    }
}
