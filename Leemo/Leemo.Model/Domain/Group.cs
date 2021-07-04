using System;
using System.Collections.Generic;

/// <summary>
/// Namespace represt Leemo.Model project
/// </summary>
namespace Leemo.Model.Domain
{
    /// <summary>
    /// Represents group
    /// </summary>
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ImageName { get; set; }
        public virtual IList<GroupUser> GroupUser { get; set; }
        public virtual IList<GroupDesignationMapping> GroupDesignationMapping { get; set; }
        public virtual IList<GroupGroupsMapping> GroupGroupsMapping { get; set; }
        public Boolean IsActive { get; set; }
        public Guid CompanyLocationId { get; set; }

    }
}
