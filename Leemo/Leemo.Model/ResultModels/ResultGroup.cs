using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Leemo.Model.ResultModels
{
    public class ResultGroup
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Please enter Name.")]
        [Display(Name = "Group Name", Prompt = "Group Name")]
        [MaxLength(20, ErrorMessage = "Group Name can be max 20 characters long.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter Description.")]
        [Display(Name = "Description", Prompt = "Description Here")]
        [MaxLength(200, ErrorMessage = " Description can be max 200 characters long.")]
        public string Description { get; set; }
        public string ImageName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid CompanyLocationId { get; set; }
        public List<ResultGroupUser> GroupUsers { get; set; }
        public List<ResultGroupRole> GroupRoles { get; set; }
        public List<ResultGroupGroupsMapping> GroupsMapping { get; set; }

        public Boolean IsActive { get; set; }
        [NotMapped]
        public List<Guid> UserIds { get; set; }
        [NotMapped]
        public List<Guid> RoleIds { get; set; }
        [NotMapped]
        public List<Guid> GroupMappingIds { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }

    public class ResultGroupUser
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class ResultGroupRole
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class ResultGroupGroupsMapping
    {
        public Guid MappedGroupId { get; set; }
        public string MappedGroupName { get; set; }

        public Boolean MappedGroupIsActive { get; set; }
    }
}
