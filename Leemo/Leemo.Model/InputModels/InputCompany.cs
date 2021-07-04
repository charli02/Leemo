using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Leemo.Model.InputModels
{
    public class InputCompany
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Company Name Required")]
        [MaxLength(50,ErrorMessage = "Name can be max 50 characters long.")]
        //[RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Only Alphabets are allowed")]
        [Display(Name = "Organization Name", Prompt = "Organization Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Employee Count", Prompt = "Employee Count")]
        public int EmployeeCount { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Phone can be max 15 characters long")]
        [MinLength(5, ErrorMessage = "Phone can be min 5 characters long")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid Mobile Number")]
        [Display(Name = "Phone", Prompt = "Phone")]
        public string Phone { get; set; }


        [MaxLength(20, ErrorMessage = " CountryCode  can be max 15 characters long")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "No white space allowed")]
        public string CountryCode { get; set; }

        [NotMapped]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid CountryCode Number")]
        public string CountryCodeNumber { get; set; }

        [NotMapped]
        [Phone]
        [MaxLength(15, ErrorMessage = " Mobile can be max 15 characters long")]
        [MinLength(5, ErrorMessage = "Mobile Number can be min 5 characters long")]
        [Required(ErrorMessage = "Please enter Mobile Number.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid Mobile Number")]
        [Display(Name = "Mobile Number", Prompt = "Mobile Number")]
        public string MobileNumber { get; set; }




        [RegularExpression(@"^[0-9]+(-[0-9]+)+$", ErrorMessage = "Invalid Mobile Number")]
        [MaxLength(20, ErrorMessage = "Mobile Number can be max 20 characters long")]
        public string Mobile { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid Fax Number")]
        [MaxLength(20, ErrorMessage = "Fax can be max 20 characters long.")]
        [Display(Name = "Fax", Prompt = "Fax")]
        public string Fax { get; set; }

        [Required]
        [MaxLength(200, ErrorMessage = "Website can be max 200 characters long.")]
        [RegularExpression(@".*\S+.*", ErrorMessage = "No white space allowed")]
        [Display(Name = "Website", Prompt = "Website")]
        public string Website { get; set; }

        [MaxLength(500, ErrorMessage = "Description can be max 500 characters long.")]
        [Display(Name = "Description", Prompt = "Description Here")]
        public string Description { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "Currency can be max 10 characters long.")]
        [RegularExpression(@".*\S+.*", ErrorMessage = "No white space allowed")]
        [Display(Name = "Currency", Prompt = "Currency")]
        public string Currency { get; set; }
        [MaxLength(10, ErrorMessage = "TimeZone can be max 10 characters long.")]
        [Display(Name = "Time Zone", Prompt = "Time Zone")]
        public string TimeZone { get; set; }
        [MaxLength(200, ErrorMessage = "Language can be max 200 characters long.")]
        [Display(Name = "Language", Prompt = "Language")]
        public string Language { get; set; }

        
        [Display(Name = "Time Format", Prompt = "Time Format")]
        [MaxLength(20, ErrorMessage = "CountryLocale can be max 20 characters long.")]
        public string TimeFormat { get; set; }
        [Display(Name = "Date Format", Prompt = "Date Format")]
        [MaxLength(20, ErrorMessage = "Date Format can be max 20 characters long.")]
        public string DateFormat { get; set; }


        //public InputCompanyAddress CompanyAddress { get; set; }

    }
}
