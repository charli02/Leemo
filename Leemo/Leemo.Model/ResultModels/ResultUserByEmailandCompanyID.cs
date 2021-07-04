using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Leemo.Model.Domain;

namespace Leemo.Model.ResultModels
{
   public class ResultUserByEmailandCompanyID
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public Guid CompanyLocationId { get; set; }
        
    }
}
