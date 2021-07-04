using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Leemo.Model.Domain
{
    public class ProductLead
    {

        public Guid Id { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public Guid ProductPackageId { get; set; }
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }

        public string CountryCode { get; set; }

        [Required]
        public string CompanyName { get; set; }
        public bool IsVerified { get; set; }
        public DateTime? VerificationDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }


        
        public string DomainName { get; set; }

        
        public string AddressLine1 { get; set; }
        
        public string Street { get; set; }

       
        public string City { get; set; }

        
        public string State { get; set; }

      
        public string ZipCode { get; set; }

 
        
        public string Country { get; set; }


        public string AddressLine2 { get; set; }
        public string Fax { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }



        


    }
}
