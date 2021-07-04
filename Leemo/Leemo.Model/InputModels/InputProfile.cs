using System;
using System.Collections.Generic;
using System.Text;


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Leemo.Model.InputModels
{
    public class InputProfile
    {
        public Guid Id { get; set; }


        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Only Alphabets are allowed")]
        [Display(Name = "Role Name", Prompt = "Role Name")]
        [MaxLength(50, ErrorMessage = " Role Name can be max 50 characters long.")]
        [Required(ErrorMessage = "Please enter Name.")]
        public string Name { get; set; }


        [Display(Name = "Description", Prompt = "Description")]
        [MaxLength(200, ErrorMessage = "Description can be max 200 characters long.")]
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public Boolean IsDeleted { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public Guid? CompanyLocationId { get; set; }

    }
}
