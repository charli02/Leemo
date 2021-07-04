using System;
using System.Collections.Generic;
using System.Text;

namespace Leemo.Model.ResultModels
{
    public class ResultUpdateUser
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Guid UserProfileId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? ProfileId { get; set; }
        public string Alias { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Website { get; set; }
        public string Fax { get; set; }
        //public Guid UserAddressId { get; set; }
        public Guid AddressId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public string CountryLocale { get; set; }
        public string DateFormat { get; set; }
        public string TimeFormat { get; set; }
        public string TimeZone { get; set; }
    }
}
