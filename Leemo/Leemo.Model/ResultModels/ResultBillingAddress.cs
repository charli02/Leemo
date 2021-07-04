using Leemo.Model.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Leemo.Model.ResultModels
{
    public class ResultBillingAddress
    {
        public Guid Id { get; set; }
        public Guid? ReferenceId { get; set; }
        public Guid? AddressTypeId { get; set; }
        public string Street { get; set; }        
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string CompanyName { get; set; }
        public Boolean IsHeadOffice { get; set; }
        public string LocationName { get; set; }
        public string AddressTypeName { get; set; }
    }
}
