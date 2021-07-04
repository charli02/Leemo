using System;
using System.Collections.Generic;
using System.Text;
using Leemo.Repository.Interface;
using Leemo.Service.Interface;
using Lemmo.Service.Interface;

namespace Leemo.Service
{
    public class HostingKeywordService : IHostingKeywordService
    {
        private readonly IHostingKeywordRepository _hostingKeywordRepository;

        public HostingKeywordService(IHostingKeywordRepository hostingKeywordRepository)
        {
            _hostingKeywordRepository = hostingKeywordRepository;
        }

       
    }
}
