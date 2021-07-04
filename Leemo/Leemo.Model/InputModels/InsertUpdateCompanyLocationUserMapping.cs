using System;
using System.Collections.Generic;
using System.Text;

namespace Leemo.Model.InputModels
{
    public class InsertUpdateCompanyLocationUserMapping
    {
        public Guid CompanyLocationId { get; set; }
        public Guid UserId { get; set; }
        public Boolean isBaseLocation { get; set; }
        public Guid OldCompanyLocationId { get; set; }
        public Guid OldUserId { get; set; }
    }
}
