using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Leemo.Api
{
    /// <summary>
    /// Program class for API project.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method of API project.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Web host builder for API project.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
