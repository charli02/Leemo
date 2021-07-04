using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TPSS.Common;
using TPSS.Common.Wrappers;
using Leemo.Model;
using Leemo.Model.Domain;
using Leemo.Model.ResultModels;
using Leemo.Service.Interface;

namespace Leemo.Api.Controllers
{
    /// <summary>
    /// Log controller for logging with db
    /// </summary>
    [ApiController]
    [Route(Constants.Attrributes.ApiDefaultRoute)]
    public class LogController : BaseController
    {
        private readonly ICommonService _commonService;

        /// <summary>
        /// constructor for initalizing log controller
        /// </summary>
        /// <param name="commonService"></param>
        public LogController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        /// <summary>
        /// Log exception
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route(Constants.Attrributes.Exception)]
        public ApiResponse<LogResultModel> Exception(ErrorLog log)
        {
            log.TimeStamp = DateTime.UtcNow;
            _commonService.LogGlobalExceptionLog(log);
            LogResultModel logResultModel = new LogResultModel()
            {
                Message = log.Message
            };
            return new ApiResponse<LogResultModel>
            {
                Succeeded = true,
                ResponseCode = Enums.HttpStatusCode.OK,
                Message = Constants.Messages.Success,
                Data = logResultModel
            };
        }
    }
}
