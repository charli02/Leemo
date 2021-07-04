using System;
using System.Collections.Generic;
using System.Text;
using Leemo.Model;
using Leemo.Model.Domain;

namespace Leemo.Repository.Interface
{
    public interface IApiRequestLogRepository
    {
        void InsertApiRequestLog(ApiRequestLog apiRequestLog);
    }
}
