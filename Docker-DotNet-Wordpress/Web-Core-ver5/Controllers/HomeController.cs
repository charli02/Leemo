using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Web_Core_ver5.Hubs;
using Web_Core_ver5.Models;

namespace Web_Core_ver5.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<ProductSetupHub> _productSetupHub;

        public HomeController(ILogger<HomeController> logger
            , IHubContext<ProductSetupHub> productSetupHub
            )
        {
            _logger = logger;
            _productSetupHub = productSetupHub;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> Index(string connectionId)
        {
            //DockerClient client = new DockerClientConfiguration(
            //    new Uri("http://ubuntu-docker.cloudapp.net:4243"))
            //     .CreateClient();

            DockerClient client = new DockerClientConfiguration().CreateClient();
            Random rnd = new Random();

            int port = 6060;

            #region test code
            //Stream stream = await client.Images.CreateImageAsync(
            //    new ImagesCreateParameters
            //    {
            //        FromImage = "harpreetptpss/webapplication1",
            //        Tag = "alpha",
            //    },
            //    new AuthConfig
            //    {
            //        Email = "test@example.com",
            //        Username = "test",
            //        Password = "pa$$w0rd"
            //    });


            //await client.Images.CreateImageAsync(
            //   new ImagesCreateParameters
            //   {
            //       FromImage = "mysql:5.7",
            //       Tag = "latest"
            //   },
            //   new AuthConfig(),
            //   new Progress<JSONMessage>());


            //IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(
            //        new ContainersListParameters()
            //        {
            //            Limit = 10,
            //        });
            #endregion

            //waiting for 20 sec to complete processing
            System.Threading.Thread.Sleep(2000);
            await _productSetupHub.Clients.Client(connectionId).SendAsync("Progress", 20);

            //#region Mysql install
            //{
            //    //set mysql container
            //    var response = await client.Containers.CreateContainerAsync(new CreateContainerParameters
            //    {

            //        Image = "mysql:latest",
            //        Name = "some-mysql",
            //        Env = new List<string>{ "MYSQL_ROOT_PASSWORD=1"
            //                            , "MYSQL_ROOT_HOST=%"
            //                            , "MYSQL_DATABASE=wordpress"
            //                            , "MYSQL_USER=tpss"
            //                            , "MYSQL_PASSWORD=1"},

            //        ExposedPorts = new Dictionary<string, EmptyStruct>
            //        {
            //            {
            //                "3306", default(EmptyStruct)
            //            }
            //        },
            //        HostConfig = new HostConfig
            //        {
            //            //bind to volume /var/lib/mysql
            //            //Binds = new[] { @"c:/temp:/data" },
            //            //Binds = new[] { @"/var/www/html/dockervolumes/mysql:/var/lib/mysql" },
            //            PortBindings = new Dictionary<string, IList<PortBinding>>
            //            {
            //                {"3306", new List<PortBinding> {new PortBinding {HostPort = port.ToString() } }}
            //            },
            //            PublishAllPorts = true
            //        }
            //    });

            //    //var cntrParameters = new CreateContainerParameters(new Config
            //    //{
            //    //    Image = "mysql:5.7",
            //    //    Env = new List<string>{ "MYSQL_ROOT_PASSWORD=somewordpress"
            //    //                            , "MYSQL_DATABASE=wordpress"
            //    //                            , "MYSQL_USER=wordpress"
            //    //                            , "MYSQL_PASSWORD=wordpress" },
            //    //    //Env = new List<string> { "PARTITION_KEY = 'NA'" },
            //    //});
            //    //var response = await client.Containers.CreateContainerAsync(cntrParameters);

            //    string _containerId;
            //    _containerId = response.ID;
            //    await client.Containers.StartContainerAsync(_containerId, new ContainerStartParameters());
            //}
            //#endregion



            System.Threading.Thread.Sleep(2000);
            await _productSetupHub.Clients.Client(connectionId).SendAsync("Progress", 40);

            int port2; //new Random(3000).Next();

            port2 = rnd.Next(82, 3000);

            //#region pull Image

            //var progress = new Progress<JSONMessage>();
            //var task = PullImage(client,
            //    new ImagesCreateParameters()
            //    {
            //        FromImage = "mcr.microsoft.com/dotnet/core/sdk",
            //        Tag = "latest"
            //    }, null,
            //    progress);
            //task.Wait();

            //#endregion

            #region WordPressInstall
            {
                //pull wordpress image
                var progress = new Progress<JSONMessage>();
                var task = PullImage(client,
                    new ImagesCreateParameters()
                    {
                        FromImage = "library/wordpress",
                        Tag = "latest"
                    }, null,
                    progress);
                task.Wait();


                //create container
                var response = await client.Containers.CreateContainerAsync(new CreateContainerParameters
                {

                    Image = "library/wordpress",
                    //Image = "harpreetptpss/webapplication4:latest",
                    Name = "wordpress-example" + port2.ToString(),
                    Env = new List<string>{ "WORDPRESS_DB_HOST=192.168.0.79:3306"
                                        , "WORDPRESS_DB_USER=tpss"
                                        , "WORDPRESS_DB_PASSWORD=1"
                                        //, "WORDPRESS_DB_NAME=wordpress"+port2.ToString()
                                        , "WORDPRESS_DB_NAME=wordpress"
                                        },

                    ExposedPorts = new Dictionary<string, EmptyStruct>
                    {
                        {
                            "80", default(EmptyStruct)
                        }
                    },
                    HostConfig = new HostConfig
                    {
                        //bind to volume /var/lib/mysql
                        //Binds = new[] { @"/var/www/html/dockervolumes/wordpress-site1" },
                        PortBindings = new Dictionary<string, IList<PortBinding>>
                        {
                            {"80", new List<PortBinding> {new PortBinding {HostPort = port2.ToString() } }}
                        },
                        PublishAllPorts = true
                    }
                });

                string _containerId;
                _containerId = response.ID;
                await client.Containers.StartContainerAsync(_containerId, new ContainerStartParameters());
            }

            #endregion

            System.Threading.Thread.Sleep(2000);
            await _productSetupHub.Clients.Client(connectionId).SendAsync("Progress", 60);

            System.Threading.Thread.Sleep(2000);
            await _productSetupHub.Clients.Client(connectionId).SendAsync("Progress", 80);

            //System.Threading.Thread.Sleep(10000);

            System.Threading.Thread.Sleep(2000);
            await _productSetupHub.Clients.Client(connectionId).SendAsync("Finished");
            //return View();
                       
            //return RedirectToAction("InstallationDone", "Home");
            return ("http://localhost:" + port2.ToString());
        }


        private Task PullImage(DockerClient dockerClient, ImagesCreateParameters imagesCreateParameters, AuthConfig authConfig,
            Progress<JSONMessage> progress)
        {
            return dockerClient.Images.CreateImageAsync(imagesCreateParameters, authConfig, progress);
        }


        [HttpPost]
        public async Task<IActionResult> OrderCoffee([FromBody] Order order)
        {
            await _productSetupHub.Clients.All.SendAsync("NewOrder", order);
            //Save order somewhere and get order id
            return Accepted(1); //return order id
        }

        //public async Task<IActionResult> ShowProgress()
        //{
        //    System.Threading.Thread.Sleep(1000);
           

        //        await _productSetupHub.Clients.Client(_productSetupHub.get).SendAsync("ReceiveOrderUpdate",   result.Update);

        //    System.Threading.Thread.Sleep(1000);
        //    if (result.New)
        //        await Clients.Caller.SendAsync("ReceiveOrderUpdate",
        //            result.Update);
        //    return Accepted();
        //}

        public IActionResult InstallationDone()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
