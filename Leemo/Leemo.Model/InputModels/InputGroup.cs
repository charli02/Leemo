using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Leemo.Model.InputModels
{
    public class InputGroup
    {
        public Guid? Id { get; set; }
        // [Required]

        //[RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Only Alphabets are allowed")]
        [Display(Name = "Group Name", Prompt = "Group Name")]
        [MaxLength(50, ErrorMessage = "Group Name can be max 50 characters long.")]
        [Required(ErrorMessage = "Please enter Name.")]
        public string Name { get; set; }

        [Display(Name = "Description", Prompt = "Description")]
        [MaxLength(200, ErrorMessage = " Description can be max 200 characters long.")]
        [Required(ErrorMessage = "Please enter Description.")]
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid CompanyLocationId { get; set; }
        public List<InputGroupUser> GroupUsers { get; set; }
        public List<InputGroupRole> GroupRoles { get; set; }
        public List<InputGroupGroupsMapping> GroupsMapping { get; set; }
        public string ImageName { get; set; }
        public Boolean IsActive { get; set; }
        [NotMapped]
        public List<Guid> UserIds { get; set; }
        [NotMapped]
        public List<Guid> RoleIds { get; set; }
        [NotMapped]
        public List<Guid> GroupMappingIds { get; set; }
        

    }

    public class InputGroupUser
    {
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }

        public string UserName { get; set; }
    }

    public class InputGroupRole
    {
        public Guid GroupId { get; set; }
        public Guid RoleId { get; set; }

        public string RoleName { get; set; }
    }

    public class InputGroupGroupsMapping
    {
        public Guid GroupId { get; set; }
        public Guid MappedGroupId { get; set; }

        public string MappedGroupName { get; set; }

        public Boolean MappedGroupIsActive { get; set; }
    }
}
