using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Namespace represt Leemo.Model project
/// </summary>
namespace Leemo.Model.Domain
{
    /// <summary>
    /// Represents user profile
    /// </summary>
    public class UserProfile
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public virtual Designation Role { get; set; }
        public Guid DesignationId { get; set; }
        //public virtual Profile Profile { get; set; }
        //public Guid ProfileId { get; set; }

        public Guid? ReportingToUserId { get; set; }
        public string Description { get; set; }

        public string Alias { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string Language { get; set; }
        public string CountryLocale { get; set; }
        public string DateFormat { get; set; }
        public string TimeFormat { get; set; }
        public string TimeZone { get; set; }
        public string CountryCode { get; set; }
        public Guid CompanyId { get; set; }
        /*public virtual Company Company { get; set; }*/
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ImageName { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public string ReportingToUserName { get; set; }
    }
}
