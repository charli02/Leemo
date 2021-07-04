using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Leemo.Model.Domain;

namespace Leemo.Model.ResultModels
{
    public class ResultUser
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public bool ForcePasswordReset { get; set; }
        public Nullable<Boolean> isFirstLogin { get; set; }
        public Designation Designation { get; set; }
        public UserProfile UserProfile { get; set; }

        public IList<Auth_Role> Auth_Roles { get; set; }

        //public UserAddress userAddress { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiration { get; set; }
        [NotMapped]
        public int TotalUsers { get; set; }
        [NotMapped]
        public int ActiveUsers { get; set; }
        [NotMapped]
        public bool isUserCurrentBaseLocation { get; set; }
    }
}
