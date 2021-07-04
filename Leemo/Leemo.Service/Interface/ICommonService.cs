using Leemo.Model;
using Leemo.Model.Domain;

/// <summary>
/// Represents service project namespace
/// </summary>
namespace Leemo.Service.Interface
{
    public interface ICommonService
    {
        void LogGlobalExceptionLog(ErrorLog log);
        void LogInDb(string type, string message, string requestPath);
        void ApiRequestLogInDb(string path,bool response,string email,string parameters,string errorMsg);
    }
}
