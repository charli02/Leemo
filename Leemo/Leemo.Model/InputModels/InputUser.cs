using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TPSS.Common;

namespace Leemo.Model.InputModels
{
    public class InputUser
    {
        [Required(ErrorMessage = "Please enter UserName.")]
        [EmailAddress]
        [Display(Name = "Email/Username", Prompt = "Example@email.com")]
        [MaxLength(200, ErrorMessage = "Email/Username can be max 200 characters long.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please choose Profile.")]
        public IList<Guid> roles { get; set; }

        public InputUserProfile userProfile { get; set; }
        [NotMapped]
        public Guid CompanyLocationId { get; set; }
        public bool IsExistingUser { get; set; }
        public InputExistingUserData ExistingUserData { get; set; }
        public List<CurrentLocationRoles> currentLocationRoles { get; set; }
        [NotMapped]
        public string CompanyLocationName { get; set; }
        [NotMapped]
        public bool IsActive { get; set; }
    }

    public class InputUserProfile
    {
        [Required(ErrorMessage = "Please enter First Name.")]
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Only Alphabets are allowed")]
        [Display(Name = "First Name", Prompt = "First Name")]
        [MaxLength(50, ErrorMessage = " First Name can be max 50 characters long.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Last Name.")]
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Only Alphabets are allowed")]
        [Display(Name = "Last Name", Prompt = "Last Name")]
        [MaxLength(50, ErrorMessage = " Last Name can be max 50 characters long.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please choose Role.")]
        public Guid DesignationId { get; set; }

        public Guid CompanyId { get; set; }

        //[ValidateNullAttribute]
        //[Required(AllowEmptyStrings = true)]
        public Guid? ReportingToUserId { get; set; }

        [Display(Name = "Description", Prompt = "Description Here")]
        [MaxLength(500, ErrorMessage = " Description can be max 500 characters long.")]
        public string Description { get; set; }
    }

    public class InputExistingUserData
    { 
        public Guid? Id { get; set; }
        public string DesignationName { get; set; }
        public string ReportingToUserEmail { get; set; }
    }
    public class CurrentLocationRoles
    { 
       public Guid ID { get; set; }
       public string Name { get; set; }
    }
}