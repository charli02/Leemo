using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Leemo.Model.Domain;

namespace Leemo.Model.ResultModels
{
    public class ResultCompany
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int EmployeeCount { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public string TimeZone { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Language { get; set; }
        public string ImageName { get; set; }
        public string CountryCode { get; set; }
        
        public string TimeFormat { get; set; }
        public string DateFormat { get; set; }


        //public CompanyAddress CompanyAddress { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
