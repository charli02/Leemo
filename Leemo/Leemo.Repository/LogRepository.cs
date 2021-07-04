using TPSS.Common.Implementations;
using Leemo.Data;
using Leemo.Model;
using Leemo.Model.Domain;
using Leemo.Repository.Interface;

/// <summary>
/// Represents Leemo repository project namespace
/// </summary>
namespace Leemo.Repository.Repository
{
    /// <summary>
    /// Represents log repository for its CRUD and other custom functions.
    /// </summary>
    public class LogRepository : RepositoryBase<ErrorLog, LeemoDbContext>, ILogReposiory
    {
        //private LeemoDbContext _context;

        public LogRepository(LeemoDbContext context): base(context)
        {
            //_context = context;
        }

        public void InsertLog(ErrorLog log)
        {
            Context.ErrorLog.Add(log);
            Context.SaveChanges();
        }
    }
}
