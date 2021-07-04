using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Namespace represt Leemo.Model project
/// </summary>
namespace Leemo.Model.Domain
{
    /// <summary>
    /// Represents address mapped for the company
    /// </summary>
    public class CompanyAddress
    {
        public Guid Id { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        public string Country { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public Guid CompanyId { get; set; }
        //public Company Company { get; set; }

        public Guid AddressTypeId { get; set; }
        public AddressType AddressType { get; set; }
    }
}
