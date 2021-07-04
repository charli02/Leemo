using System;
using System.Collections.Generic;
using System.Text;

namespace Leemo.Model.Domain
{
    public class Product
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public Boolean IsLocationBased { get; set; }
        public int SortOrder{ get; set; }
    }
}
