using System;

namespace TPSS.Common
{
    /// <summary>
    /// This class represents the sections defined in appSettings.json
    /// </summary>
    public class AppSettings
    {
        public string AllowedHosts { get; set; }

        public ConnectionStrings connectionStrings { get; set; }

        public SwaggerSettings swaggerSettings { get; set; }

        public Logging logging { get; set; }

        public Jwt jwt { get; set; }

        public PasswordSettings passwordSettings { get; set; }

        public ApiResponsePageSettings apiResponsePageSettings { get; set; }

        public SendingEmailSettings sendingEmailSettings { get; set; }

        public int CommandTimeout { get; set; }

        public string RootDesignationId { get; set; }

        public int RandomNumberWithDomainMaxLength { get; set; }
        public int RandomNumberWithDomainMinLength { get; set; }

        public int DomainMaxLength { get; set; }

        public int DomainMinLength { get; set; }


        public string Resources_BaseDir { get; set; }

        public string Resources_StaticFileContainer { get; set; }

        public string EmailTemplatePath { get; set; }
    }

    public class ConnectionStrings
    { 
        public string Leemo_API_DbConnection { get; set; }
    }

    public class SwaggerSettings
    { 
        public string Url { get; set; }
        public string Name { get; set; }
    }

    public class Logging
    { 
        public bool IncludeScopes { get; set; }
        public LogLevel logLevel { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
        public string System { get; set; }
        public string Microsoft { get; set; }
    }

    public class Jwt
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }
        public string ExpiryTime { get; set; }
    }

    public class PasswordSettings
    {
        public string RandomPasswordValidCharacters { get; set; }
        public int PasswordLength { get; set; }
    }

    public class ApiResponsePageSettings
    {
        public int DefaultPageNumber { get; set; }
        public int DefaultPageSize { get; set; }
        //Testing Purpose for Search
        public string DefaultQuerySearch { get; set; }
        //TESTING PURPOSE FOR GET ACTIVE USERS
        public int DefaultGetActiveUsrs { get; set; }
    }

    public class SendingEmailSettings
    {
        public string From { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string WebUrl { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string LeemoCompanyRegistrationWeb { get; set; }
        public bool IsBodyHtml { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public string alias { get; set; }
    }
}
