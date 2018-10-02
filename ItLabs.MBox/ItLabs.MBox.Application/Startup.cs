using Amazon.S3;
using ItLabs.MBox.Contracts.Dtos;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StructureMap;
using System;
using System.Linq;

namespace ItLabs.MBox.Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services.AddEntityFrameworkNpgsql().AddDbContext<MBoxDbContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString("MBoxAplicationConnection")));


            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<MBoxDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });


            // STRUCTURE MAP CONTAINER
            var container = new Container();

            container.Configure(config =>
            {
                config.AddRegistry(new StructuremapRegistry());
                config.Populate(services);
            });


            var configManager = container.GetInstance<IConfigurationManager>();

            var awsSettings = InitializeAwsSettings(configManager);

            container.Configure(config => { config.For<AwsSettings>().Use(awsSettings); });


            /// AWS SERVICES HERE
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();
            //services.AddAWSService<IAmazonDynamoDB>();



            return container.GetInstance<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public AwsSettings InitializeAwsSettings(IConfigurationManager configManager)
        {

            var allConfig = configManager.GetAll();
            Int32.TryParse(allConfig.FirstOrDefault(x => x.Key == ConfigurationKey.AwsSesPort)?.Value, out int port);

            return new AwsSettings
            {
                AwsSesFromAddress = allConfig.FirstOrDefault(x => x.Key == ConfigurationKey.AwsSesFromAddress)?.Value,
                AwsSesUsername = allConfig.FirstOrDefault(x => x.Key == ConfigurationKey.AwsSesUsername)?.Value,
                AwsSesHost = allConfig.FirstOrDefault(x => x.Key == ConfigurationKey.AwsSesHost)?.Value,
                AwsSesPassword = allConfig.FirstOrDefault(x => x.Key == ConfigurationKey.AwsSesPassword)?.Value,
                AwsSesPort = port,
                ContactFormRecieverMail = allConfig.FirstOrDefault(x => x.Key == ConfigurationKey.ContactFormRecieverMail)?.Value,
                TestRecieverMail = allConfig.FirstOrDefault(x => x.Key == ConfigurationKey.TestReceiverEmail)?.Value,
                AwsS3AccessKey = allConfig.FirstOrDefault(x => x.Key == ConfigurationKey.AwsS3AccessKey)?.Value,
                AwsS3SecretAccessKey = allConfig.FirstOrDefault(x => x.Key == ConfigurationKey.AwsS3SecretAccessKey)?.Value,
                AwsS3BucketName = allConfig.FirstOrDefault(x => x.Key == ConfigurationKey.AwsS3BucketName)?.Value,
            };
        }
    }
}
