using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Namespace represt Leemo.Model project
/// </summary>
namespace Leemo.Model.Domain
{
    /// <summary>
    /// Represents role
    /// </summary>
    public class Designation
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Please enter Name.")]
        [MaxLength(50, ErrorMessage = " Designation Name can be max 50 characters long.")]
        public string Name { get; set; }
        [MaxLength(200, ErrorMessage = " Description can be max 200 characters long.")]
        public string Description { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Boolean IsRoot { get; set; }
        public Guid CompanyLocationId { get; set; }
        [NotMapped]
        public Guid ParentDesignationId { get; set; }
    }
}
