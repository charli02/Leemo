using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reverse_Proxy_Yarp_Code_Database
{
    //public class SimpleContext : DbContext
    //{
    //    public SimpleContext(DbContextOptions<SimpleContext> options)
    //        : base(options)
    //    {
    //    }

    //    public DbSet<User> Users { get; set; }
    //}
    public class DbHostingContext : DbContext
    {
        public DbHostingContext(DbContextOptions<DbHostingContext> options) : base(options)
        {

        }

        public DbHostingContext() : base()
        {

        }

        public DbSet<HostingInfo> HostingInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

    public class HostingInfo
    {
        [Key]
        public Guid Id { get; set; }

        public string Host { get; set; }

        public string DockerContainer { get; set; }

        public bool IsActive { get; set; }
    }
}
