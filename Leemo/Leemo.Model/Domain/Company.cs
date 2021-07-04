using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Namespace represt Leemo.Model project
/// </summary>
namespace Leemo.Model.Domain
{
    /// <summary>
    /// Represents address type
    /// </summary>
    public class Company
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int EmployeeCount { get; set; }

        [Required]
        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string CountryCode { get; set; }

        public string Fax { get; set; }

        [Required]
        public string Website { get; set; }

        public string Description { get; set; }

        [Required]
        public string Currency { get; set; }

        public string TimeZone { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Language { get; set; }
        public string ImageName { get; set; }
       
        public string TimeFormat { get; set; }
        public string DateFormat { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        //public virtual CompanyAddress CompanyAddress { get; set; }
        //public virtual Addresses CompanyAddress { get; set; }
    }
}
