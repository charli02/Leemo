using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TPSS.Common.Implementations;
using Leemo.Data;
using Leemo.Model.Domain;
using Leemo.Repository.Interface;

namespace Leemo.Repository
{
    public class HostingKeywordRepository : RepositoryBase<HostingKeyword, LeemoDbContext>, IHostingKeywordRepository
    {

        //private XLMSDbContext _context;

        public HostingKeywordRepository(LeemoDbContext context) : base(context)
        {
            //_context = context;
        }

        public HostingKeyword GetHostingKeywordByKeyword(string hostingKeyword)
        {

            return Context.HostingKeyword.Where(x => x.Keyword.Trim().ToLower()==hostingKeyword.Trim().ToLower() && x.IsValid == false).FirstOrDefault();

        }

        public List<HostingKeyword> GetHostingKeyword()
        {

            return Context.HostingKeyword.Where(x => x.IsValid==false).ToList();

        }
    }
}
