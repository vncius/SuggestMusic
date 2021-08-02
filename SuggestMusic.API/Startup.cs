using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SuggestMusic.Configuration;
using SuggestMusic.Infrastructure.Exceptions;
using System;

namespace SuggestMusic.API
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
            DependencyInjectionConfigure.ConfigureDependencias(services);

            services.AddControllers();
            services.AddHttpClient();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Doc - Suggest Music",
                        Version = "v1",
                        Description = "Documentation for API Suggest Music",
                        Contact = new OpenApiContact
                        {
                            Name = "Vinícius Vieira Abreu",
                            Url = new Uri("https://www.linkedin.com/in/vncius/"),
                            Email = "v.vieira.go@gmail.com"
                        }
                    });
            });
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Documentation Suggest Music");
            });

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    await ConfigureExceptionHandling.Configure(context, loggerFactory);
                });
            }
            );

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
