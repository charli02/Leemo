using System;
using System.Collections.Generic;
using System.Text;

namespace Leemo.Model.InputModels
{
    public class InputRoleHierarchy
    {
        public Guid RoleId { get; set; }
        public Guid ParentRoleId { get; set; }
    }
}
