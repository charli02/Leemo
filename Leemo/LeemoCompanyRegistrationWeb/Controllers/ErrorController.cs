using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TPSS.Common;
using Leemo.Model;
using Leemo.Model.Domain;
using Leemo.Model.ResultModels;
using LeemoCompanyRegistrationWeb.Web.HttpClient;
using LeemoCompanyRegistrationWeb.Models;

namespace LeemoCompanyRegistrationWeb.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppSettings _appSettings;

        public ErrorController(ILogger<HomeController> logger, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route(Constants.WebConstants.Urls.WEB_Route_404)]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Index()
        {
            // Retrieve error information in case of internal errors
            var error = HttpContext
                      .Features
                      .Get<IExceptionHandlerFeature>();

            if (error == null)
                return View(new ErrorViewModel { Message = error.Error.Message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            else
            {
                // Use the information about the exception 
                var exception = error.Error;

                var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
                ErrorLog log = new ErrorLog()
                {
                    Message = exception.Message,
                    TimeStamp = DateTime.UtcNow,
                    ProjectSource = Constants.ProjectSourceWEB,
                    RequestPath = exceptionHandlerPathFeature.Path,
                    StackTrace = exception.StackTrace
                };

                var response = await HttpRequestFactory.Post(string.Format("{0}{1}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_LogException), log);
                if (!(response.IsSuccessStatusCode))
                {
                    //return Json("400");
                }
                var result = response.ContentAsType<LogResultModel>();
                if (result != null)
                {
                    //TODO: Display error information to view.
                }
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
