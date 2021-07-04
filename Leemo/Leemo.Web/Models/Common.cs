using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Leemo.Web.Models.Common
{
    public class ImageUpload
    {
        public Guid Id { get; set; }
        public string Alt { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public string EditImageModalTarget { get; set; }
        public string DisplayImageModalTarget { get; set; }
    }
}
