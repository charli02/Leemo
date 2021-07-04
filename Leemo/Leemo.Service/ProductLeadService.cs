using System;
using System.Collections.Generic;
using System.Text;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Repository.Interface;
using Leemo.Service.Interface;
using TPSS.Common.Interfaces;
using TPSS.Common;
using Microsoft.Extensions.Options;
using Leemo.Model.ResultModels;
using System.Text.RegularExpressions;
using System.Linq;
using Lemmo.Service.Interface;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using MimeKit;

namespace Leemo.Service
{
    public class ProductLeadService: IProductLeadService
    {
        private readonly IProductLeadRepository _productLeadRepository;
      
        private readonly ICompanyProductMappingRepository _companyProductMappingRepository;
        private readonly AppSettings _appSettings;
        private readonly IAddressTypeService _addressTypeService;
        private readonly IHostingKeywordRepository _hostingKeywordRepository;
        

        public ProductLeadService(IProductLeadRepository productLeadRepository, 
            IOptions<AppSettings> appSettings,
            ICompanyProductMappingRepository companyProductMappingRepository,
            IAddressTypeService addressTypeService, IHostingKeywordRepository hostingKeywordRepository
            )
        {
            _productLeadRepository = productLeadRepository;
           
            _companyProductMappingRepository = companyProductMappingRepository;
            _appSettings = appSettings.Value;
            _addressTypeService = addressTypeService;
           _hostingKeywordRepository = hostingKeywordRepository;
           
        }

       public InputProductLead CreateProductLead(InputProductLead inputProductLead) {

            var productLead = new ProductLead();
            if (inputProductLead != null)
            {
                productLead.ProductId = inputProductLead.ProductId;
                productLead.ProductPackageId = inputProductLead.ProductPackageId;
                productLead.Email = inputProductLead.Email;
                productLead.CompanyName = inputProductLead.CompanyName;
                productLead.FullName = inputProductLead.FullName;
                productLead.CountryCode = inputProductLead.CountryCode;
                productLead.Phone = inputProductLead.Phone;
                productLead.CreatedDate = DateTime.UtcNow;
                productLead.IpAddress = inputProductLead.IpAddress;
                productLead.MacAddress = inputProductLead.MacAddress;
                 var  DomainName = CommonFunction.CompanyNametoSuggestedDomainName(productLead.CompanyName);
               
                var CheckAvailableDomainName = GetProductLeadCheckAvailableDomain(DomainName);
                if(CheckAvailableDomainName == Constants.ResponseType.NotFound)
                {
                    productLead.DomainName = DomainName;
                }




                productLead.IsVerified = false;
                _productLeadRepository.Add(productLead);
                _productLeadRepository.Save();

                //GetingUserNAme Split from @
                //var EmailSendto = "";

                //if (productLead.Email != null)
                //{
                //    var CompanyEmail = productLead.Email.Split('@');
                //    if (CompanyEmail.Length > 0)
                //    {
                //        EmailSendto = CompanyEmail[0];
                //    }
                //}

                ////Encoding ProductLead Id
                //var Id = CommonFunction.EncodeData(productLead.Id.ToString());

                //string filepath = string.Concat(_appSettings.Resources_BaseDir, _appSettings.EmailTemplatePath) + Path.DirectorySeparatorChar.ToString()
                //        + "Confirm_Company_Registration.html"; 

                //var builder = new BodyBuilder();
                //using (StreamReader SourceReader = System.IO.File.OpenText(filepath))
                //{
                //    builder.HtmlBody = SourceReader.ReadToEnd();
                //}
              
                //var bodymsg = builder.HtmlBody;

                // bodymsg = bodymsg.Replace("||ConfirmAccountWebUrlLeemoSetup||", _appSettings.sendingEmailSettings.LeemoCompanyRegistrationWeb + "/" + Constants.WebConstants.Urls.CompanyRegistration + "/"+ Id);
                //bodymsg = bodymsg.Replace("||WebUrlLeemoSetup||", _appSettings.sendingEmailSettings.LeemoCompanyRegistrationWeb + "/");
                //bodymsg = bodymsg.Replace("||Email||", EmailSendto);
                //bodymsg = bodymsg.Replace("||CompanyName||", productLead.CompanyName);

                ////string bodymsg = String.Format("<p style=\"padding:0;font-family:'Segoe UI Light','Segoe UI','Helvetica Neue Medium',Arial,sans-serif;font-size:41px;color:#2672ec\">Verify your email address</p><p>Dear <span style=\"color:#031121\">{4},</span></p> <p>We are glad to see {5} on Uproster.io by leemo. </p><p> We wish you a very easy & fruitful experince with our services. To activate your account, Please verify your email, </p><p> <a style=\"box-sizing:border-box;text-decoration:none;background-color:#529eef;border-radius:5px;display:inline-block;font-size:14px;font-weight:bold;margin:0;padding:10px 20px;border:1px solid #0366d6;color:#031121\" href=\"{1}/{2}/{3}\" class=\"btn-sucess\"> verify Email Address</a>  </p><p> Our Business development team, support team and someone from senior management team will contact you to ensure a sweet journey with us. Sincerly Yours,</p><p> Team Uproster.io - A Product By Leemo</p>",   productLead.CompanyName, _appSettings.sendingEmailSettings.WebUrlLeemoSetup, Constants.WebConstants.Urls.CompanyVerify,productLead.Id, productLead.Email,productLead.CompanyName);
                //CommonFunction.sendEmail(_appSettings.sendingEmailSettings.From, productLead.Email, bodymsg, Constants.EmailConstants.EmailSubjects.Verfiy, _appSettings.sendingEmailSettings.Password, _appSettings.sendingEmailSettings.Host, _appSettings.sendingEmailSettings.Port, _appSettings.sendingEmailSettings.EnableSsl, _appSettings.sendingEmailSettings.IsBodyHtml, _appSettings.sendingEmailSettings.UseDefaultCredentials, _appSettings.sendingEmailSettings.alias);

                return GetProductLeadById(productLead.Id);
            }
          
            return inputProductLead;
        }


