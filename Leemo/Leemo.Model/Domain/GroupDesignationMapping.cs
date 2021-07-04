using System;

/// <summary>
/// Namespace represt Leemo.Model project
/// </summary>
namespace Leemo.Model.Domain
{
    /// <summary>
    /// Represents roles for group
    /// </summary>
    public class GroupDesignationMapping
    {
        //public Guid Id { get; set; }
        public Guid DesignationId { get; set; }
        public Designation Designation { get; set; }
        public Guid GroupId { get; set; }
        public Group Group { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
