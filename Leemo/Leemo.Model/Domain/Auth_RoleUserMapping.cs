using System;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Namespace represt Leemo.Model project
/// </summary>
namespace Leemo.Model.Domain
{
    /// <summary>
    /// Represents profile user mapping
    /// </summary>
    public class Auth_RoleUserMapping
    {
        //public Guid Id { get; set; }
        [Column("RoleId")]
        public Guid RoleId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        [NotMapped]
        public Auth_Role Auth_Role { get; set; }
    }
}