        public UpdateInputProductLead VerifyInputProductLeadById(string Id) {

            //Encoding ProductLead Id
            var productLeadId = CommonFunction.DecodeData(Id);

            UpdateInputProductLead updateInputProductLead = new UpdateInputProductLead();
            var productLead = _productLeadRepository.GetById(Guid.Parse(productLeadId));
            if (productLead != null)
            {
                updateInputProductLead.Id = productLead.Id;
                updateInputProductLead.Phone = productLead.Phone;
                
                updateInputProductLead.DomainName = productLead.DomainName;
               
                //Verify the ProductLead 
                if (productLead.IsVerified == false) {
                    productLead.IsVerified = true;
                    productLead.VerificationDate = DateTime.Now;
                    //update ProductLead [IsVerified==true]
                    _productLeadRepository.Edit(productLead);
                    _productLeadRepository.Save();
                   
                }
               
                

                return updateInputProductLead;
            }
            return null;
        }


       

        public InputProductLead GetProductLeadById(Guid Id)
        {
            InputProductLead inputProductLead = new InputProductLead();
            var productLead = _productLeadRepository.GetById(Id);
            if (productLead != null)
            {
                inputProductLead.Id = productLead.Id;
                inputProductLead.IdVerify = CommonFunction.EncodeData(productLead.Id.ToString());
                inputProductLead.ProductId = productLead.ProductId;
                inputProductLead.ProductPackageId = productLead.ProductPackageId;
                inputProductLead.Email = productLead.Email;
                inputProductLead.CompanyName = productLead.CompanyName;
                inputProductLead.Phone = productLead.Phone;
                inputProductLead.CreatedDate = productLead.CreatedDate;
                inputProductLead.IpAddress = productLead.IpAddress;
                inputProductLead.MacAddress = productLead.MacAddress;
                inputProductLead.IsVerified = productLead.IsVerified;          
                inputProductLead.FullName = productLead.FullName;
                inputProductLead.CountryCode = productLead.CountryCode;
                return inputProductLead;
            }
            return null;

        }
            
     

        public bool GetProductLeadByEmail(string email)
        {

            var emailSearch= _productLeadRepository.GetProductLeadByEmail(email);
            if (emailSearch != null)
            {
              
                return true;
            }
            else {
                return false;
            }

        }

        public bool GetProductLeadByCompanyName(string CompanyName)
        {

            var productLead = _productLeadRepository.GetByCompanyName(CompanyName);
            if (productLead != null)
            {

                return true;
            }
            else
            {
                return false;
            }

        }

        public int GetProductLeadCheckAvailableDomain(string domainName)
        {
                     
            var flag = CheckHostingKeyword(domainName);
            if (flag == false)
            {
                var domainNameFind = _companyProductMappingRepository.GetProductLeadCheckAvailableDomain(domainName);
                if (domainNameFind != null)
                {
                    return Constants.ResponseType.AlreadyExists;
                }
                else
                {
                    return Constants.ResponseType.NotFound;
                }
            }
            return Constants.ResponseType.AlreadyExists;

        }
        // Generates a random number within a range.
        public List<string> RandomNumberWithDomainSuggestion(string suggestedDomainName)
        {
            var flag = CheckHostingKeyword(suggestedDomainName);
            if (flag == false)
            { 
                int min = _appSettings.RandomNumberWithDomainMinLength, max = _appSettings.RandomNumberWithDomainMaxLength;
            Random random = new Random();
            List<string> result = new List<string>();

            for (int i = 0; i < 3;)
            {
                var randomNumber = random.Next(min, max);
                suggestedDomainName = suggestedDomainName + randomNumber;

                var domainNameFind = _companyProductMappingRepository.GetProductLeadCheckAvailableDomain(suggestedDomainName);
                if (domainNameFind == null)
                {
                    result.Add(suggestedDomainName);
                    i++;
                }
            }
                return result;
        }

            return null;
        }


