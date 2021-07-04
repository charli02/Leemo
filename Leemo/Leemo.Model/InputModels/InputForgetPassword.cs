using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Leemo.Model.InputModels
{
    public class InputForgetPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [NotMapped]
        public Guid CompanyId { get; set; }
        [NotMapped]
        public Guid CompanyLocationId { get; set; }
    }
}
