using System;
using System.Collections.Generic;
using System.Text;

namespace Leemo.Model.ResultModels
{
    public class ResultActiveUser
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public Boolean IsActive { get; set; }
    }
}