        public Boolean CheckHostingKeyword(string domainName){

            //Get domain name starting
            var GetdomainNameStartwith = CommonFunction.RegexPatternCheckforDomainNamebyHostingkeyword(domainName.Trim().ToLower());

            var checkHostingKeyword = GetdomainNameStartwith;
            while (checkHostingKeyword.Length > 0 && checkHostingKeyword.Length >= _appSettings.DomainMinLength)
            {
                //Validate GetdomainNameStartwith not exist in data
                var resultHostingKeyword = _hostingKeywordRepository.GetHostingKeywordByKeyword(checkHostingKeyword);
                if (resultHostingKeyword == null)
                {
                    //Deleteing last char of given string
                    checkHostingKeyword = checkHostingKeyword.Remove(checkHostingKeyword.Length - 1);
                    continue;
                }
                //return is true when domainName is found in  HostingKeyword table 
                return true;
          

            }

            //return is true when domainName is not found in  HostingKeyword table 
            return false;
        }
        //currentAddress.AddressTypeId = _addressTypeService.GetAddressTypeIdWithName(Constants.AddressTypeNames.ProductLeadAddress);


        public ResultProductLead EditUser(UpdateInputProductLead updateInputProductLead) {


            if (updateInputProductLead.Id != null)
            {
                if (updateInputProductLead.Country == null)
                        return GetResultProductLeadById(updateInputProductLead.Id);
             

           


            //if (updateInputProductLead.NewPassword != null)
            //{
            //    string hashedPasswordWithSalt = CommonFunction.HashPassword(updateInputProductLead.NewPassword, null, false);
            //    var passwordAndHash = hashedPasswordWithSalt.Split(':');
            //}

          
            var productLead = _productLeadRepository.GetById(updateInputProductLead.Id);
                    if (productLead != null)
                    {
                        productLead.AddressLine1 = updateInputProductLead.AddressLine1;
                        productLead.City = updateInputProductLead.City;
                        productLead.DomainName = updateInputProductLead.DomainName;
                    productLead.AddressLine2 = updateInputProductLead.AddressLine2;
                    productLead.Country = updateInputProductLead.Country;
                        productLead.ZipCode = updateInputProductLead.ZipCode;
                        productLead.State = updateInputProductLead.State;
                    productLead.Fax = updateInputProductLead.Fax;

                        //if (passwordAndHash != null || passwordAndHash.Length > 1)
                        //{
                        //    productLead.PasswordHash = passwordAndHash[0];
                        //    productLead.PasswordSalt = passwordAndHash[1];
                        //}

                        _productLeadRepository.Edit(productLead);
                        _productLeadRepository.Save();


                    }
                 
            
            }
            return GetResultProductLeadById(updateInputProductLead.Id);
        }

       public ResultProductLead GetResultProductLeadById(Guid Id) {

            ResultProductLead resultProductLead = new ResultProductLead();
            var productLead = _productLeadRepository.GetById(Id);
            if (productLead != null)
            {
                resultProductLead.Id = productLead.Id;
                resultProductLead.ProductId = productLead.ProductId;
                resultProductLead.ProductPackageId = productLead.ProductPackageId;
                resultProductLead.Email = productLead.Email;
                resultProductLead.CompanyName = productLead.CompanyName;
                resultProductLead.Phone = productLead.Phone;
                resultProductLead.CreatedDate = productLead.CreatedDate;
                resultProductLead.IpAddress = productLead.IpAddress;
                resultProductLead.MacAddress = productLead.MacAddress;
                resultProductLead.IsVerified = productLead.IsVerified;
                resultProductLead.AddressLine1 = productLead.AddressLine1;
                resultProductLead.Fax = productLead.Fax;
                resultProductLead.AddressLine2 = productLead.AddressLine2;
                resultProductLead.City = productLead.City;
                resultProductLead.DomainName = productLead.DomainName;
                resultProductLead.Street = productLead.Street;
                resultProductLead.Country = productLead.Country;
                resultProductLead.ZipCode = productLead.ZipCode;
                resultProductLead.State = productLead.State;
                resultProductLead.FullName = productLead.FullName;

                return resultProductLead;
            }
            return null;


        }
    }
}
