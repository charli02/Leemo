using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Leemo.Model.Domain
{
    public class GeneralCode
    {
        [Key]
        public Guid Id { get; set; }

        public Guid GeneralCodeGroup { get; set; }

        [Required]
        [MaxLength(150)]
        public string CodeName { get; set; }

        [Required]
        [MaxLength(150)]
        public string CodeValue { get; set; }

        public bool IsActive { get; set; }
    }
}
