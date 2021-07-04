using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Leemo.Model.InputModels;

namespace Leemo.Model.ResultModels
{
    public class ResultProductLead
    {
        public Guid Id { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public Guid ProductPackageId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public string FullName { get; set; }
        [Required]
        public string CompanyName { get; set; }
        public bool IsVerified { get; set; }
        public DateTime? VerificationDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }


        //50
        public string DomainName { get; set; }

        //150
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }
        [Required]//150
        public string Street { get; set; }

        [Required]//50
        public string City { get; set; }

        [Required]//50
        public string State { get; set; }

        [Required]//20
        public string ZipCode { get; set; }

        [Required]
        //20
        public string Country { get; set; }

        public string Fax { get; set; }
    }
}
