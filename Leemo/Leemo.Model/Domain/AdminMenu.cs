using System;

/// <summary>
/// Namespace represt Leemo.Model project
/// </summary>
namespace Leemo.Model.Domain
{
    /// <summary>
    /// Represents profile
    /// </summary>
    public class AdminMenu
    {
        public Guid Id { get; set; }
        public string MenuName { get; set; }
        public int SortOrder { get; set; }
        public string MenuAccessLevel { get; set; }
        public Boolean IsActive { get; set; }
    }
}
