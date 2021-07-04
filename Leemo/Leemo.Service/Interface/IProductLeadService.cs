using System;
using System.Collections.Generic;
using System.Text;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;

namespace Lemmo.Service.Interface
{
    public interface IProductLeadService
    {
        InputProductLead CreateProductLead(InputProductLead inputProductLead);
        UpdateInputProductLead VerifyInputProductLeadById(string id);

        
        InputProductLead GetProductLeadById(Guid Id);
        Boolean  GetProductLeadByEmail(string email);
        Boolean GetProductLeadByCompanyName(string CompanyName);
        Boolean CheckHostingKeyword(string domainName);
        int GetProductLeadCheckAvailableDomain(string domain);

        List<string> RandomNumberWithDomainSuggestion(string name);

        ResultProductLead EditUser(UpdateInputProductLead updateInputProductLead);
        ResultProductLead GetResultProductLeadById(Guid Id);
        
    }
}
