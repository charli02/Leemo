using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

/// <summary>
/// Namespace represt Leemo.Model project
/// </summary>
namespace Leemo.Model.Domain
{
    /// <summary>
    /// Represents user
    /// </summary>
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        public string UserName { get; set; }

        //[Required]
        //public string Email { get; set; }
        
        public string PasswordSalt { get; set; }

        public string PasswordHash { get; set; }

        public Boolean IsActive { get; set; }

        public Boolean ForcePasswordReset { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string TempPasswordHash { get; set; }
        public string TempPasswordSalt { get; set; }
        public DateTime? TempPasswordExpiryDate { get; set; }
        public Nullable<Boolean> isFirstLogin { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
