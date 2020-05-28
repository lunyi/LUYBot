using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UGS.Infrastructures.Extensions;
using SkypeBot.Middleware;

namespace SkypeBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env)
        {
            app.UseDefaultFiles()
                .UseBotLogger();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(error =>
                {
                     error.Run(async context =>
                     {
                         context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                         context.Response.ContentType = "application/json";

                         var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                         if (contextFeature != null)
                         {
                             var response = new
                             {
                                 err = 900,
                                 errdesc = contextFeature.Error.GetMessageChain()
                             };
                             
                             await context.Response.WriteAsync(response.ToString());
                         }
                     });
                });
            }
            
            app.UseDefaultFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
                endpoints.MapControllers());
        }
    }
}