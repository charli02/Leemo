using System;

/// <summary>
/// Namespace represt Leemo.Model project
/// </summary>
namespace Leemo.Model.Domain
{
    /// <summary>
    /// Represents profile
    /// </summary>
    public class Auth_Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public Boolean IsDeleted { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public Guid? CompanyLocationId { get; set; }
    }
}
