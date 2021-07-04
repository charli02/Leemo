/// <summary>
/// Represents Leemo common project layer
/// </summary>
namespace TPSS.Common
{
    /// <summary>
    /// Contains all the enums used in the application
    /// </summary>
    public class Enums
    {
        public enum HttpStatusCode
        {
            OK = 200,
            AccessDenied = 401,
            BadRequest = 400,
            NotFound = 404,
            InternalServerError = 500
        }
    }
}
