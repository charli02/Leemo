using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Leemo.Model.Domain
{
    public class Addresses
    {
        public Guid Id { get; set; }
        public string AddressLine1 { get; set; }
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

        public Guid? ReferenceId { get; set; }

        public Guid? AddressTypeId { get; set; }

        public AddressType AddressType { get; set; }
    }
}
