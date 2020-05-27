using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.IO;

namespace akvwebapp
{
    public class Startup
    {

        string vaultUri = "";
        string dbCredentials = "";
        // flag to check if running locally in a container (not in k8s)
        bool standAloneContainer = true;
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 // To enable it to read it from configmap in k8s, updated the path.  
                 // In the k8s, the appsettings.json will be placed inside config folder.                 
                 .AddJsonFile("config/appsettings.json", optional: true, reloadOnChange: true);

            var config = builder.Build();

            var appConfig = config.GetSection("EnvironmentConfig").Get<EnvironmentConfig>();
            // if running locally in a container which is not in k8s & no configmaps configured
            if (appConfig != null)
            {
                vaultUri = appConfig.VaultUri;
                dbCredentials = appConfig.DBCredentials;
                standAloneContainer =  false;
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();            

            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),                    
                    // Mode = RetryMode.Exponential,
                    MaxRetries = 5
                }
            };

            string secretValue = null;
            try
            {
                // If running in kubernetes
                if (standAloneContainer == false)
                {
                    var client = new SecretClient(new Uri(vaultUri), new DefaultAzureCredential(),options);
                    KeyVaultSecret secret = client.GetSecret(dbCredentials);
                    secretValue = secret.Value;
                }
                else
                {
                    secretValue = "The app running locally on a container cannot access the appsettings.json inside config folder which is created by k8s configmap";
                }

            }
            catch (Exception ex)
            {                                
                secretValue = "Cannot access key vault. " + Environment.NewLine + ex.Message;
            }

            app.UseEndpoints(endpoints =>
            {
                // Set up the response for base path
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World from a .Net core web app!!!");
                });

                // Set up the response to demonstrate Azure Key Vault
                endpoints.MapGet("/keyvault", async context =>
                {                    
                    await context.Response.WriteAsync(secretValue);
                });
            });
        }
    }
}
