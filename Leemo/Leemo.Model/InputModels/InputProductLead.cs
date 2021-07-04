using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Leemo.Model.InputModels
{
    public class InputProductLead
    {
        public Guid Id { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public Guid ProductPackageId { get; set; }

        [Required(ErrorMessage = "Please enter FullName.")]
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Only Alphabets are allowed")]
        [Display(Name = "FullName", Prompt = "FullName")]
        [MaxLength(50, ErrorMessage = " FullName can be max 50 characters long.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "*Email Address Required")]
        [EmailAddress]
        [Display(Name = "Email/Username", Prompt = "Email Address")]
        [MaxLength(150, ErrorMessage = "Email/Username can be max 150 characters long.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "No white space allowed")]
        [MaxLength(10, ErrorMessage = " CountryCode can be max 10 characters long.")]
        public string CountryCode { get; set; }

        [NotMapped]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid CountryCode Number")]
        public string CountryCodeNumber { get; set; }

        [NotMapped]
        [Phone]
        [MaxLength(15, ErrorMessage = " Phone Number can be max 15 characters long")]
        [MinLength(5, ErrorMessage = "Phone Number can be min 5 characters long")]
        [Required(ErrorMessage = "Please enter Phone Number.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid Phone Number")]
        [Display(Name = "Phone Number", Prompt = "Phone Number")]
        public string PhoneNumber { get; set; }

        
        //[Required(ErrorMessage = "*Phone Number Required")]
        [Display(Name = "Phone Number", Prompt = "Phone Number")]
        [MaxLength(50, ErrorMessage = "Phone Number can be max 50 characters long")]
        [RegularExpression(@"^[0-9]+(-[0-9]+)+$", ErrorMessage = "Invalid Phone Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "*Company Name Required")]
        [MaxLength(250, ErrorMessage = "Name can be max 250 characters long.")]
        [Display(Name = "Organization Name", Prompt = "Organization Name")]
        public string CompanyName { get; set; }
        public bool IsVerified { get; set; }
        public DateTime? VerificationDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public string IpAddress { get; set; }
        public string MacAddress { get; set; }

        public string DomainName { get; set; }
        public string IdVerify { get; set; }

    }


    public class UpdateInputProductLead
    {
        public Guid Id { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "*Domain Name Required")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9]*$", ErrorMessage = "*Start with alphabets, Enter numbers and alphabets only.")]
        [Display(Name = "DomainName", Prompt = "Domain Name")]
        [MaxLength(16, ErrorMessage = " Domain Name can be max 16 characters long.")]
        [MinLength(3, ErrorMessage = " Your Domain Name Should Contain Atleast 3 Characters.")]
        //50
        public string DomainName { get; set; }
        [Display(Name = "AddressLine1", Prompt = "AddressLine1")]
        [MaxLength(150, ErrorMessage = "AddressLine1 can be max 150 characters long.")]
        public string AddressLine1 { get; set; }




        [Required]//50

        [MaxLength(20, ErrorMessage = "City can be max 20 characters long.")]
        [RegularExpression(@".*\S+.*", ErrorMessage = "No white space allowed")]
        [Display(Name = "City", Prompt = "City")]
        public string City { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "State can be max 20 characters long.")]
        [RegularExpression(@".*\S+.*", ErrorMessage = "No white space allowed")]
        [Display(Name = "State", Prompt = "State")]
        public string State { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = " ZipCode can be max 5 characters Small.")]
        [MaxLength(10, ErrorMessage = " Zip Code can be max 6 characters long.")]
      //  [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Invalid Zip Code")]
        [RegularExpression(@"^(?=.*\d.*)[A-Za-z0-9]{3,10}$", ErrorMessage = "Invalid Zip Code")]



        [Display(Name = "Zip Code", Prompt = "Zip Code")]
        public string ZipCode { get; set; }

        [Required]
        [RegularExpression(@".*\S+.*", ErrorMessage = "No white space allowed")]
        [MaxLength(20, ErrorMessage = "Country Code can be max 20 characters long.")]
        [Display(Name = "Country", Prompt = "Country")]
        public string Country { get; set; }
        [Display(Name = "AddressLine2", Prompt = "AddressLine2")]
        [MaxLength(150, ErrorMessage = "AddressLine2 can be max 150 characters long.")]
        public string AddressLine2 { get; set; }


        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid Fax Number")]
        [MaxLength(20, ErrorMessage = "Fax can be max 20 characters long.")]
        [Display(Name = "Fax", Prompt = "Fax")]
        public string Fax { get; set; }

        [Phone]
       [Required(ErrorMessage = "*Phone Number Required")]
        [Display(Name = "Phone Number", Prompt = "Phone Number")]
        [MaxLength(50, ErrorMessage = "Phone Number can be max 50 characters long")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid Phone Number")]
        public string Phone { get; set; }

       
        [Display(Name = "New Password", Prompt = "New Password")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Password must meet the following requirements :")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm Password", Prompt = "Confirm Password")]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }


    }



    public class UpdateInputProductLeadPassword
    {
     

    }
}
