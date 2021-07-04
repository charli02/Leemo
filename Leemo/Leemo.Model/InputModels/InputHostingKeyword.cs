using System;
using System.Collections.Generic;
using System.Text;

namespace Leemo.Model.InputModels
{
   public class InputHostingKeyword
    {
        public Guid Id { get; set; }

        public string Keyword { get; set; }

        public bool IsValid { get; set; }
    }
}
