using System.Collections.Generic;
/// <summary>
/// Represents Leemo common project
/// </summary>
namespace TPSS.Common.Wrappers
{
    /// <summary>
    /// This class used to cast the response of request
    /// </summary>
    /// <typeparam name="T">Represents model class</typeparam>
    public class ApiResponse<T>
    {
        public ApiResponse()
        {
        }

        public ApiResponse(object data)
        {
            Succeeded = true;
            Message = string.Empty;
            Data = data;
        }

        public bool Succeeded { get; set; }
        public Enums.HttpStatusCode ResponseCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public short ResponseType { get; set; }
    }
}
