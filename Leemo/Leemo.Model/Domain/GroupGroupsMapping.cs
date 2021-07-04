using System;

/// <summary>
/// Namespace represt Leemo.Model project
/// </summary>
namespace Leemo.Model.Domain
{
    /// <summary>
    /// Represents groups related to specific group
    /// </summary>
    public class GroupGroupsMapping
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public Group Group { get; set; }
        public Guid MappedGroupId { get; set; }
        public Group MappedGroup { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
