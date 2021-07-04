using System;
using System.Collections.Generic;
using System.Text;

namespace Leemo.Model.Domain
{
    public class ApiRequestLog
    {
        public Guid Id { get; set; }
        public string RequestbyUser { get; set; }
        public string IPAddress { get; set; }
        public DateTime RequestDateTime { get; set; }
        public string ApiPath { get; set; }
        public string RequestParameters { get; set; }
        public bool ResponseSuccess { get; set; }
        public string ErrorDescription { get; set; }
    }
}
