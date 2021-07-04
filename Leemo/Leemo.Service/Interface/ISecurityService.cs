using System;
using System.Collections.Generic;
using Leemo.Model;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;

/// <summary>
/// Represents service project namespace
/// </summary>
namespace Leemo.Service.Interface
{
    public interface ISecurityService
    {
        IList<Auth_FeatureListWithGeneralCodeResult> GetAuth_FeatureListWithGeneralCodeResults();
    }
}
