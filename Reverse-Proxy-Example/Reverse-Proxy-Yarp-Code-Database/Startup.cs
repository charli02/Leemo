using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yarp.ReverseProxy.Abstractions;
using Yarp.ReverseProxy.Middleware;
using Yarp.ReverseProxy.RuntimeModel;
using Yarp.Sample;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Reverse_Proxy_Yarp_Code_Database
{
    /// <summary>
    /// Initialiaztion for ASP.NET using YARP reverse proxy
    /// </summary>
    public class Startup
    {
        private const string DEBUG_HEADER = "Debug";
        private const string DEBUG_METADATA_KEY = "debug";
        private const string DEBUG_VALUE = "true";
        List<string> host1 = new List<string>();
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// 
        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Use a MS SQL Server database
            var sqlConnectionString = @"Data Source=192.168.1.102\SQL2017,50352;Initial Catalog=xlmsDbV2;User ID=xlmsDbUser; Password=0d)n1L#;MultipleActiveResultSets=True;connect timeout=100";

            services.AddDbContext<DbHostingContext>(options =>
                options.UseSqlServer(sqlConnectionString)
            );


            services.AddTransient<IHostingService, HostingService>();
            var scopeFactory = services.BuildServiceProvider().GetService<IHostingService>();
            var hostings = scopeFactory.GetHostings().ToList();


            // Specify a custom proxy config provider, in this case defined in InMemoryConfigProvider.cs
            // Programatically creating route and cluster configs. This allows loading the data from an arbitrary source.
            //services.AddReverseProxy()
            //    .LoadFromMemory(GetRoutes(), GetClusters());            
            
            services.AddReverseProxy()
                .LoadFromMemory(GetRoutes(hostings), GetClusters(hostings));

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            //using (var serviceScope = app.ApplicationServices.CreateScope())
            //{
            //    var services = serviceScope.ServiceProvider;
            //    var myDbContext = services.GetService<DbSampleContext>();
            //    host1.Add("testing.local");

            //    //var testdata = myDbContext.User.ToList();

            //}

            app.UseEndpoints(endpoints =>
            {
                // We can customize the proxy pipeline and add/remove/replace steps
                endpoints.MapReverseProxy(proxyPipeline =>
                {
                    // Use a custom proxy middleware, defined below
                    proxyPipeline.Use(MyCustomProxyStep);
                    // Don't forget to include these two middleware when you make a custom proxy pipeline (if you need them).
                    proxyPipeline.UseAffinitizedDestinationLookup();
                    proxyPipeline.UseProxyLoadBalancing();
                });
            });
        }

        #region sample routing info
        //private ProxyRoute[] GetRoutes()
        //{
            
            

        //    return new[]
        //    {
        //        new ProxyRoute()
        //        {
        //            RouteId = "route2",
        //            ClusterId = "cluster2",
        //            Match = new ProxyMatch
        //            {
        //                Hosts = host1,
        //                // Path or Hosts are required for each route. This catch-all pattern matches all request paths.
        //                Path = "{**catch-all}"
        //            }
        //        },
        //        new ProxyRoute()
        //        {
        //            RouteId = "route3",
        //            ClusterId = "cluster3",
        //            Match = new ProxyMatch
        //            {
        //                Hosts = host1,
        //                // Path or Hosts are required for each route. This catch-all pattern matches all request paths.
        //                Path = "/something/{*any}"
        //            }
        //        },
        //        new ProxyRoute()
        //        {
        //            RouteId = "route1",
        //            ClusterId = "cluster1",
        //            Match = new ProxyMatch
        //            {
        //                // Path or Hosts are required for each route. This catch-all pattern matches all request paths.
        //                Path = "{**catch-all}"
        //            }
        //        }
        //    };
        //}

        //private Cluster[] GetClusters()
        //{
        //    var debugMetadata = new Dictionary<string, string>();
        //    debugMetadata.Add(DEBUG_METADATA_KEY, DEBUG_VALUE);

        //    return new[]
        //    {
        //        new Cluster()
        //        {
        //            Id = "cluster1",
        //            SessionAffinity = new SessionAffinityOptions { Enabled = true, Mode = "Cookie" },
        //            Destinations = new Dictionary<string, Destination>(StringComparer.OrdinalIgnoreCase)
        //            {
        //                { "destination1", new Destination() { Address = "http://localhost:8081/" } },
        //                //{ "debugdestination1", new Destination() {
        //                //    Address = "https://bing.com",
        //                //    Metadata = debugMetadata  }
        //                //},
        //            }
        //        },
        //        new Cluster()
        //        {
        //            Id = "cluster2",
        //            SessionAffinity = new SessionAffinityOptions { Enabled = true, Mode = "Cookie" },
        //            Destinations = new Dictionary<string, Destination>(StringComparer.OrdinalIgnoreCase)
        //            {
        //                { "destination1", new Destination() { Address = "http://localhost:8085/" } },
        //                //{ "debugdestination1", new Destination() {
        //                //    Address = "https://bing.com",
        //                //    Metadata = debugMetadata  }
        //                //},
        //            }
        //        },
        //        new Cluster()
        //        {
        //            Id = "cluster3",
        //            SessionAffinity = new SessionAffinityOptions { Enabled = true, Mode = "Cookie" },
        //            Destinations = new Dictionary<string, Destination>(StringComparer.OrdinalIgnoreCase)
        //            {
        //                { "destination1", new Destination() { Address = "http://localhost:2500/" } },
        //                //{ "debugdestination1", new Destination() {
        //                //    Address = "https://bing.com",
        //                //    Metadata = debugMetadata  }
        //                //},
        //            }
        //        }
        //    };
        //}
        #endregion


        #region hosting info
        private ProxyRoute[] GetRoutes(List<HostingInfo> hostingInfos)
        {
            List<ProxyRoute> proxyRoutes = new List<ProxyRoute>();
            foreach(var hosting in hostingInfos)
            {
                var proxyRoute = new ProxyRoute()
                {
                    ClusterId = hosting.Id.ToString(),
                    RouteId = hosting.Id.ToString(),
                    Match = new ProxyMatch
                    {
                        Hosts = (new List<string>(){hosting.Host }),
                        Path = "{**catch-all}"
                    }
                };
                proxyRoutes.Add(proxyRoute);
            }

            return proxyRoutes.ToArray();
        }

        private Cluster[] GetClusters(List<HostingInfo> hostingInfos)
        {
            var debugMetadata = new Dictionary<string, string>();
            debugMetadata.Add(DEBUG_METADATA_KEY, DEBUG_VALUE);

            List<Cluster> Clusters = new List<Cluster>();
            foreach (var hosting in hostingInfos)
            {
                Cluster cluster = new Cluster()
                {
                    Id = hosting.Id.ToString(),
                    SessionAffinity = new SessionAffinityOptions { Enabled = true, Mode = "Cookie" },
                    Destinations = new Dictionary<string, Destination>(StringComparer.OrdinalIgnoreCase)
                    {
                        { hosting.Id.ToString(), new Destination() { Address = hosting.DockerContainer } }
                    }
                };

                Clusters.Add(cluster);
            }
            return Clusters.ToArray();            
        }

        #endregion

        /// <summary>
        /// Custom proxy step that filters destinations based on a header in the inbound request
        /// Looks at each destination metadata, and filters in/out based on their debug flag and the inbound header
        /// </summary>
        public Task MyCustomProxyStep(HttpContext context, Func<Task> next)
        {
            // Can read data from the request via the context
            var useDebugDestinations = context.Request.Headers.TryGetValue(DEBUG_HEADER, out var headerValues) && headerValues.Count == 1 && headerValues[0] == DEBUG_VALUE;

            // The context also stores a ReverseProxyFeature which holds proxy specific data such as the cluster, route and destinations
            var availableDestinationsFeature = context.Features.Get<IReverseProxyFeature>();
            var filteredDestinations = new List<DestinationInfo>();

            // Filter destinations based on criteria
            foreach (var d in availableDestinationsFeature.AvailableDestinations)
            {
                //Todo: Replace with a lookup of metadata - but not currently exposed correctly here
                if (d.DestinationId.Contains("debug") == useDebugDestinations) { filteredDestinations.Add(d); }
            }
            availableDestinationsFeature.AvailableDestinations = filteredDestinations;

            // Important - required to move to the next step in the proxy pipeline
            return next();
        }


        #region Sample Service
        public interface IHostingService
        {
            IList<HostingInfo> GetHostings();
        }

        public class HostingService : IHostingService
        {
            public readonly DbHostingContext _dbHostingContext;
            public HostingService(DbHostingContext dbHostingContext)
            {
                _dbHostingContext = dbHostingContext;
            }

            public IList<HostingInfo> GetHostings()
            {
                return _dbHostingContext.HostingInfo.ToList();
            }

        }

        #endregion
    }
}
