using System;
using System.Collections.Generic;
using System.Text;
using TPSS.Common.Interfaces;
using Leemo.Model.Domain;

namespace Leemo.Repository.Interface
{
    public interface IHostingKeywordRepository :IRepository<HostingKeyword>
    {
        HostingKeyword GetHostingKeywordByKeyword(string hostingKeyword);
        List<HostingKeyword> GetHostingKeyword();
        
    }
}
