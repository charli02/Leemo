using System;
using System.Collections.Generic;
using System.Text;

namespace Leemo.Model.InputModels
{
    public class InputUpdateGroupImage
    {
        public Guid GroupId { get; set; }
        public string ImageName { get; set; }
    }
}
