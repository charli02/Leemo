using System;
using System.Collections.Generic;
using System.Text;
using Leemo.Model.ResultModels;

namespace Leemo.Model.WrapperModels
{
    public class GroupsAndUsers
    {
        public List<ResultUser> ResultUser { get; set; }
        public List<ResultGroup> ResultGroup { get; set; }
    }
}
