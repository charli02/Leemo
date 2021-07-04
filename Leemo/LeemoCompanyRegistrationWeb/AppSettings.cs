namespace LeemoCompanyRegistrationWeb
{
    public class AppSettings
    {
        public Leemo_API_Config Leemo_API_Config { get; set; }
        public int DomainMaxLength { get; set; }

        public int DomainMinLength { get; set; }

    }
        public class Leemo_API_Config
    {
            public string BaseUrl { get; set; }
        }

    
}
