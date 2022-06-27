using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogAnalyzerLibraryCommon.Swagger
{
    public static class Extension
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection services,
            IConfiguration configuration)
        {
            SwaggerOptions options = new SwaggerOptions();
            options.Title = configuration["Swagger:Title"];
            options.Version = configuration["Swagger:Version"];
            options.Build = configuration["BuildNumber"];

            services.AddSwaggerGen(c =>

                c.SwaggerDoc(
                    options.Version,
                    new OpenApiInfo
                    {
                        Title = $"{options.Title} API",
                        Version = $"{options.Version}",
                        Description = $"{options.Build}"
                    }
                ));
            //services.AddSwaggerGenNewtonsoftSupport();

            return services;
        }

        public static IApplicationBuilder UseSwaggerService(this IApplicationBuilder builder,
            IConfiguration configuration)
        {
            SwaggerOptions options = new SwaggerOptions();
            options.Title = configuration["Swagger:Title"];
            options.Version = configuration["Swagger:Version"];

            builder.UseSwagger(c => { c.RouteTemplate = "/_swagger/{documentName}/swagger.json"; });
            builder.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "_swagger";
                c.SwaggerEndpoint($"/_swagger/{options.Version}/swagger.json",
                    $"{options.Title} API {options.Version}");
            });
            return builder;
        }
    }
}
