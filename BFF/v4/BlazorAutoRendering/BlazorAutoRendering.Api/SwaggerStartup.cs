using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace BlazorAutoRendering.Api
{
    /// <summary>
    /// Swaggergen needs a simpler startup.
    /// Why: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/763
    /// How (this class name is special): https://github.com/domaindrivendev/Swashbuckle.AspNetCore#swashbuckleaspnetcorecli
    /// </summary>
    public class SwaggerWebHostFactory
    {
        public static IWebHost CreateWebHost()
        {
            return WebHost.CreateDefaultBuilder()
                .UseStartup<SwaggerStartup>()
                .Build();
        }
    }

    public class SwaggerStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            Assembly controllers = Assembly.Load("BlazorAutoRendering.Api");

            services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(controllers));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("greetings", new OpenApiInfo
                { // The name must match the final parameter to dotnet tool run swagger tofile in the csproj post build command.
                    Title = $"Duende BFF BlazorAutoRendering example API",
                    Version = "BlazorAutoRendering",
                    Description = $"<p>Duende BFF BlazorAutoRendering example.</p>"
                });

                //var basePath = System.AppContext.BaseDirectory;
                //string docFileName = Directory.GetFiles(basePath, "Duende.BlazorAutoRendering.Api.xml").First();
                //var docFile = Path.Combine(basePath, docFileName);

                //if (File.Exists(docFile))
                //{
                //    options.IncludeXmlComments(docFile, true);
                //}

                // Can't add auth because the authority depends on the environment.
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) { }
    }
}

