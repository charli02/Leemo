using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Namespace represt Leemo.Model project
/// </summary>
namespace Leemo.Model.Domain
{
    /// <summary>
    /// Represents error log
    /// </summary>
    public class ErrorLog
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string RequestId { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public string RequestPath { get; set; }
        public string User { get; set; }
        public string ActionDescriptor { get; set; }
        public string IpAddress { get; set; }
        public string LogType { get; set; }
        public string ProjectSource { get; set; }
    }
}
