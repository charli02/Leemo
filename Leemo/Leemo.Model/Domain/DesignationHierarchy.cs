using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Namespace represt Leemo.Model project
/// </summary>
namespace Leemo.Model.Domain
{
    /// <summary>
    /// Represents role hierarchy
    /// </summary>
    public class DesignationHierarchy
    {
        //public Guid? Id { get; set; }
        public Guid DesignationId { get; set; }
        public Designation Designation { get; set; }

        public Guid ParentDesignationId { get; set; }
        public Designation ParentDesignation { get; set; }

        [NotMapped]
        public List<DesignationHierarchy> Children { get; set; }

        public int SortOrder { get; set; }

        [NotMapped]
        public string DesignationListHTML{ get; set; }

    }
}
