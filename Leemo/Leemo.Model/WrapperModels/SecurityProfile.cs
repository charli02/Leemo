using System;
using System.Collections.Generic;
using System.Text;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;

namespace Leemo.Model.WrapperModels
{
    public class SecurityProfile
    {
        public List<ResultRole> ResultProfile { get; set; }

        public InputProfile inputProfile { get; set; }
        public List<ResultUser> ResultUser { get; set; }
        
    }
}
