using Microsoft.EntityFrameworkCore;
using Leemo.Model;
using Leemo.Model.Domain;

/// <summary>
/// Represents Leemo data project
/// </summary>
namespace Leemo.Data
{
    /// <summary>
    /// Represents Leemo db context
    /// </summary>
    public class LeemoDbContext : DbContext
    {
        public LeemoDbContext(DbContextOptions<LeemoDbContext> options)
            : base(options)
        {
        }

        public LeemoDbContext()
           : base()
        {
        }

        #region DbSet
        //Company
        public virtual DbSet<Company> Company { get; set; }
        //public virtual DbSet<CompanyAddress> CompanyAddress { get; set; }

        //User
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserProfile> UserProfile { get; set; }
        //public virtual DbSet<UserAddress> UserAddress { get; set; }

        //AddressType
        public virtual DbSet<AddressType> AddressType { get; set; }

        //Role
        public virtual DbSet<Designation> Designation { get; set; }
        public virtual DbSet<DesignationHierarchy> DesignationHierarchy { get; set; }

        ////Profile
        //public virtual DbSet<Profile> Profile { get; set; }
        //public virtual DbSet<Permission> Permission { get; set; }
        //public virtual DbSet<ProfileUserMapping> ProfileUserMapping { get; set; }
        //public virtual DbSet<ProfilePermissionMapping> ProfilePermissionMapping { get; set; }

        //Security
        public virtual DbSet<Auth_Role> Auth_Role { get; set; }
        public virtual DbSet<Auth_RoleUserMapping> Auth_RoleUserMapping { get; set; }
        public virtual DbSet<Auth_RoleFeatureMappingTemp> Auth_RoleFeatureMappingTemp { get; set; }
        public virtual DbSet<Feature> Feature { get; set; }
        public virtual DbSet<GeneralCodeGroup> GeneralCodeGroup { get; set; }
        public virtual DbSet<GeneralCode> GeneralCode { get; set; }
        public virtual DbSet<Auth_FeatureCodeMapping> Auth_FeatureCodeMapping { get; set; }
        public virtual DbSet<Auth_RoleFeatureMapping> Auth_RoleFeatureMapping { get; set; }


        //Groups
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<GroupUser> GroupUser { get; set; }
        public virtual DbSet<GroupDesignationMapping> GroupDesignationMapping { get; set; }
        public virtual DbSet<GroupGroupsMapping> GroupGroupsMapping { get; set; }

        //Logging
        public virtual DbSet<ErrorLog> ErrorLog { get; set; }

        //Api Request Log
        public virtual DbSet<ApiRequestLog> ApiRequestLog { get; set; }

        //AdminMenu
        public virtual  DbSet<AdminMenu> AdminMenu { get; set; }

        //Address
        public virtual DbSet<Addresses> Addresses { get; set; }

        //Location
        public virtual DbSet<CompanyLocation> CompanyLocation { get; set; }
        public virtual DbSet<CompanyLocationUserMapping> CompanyLocationUserMapping { get; set; }

        //Product
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<CompanyProductMapping> CompanyProductMapping { get; set; }


        public virtual DbSet<ProductLead> ProductLead { get; set; }

       
        public virtual DbSet<HostingKeyword> HostingKeyword { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GroupGroupsMapping>()
                        .HasOne(x => x.Group)
                        .WithMany(x => x.GroupGroupsMapping)
                        .HasForeignKey(x => x.GroupId);

            modelBuilder.Entity<Auth_FeatureCodeMapping>().HasKey(c => new { c.FeatureId, c.CodeId });
            modelBuilder.Entity<Auth_RoleFeatureMapping>().HasKey(c => new { c.FeatureId, c.CodeId, c.RoleId });
            modelBuilder.Entity<Auth_RoleUserMapping>().HasKey(c => new { c.UserId, c.RoleId });
            modelBuilder.Entity<DesignationHierarchy>().HasKey(c => new { c.DesignationId, c.ParentDesignationId });
            modelBuilder.Entity<GroupDesignationMapping>().HasKey(c => new { c.DesignationId, c.GroupId });
            modelBuilder.Entity<GroupUser>().HasKey(c => new { c.UserId, c.GroupId });
            modelBuilder.Entity<Auth_RoleFeatureMappingTemp>().HasKey(c => new { c.FeatureId, c.CodeId, c.RoleId, c.SessionId });
            modelBuilder.Entity<CompanyLocationUserMapping>().HasKey(c => new { c.CompanyLocationId, c.UserId });
            modelBuilder.Entity<CompanyProductMapping>().HasKey(c => new { c.CompanyId, c.ProductId });

            

        }
    }
}
