using System;
using TPSS.Common;
using Leemo.Model;
using Leemo.Model.Domain;
using Leemo.Repository.Interface;
using Leemo.Service.Interface;

/// <summary>
/// Represents Leemo service project namespace
/// </summary>
namespace Leemo.Service
{
    /// <summary>
    /// Represnets profile serivce class which interact with repository.
    /// </summary>
    public class CommonService : ICommonService
    {
        private readonly ILogReposiory _logReposiory;
        private readonly IApiRequestLogRepository _apiLogReposiory;

        public CommonService(ILogReposiory logReposiory, IApiRequestLogRepository apiLogReposiory)
        {
            _logReposiory = logReposiory;
            _apiLogReposiory = apiLogReposiory;
        }

        public void LogGlobalExceptionLog(ErrorLog log)
        {
            _logReposiory.InsertLog(log);
        }

        public void LogInDb(string type, string message, string requestPath)
        {
            _logReposiory.InsertLog(new ErrorLog()
            {
                TimeStamp = DateTime.UtcNow,
                LogType = type,
                Message = message,
                RequestPath = requestPath
            });
        }
        public void ApiRequestLogInDb(string path, bool response, string email,string parameteres, string errorMsg)
        {
            _apiLogReposiory.InsertApiRequestLog(new ApiRequestLog()
            {
                RequestbyUser = email,
                IPAddress = CommonFunction.GetIpAddress(),
                RequestDateTime = DateTime.UtcNow,
                ApiPath = path,
                RequestParameters = parameteres,
                ResponseSuccess = response,
                ErrorDescription = errorMsg
            });
        }
    }
}
