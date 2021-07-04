using System;
using System.Collections.Generic;
using Leemo.Model.Domain;

/// <summary>
/// Represents service project namespace
/// </summary>
namespace Leemo.Service.Interface
{
    public interface IDesignationService
    {
        public IEnumerable<Designation> GetDesignations();

        public Designation GetDesignation(Guid Id);

        void CreateDesignation(Designation designation);

        Designation EditDesignation(Designation designation);

        //public Designation EditDesignation(Designation designation);

        void DeleteDesignation(Designation designation);
        void DeleteDesignation(Guid DesignationId, out int retVal, out string errorMsg);
        public Designation GetDesignationByName(string name,Guid companyLocationId);
    }
}
