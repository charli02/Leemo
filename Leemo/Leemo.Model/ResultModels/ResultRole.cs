using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Leemo.Model.Domain;

namespace Leemo.Model.ResultModels
{
   public class ResultRole
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public Boolean IsDeleted { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }

        public string CreatedByUser { get; set; }
        public string ModifiedByUser { get; set; }
        public string FirstNameCreatedBy { get; set; }
        public string LastNameCreatedBy { get; set; }
        public string FirstNameModifiedBy { get; set; }
        public string LastNameModifiedBy { get; set; }

    }
}
