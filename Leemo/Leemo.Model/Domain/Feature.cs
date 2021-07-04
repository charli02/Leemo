using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Leemo.Model.Domain
{
    public class Feature
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string FeatureName { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public Guid? AdminMenuId { get; set; }
    }
}
