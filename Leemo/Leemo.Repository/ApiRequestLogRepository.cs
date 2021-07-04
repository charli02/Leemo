using System;
using System.Collections.Generic;
using System.Text;
using TPSS.Common.Implementations;
using Leemo.Data;
using Leemo.Model;
using Leemo.Model.Domain;
using Leemo.Repository.Interface;

namespace Leemo.Repository.Repository
{
    public class ApiRequestLogRepository : RepositoryBase<ApiRequestLog, LeemoDbContext>, IApiRequestLogRepository
    {
        //private LeemoDbContext _context;

        public ApiRequestLogRepository(LeemoDbContext context) : base(context)
        {
            //_context = context;
        }

        public void InsertApiRequestLog(ApiRequestLog apiRequestLog)
        {
            Context.ApiRequestLog.Add(apiRequestLog);
            Context.SaveChanges();
        }
    }
}
