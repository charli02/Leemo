using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DockerAspTestDbConnection.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public Boolean IsLocationBased { get; set; }
        public int SortOrder { get; set; }
    }
}
