using DockerExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using System.IO;

namespace DockerExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string test)
        {
            //DockerClient client = new DockerClientConfiguration(
            //    new Uri("http://ubuntu-docker.cloudapp.net:4243"))
            //     .CreateClient();

            DockerClient client = new DockerClientConfiguration().CreateClient();

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

            //#region Mysql install
            //{
            //    //set mysql container
            //    var response = await client.Containers.CreateContainerAsync(new CreateContainerParameters
            //    {

            //        Image = "mysql:latest",
            //        Name = "some-mysql",
            //        Env = new List<string>{ "MYSQL_ROOT_PASSWORD=my-secret-pw"
            //                            , "MYSQL_ROOT_HOST=%"
            //                            , "MYSQL_DATABASE=wordpress"
            //                            , "MYSQL_USER=wordpress"
            //                            , "MYSQL_PASSWORD=wordpress"},

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
            //            Binds = new[] { @"c:/temp:/data" },
            //            PortBindings = new Dictionary<string, IList<PortBinding>>
            //            {
            //                {"3306", new List<PortBinding> {new PortBinding {HostPort = "6603" } }}
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

            ////waiting for 20 sec to complete processing
            //System.Threading.Thread.Sleep(20000);

            #region WordPressInstall
            {
                //set mysql container
                var response = await client.Containers.CreateContainerAsync(new CreateContainerParameters
                {

                    Image = "wordpress:latest",
                    Name = "wordpress-example",
                    Env = new List<string>{ "WORDPRESS_DB_HOST=192.168.1.100:6603"
                                        , "WORDPRESS_DB_USER=wordpress"
                                        , "WORDPRESS_DB_PASSWORD=wordpress"
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
                        //Binds = new[] { @"c:/temp:/data" },
                        PortBindings = new Dictionary<string, IList<PortBinding>>
                        {
                            {"80", new List<PortBinding> {new PortBinding {HostPort = "8000" } }}
                        },
                        PublishAllPorts = true
                    }
                });

                string _containerId;
                _containerId = response.ID;
                await client.Containers.StartContainerAsync(_containerId, new ContainerStartParameters());
            }

            System.Threading.Thread.Sleep(10000);
            #endregion


            return View();
        }

        //private async Task StartContainer()
        //{
        //    var response = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
        //    {
        //        Image = "amazon/dynamodb-local",
        //        ExposedPorts = new Dictionary<string, EmptyStruct>
        //            {
        //                {
        //                    "8000", default(EmptyStruct)
        //                }
        //            },
        //            HostConfig = new HostConfig
        //            {
        //                PortBindings = new Dictionary<string, IList<PortBinding>>
        //                {
        //                    {"8000", new List<PortBinding> {new PortBinding {HostPort = "8000"}}}
        //                },
        //                PublishAllPorts = true
        //            }
        //    });
        //}

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
