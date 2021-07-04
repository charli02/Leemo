using System;
using System.Collections.Generic;
using System.Text;

namespace Leemo.Model.ResultModels
{
    public class ResultAdminMenu
    {
        public Guid Id { get; set; }
        public string MenuName { get; set; }
        public int SortOrder { get; set; }
        public string MenuAccessLevel { get; set; }
        public Boolean IsActive { get; set; }
    }
}
