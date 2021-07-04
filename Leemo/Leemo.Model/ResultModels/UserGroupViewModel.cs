using System;
using System.Collections.Generic;
using System.Text;
using Leemo.Model.Domain;

namespace Leemo.Model.ResultModels
{
   public class UserGroupViewModel
    {
       
            public IEnumerable<UserProfile> UserProfile { get; set; }
            public IEnumerable<Group> groups { get; set; }
        
    }
}
