using System;

namespace Leemo.Web
{
    /// <summary>
    /// This class represents the sections defined in appSettings.json
    /// </summary>
    public class AppSettings
    {
        public string AllowedHosts { get; set; }

        public Leemo_API_Config Leemo_API_Config { get; set; }

        public PageSettings PageSettings { get; set; }

        public string Resources_BaseDir { get; set; }

        public string Resources_StaticFileContainer { get; set; }

        public string ProfileImagesPath { get; set; }

        public string CompanyImagesPath { get; set; }

        public string GroupImagesPath { get; set; }

        public long MaxImageSize { get; set; }

    }

    public class Leemo_API_Config
    {
        public string BaseUrl { get; set; }
    }

    public class PageSettings
    {
        public int DefaultPageNumber { get; set; }
        public int DefaultPageSize { get; set; }
        public int DefaultGetActiveUsrs { get; set; }
        public string DefaultQuerySearch { get; set; }
        
    }
}
