using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using Leemo.Api.Filters;
using Leemo.Api.Middlewares;
using TPSS.Common;
using Leemo.Data;
using Leemo.Repository.Interface;
using Leemo.Repository.Repository;
using Leemo.Service;
using Leemo.Service.Interface;
using Leemo.Repository;
using Lemmo.Service.Interface;

namespace Leemo.Api
{
    /// <summary>
    /// Entry point of the application and used to configure the request pipeline which handles all requests made to the application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initialise starup class with required stuff.
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Proerties area
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container. 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<LeemoDbContext>(options =>
            //    options.UseSqlServer(Configuration[Constants.ConnectionSettingPath]));

            // Use a MS SQL Server database
            var sqlConnectionString = Configuration[Constants.ConnectionSettingPath];

            services.AddDbContext<LeemoDbContext>(options =>
                options.UseSqlServer(sqlConnectionString)
            );


            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddSwaggerGen(swagger =>
            {
                // To Enable authorization using Swagger (JWT)  
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            services.AddSingleton(c => Configuration);

            services.AddScoped<ExceptionFilter>();
            

            #region Services
            //Company
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<ICompanyAddressService, CompanyAddressService>();

            //User
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserProfileService, UserProfileService>();
            services.AddTransient<IUserAddressService, UserAddressService>();

            //AddressType
            services.AddTransient<IAddressTypeService, AddressTypeService>();

            //Role
            services.AddTransient<IDesignationService, DesignationService>();
            services.AddTransient<IDesignationHierarchyService, DesignationHierarchyService>();

            //Profile
            //services.AddTransient<IProfileService, ProfileService>();
            //services.AddTransient<IPermissionService, PermissionService>();
            //services.AddTransient<IProfileUserMappingService, ProfileUserMappingService>();
            //services.AddTransient<IProfilePermissionMappingService, ProfilePermissionMappingService>();

            services.AddTransient<IAuth_RoleService, Auth_RoleService>();
            services.AddTransient<IAuth_RoleUserMappingService, Auth_RoleUserMappingService>();
            services.AddTransient<IAuth_RoleFeatureMappingService, Auth_RoleFeatureMappingService>();



            //Group
            services.AddTransient<IGroupService, GroupService>();
            services.AddTransient<IGroupUserService, GroupUserService>();
            services.AddTransient<IGroupRoleService, GroupRoleService>();
            services.AddTransient<IGroupGrouopsMappingService, GroupGrouopsMappingService>();

            //Log
            services.AddTransient<ICommonService, CommonService>();

            //Addresses
            services.AddTransient<IAddressesService, AddressesService>();

            //Locations
            services.AddTransient<ICompanyLocationService, CompanyLocationService>();

            //CompanyLocationUserMapping
            services.AddTransient<ICompanyLocationUserMappingService, CompanyLocationUserMappingService>();

            //Product
            services.AddTransient<IProductService, ProductService>();


            //BillingAddress
            services.AddTransient<IBillingAddressService, BillingAddressService>();
            //ProductLead
            services.AddTransient<IProductLeadService, ProductLeadService>();
            //CompanyProductMappingService
            services.AddTransient<ICompanyProductMappingService, CompanyProductMappingService>();
            //HostingKeywordService
            services.AddTransient<IHostingKeywordService, HostingKeywordService>();
            #endregion


            #region Repositories
            //Company
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<ICompanyAddressRepository, CompanyAddressRepository>();

            //User
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserProfileRepository, UserProfileRepository>();
            services.AddTransient<IUserAddressRepository, UserAddressRepository>();

            //Address Type
            services.AddTransient<IAddressTypeRepository, AddressTypeRepository>();
            services.AddTransient<IUserProfileRepository, UserProfileRepository>();
            services.AddTransient<IUserAddressRepository, UserAddressRepository>();

            //Role
            services.AddTransient<IDesignationRepository, DesignationRepository>();
            services.AddTransient<IDesignationHierarchyRepository, DesignationHierarchyRepository>();

            ////Profile
            //services.AddTransient<IProfileRepository, ProfileRepository>();
            //services.AddTransient<IPermissionRepository, PermissionRepository>();
            //services.AddTransient<IProfileUserMappingRepository, ProfileUserMappingRepository>();
            //services.AddTransient<IProfilePermissionMappingRepository, ProfilePermissionMappingRepository>();

            services.AddTransient<IAuth_RoleRepository, Auth_RoleRepository>();
            services.AddTransient<IAuth_RoleUserMappingRepository, Auth_RoleUserMappingRepository>();
            services.AddTransient<IAuth_RoleFeatureMappingRepository, Auth_RoleFeatureMappingRepository>();
            services.AddTransient<IAuth_RoleFeatureMappingTempRepository, Auth_RoleFeatureMappingTempRepository>();


            //Group
            services.AddTransient<IGroupRepository, GroupRepository>();
            services.AddTransient<IGroupUserRepository, GroupUserRepository>();
            services.AddTransient<IGroupRoleRepository, GroupRoleRepository>();
            services.AddTransient<IGroupGroupsMappinngRepository, GroupGroupsMappinngRepository>();

            //Log
            services.AddTransient<ILogReposiory, LogRepository>();

            //Api Request Log
            services.AddTransient<IApiRequestLogRepository,ApiRequestLogRepository>();

            //Addresses
            services.AddTransient<IAddressesRepository, AddressesRepository>();

            //Locations
            services.AddTransient<ILocationRepository, LocationRepository>();
            //CompanyLocationUserMapping
            services.AddTransient<ICompanyLocationUserMappingRepository, CompanyLocationUserMappingRepository>();

            //Product
            services.AddTransient<IProductRepository, ProductRepository>();
            //BillingAddress
            services.AddTransient<IBillingAddressRepository, BillingAddressRepository>();//ProductLead
            services.AddTransient<IProductLeadRepository, ProductLeadRepository>();
            //CompanyProductMappingRepository
            services.AddTransient<ICompanyProductMappingRepository, CompanyProductMappingRepository>();

            //HostingKeywordRepository
            services.AddTransient<IHostingKeywordRepository, HostingKeywordRepository>();
            #endregion


            services.Configure<AppSettings>
               (options => Configuration.GetSection(Constants.AppSettings).Bind(options));

            //Implementation for JWT authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidIssuer = Configuration.GetValue<string>(string.Concat(Constants.AppSettings, ":", Constants.JwtSettings.Jwt, ":", Constants.
                    JwtSettings.Issuer)),
                    ValidAudience = Configuration.GetValue<string>(string.Concat(Constants.AppSettings, ":", Constants.JwtSettings.Jwt, ":", Constants.
                    JwtSettings.Audience)),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>(string.Concat(Constants.AppSettings, ":", Constants.JwtSettings.Jwt, ":", Constants.
                    JwtSettings.Key)))),
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline. 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(Configuration.GetValue<string>(string.Concat(Constants.AppSettings, ":", Constants.SwaggerSettings.RootSectionName, ":", Constants.
                    SwaggerSettings.Url)),
                    Configuration.GetValue<string>(string.Concat(Constants.AppSettings, ":", Constants.SwaggerSettings.RootSectionName, ":", Constants.SwaggerSettings.Name)));
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            ConnectionManager.SetConfig(Configuration);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseExceptionHandlerMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
