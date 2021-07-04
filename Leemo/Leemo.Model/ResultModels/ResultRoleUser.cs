using System;
using System.Collections.Generic;
using System.Text;

namespace Leemo.Model.ResultModels
{
    public class ResultRoleUser
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public Boolean IsActive { get; set; }
    }
}
