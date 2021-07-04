﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicFormsCore
{
    [Table("DataFormFieldType")]
    public partial class DataFormFieldType
    {
        //public DataFormFieldType()
        //{
        //    DataFormField = new HashSet<DataFormField>();
        //}

        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(150)]
        public string FieldTypeName { get; set; }

        //[StringLength(150)]
        //public string FieldTypeXML { get; set; }

        public bool Active{ get; set; }

        public bool AttributeChecked { get; set; }

        public bool AttributeDisabled { get; set; }

        public bool AttributeMax { get; set; }

        public bool AttributeMin { get; set; }

        public bool AttributeMaxlength { get; set; }

        public bool AttributePattern { get; set; }

        public bool AttributeReadonly { get; set; }

        public bool AttributeRequired { get; set; }

        public bool AttributeSize { get; set; }

        public bool AttributeStep { get; set; }

        public bool AttributeValue { get; set; }               

        //[InverseProperty("DataFormFieldType")]
        //public virtual ICollection<DataFormField> DataFormField { get; set; }
    }
}