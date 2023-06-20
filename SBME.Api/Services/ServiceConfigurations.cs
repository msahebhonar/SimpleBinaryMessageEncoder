using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using SBME.Common;
using SBME.Entities.Builders;
using SBME.Entities.Interfaces;
using SBME.Services;
using SBME.Services.Interfaces;
using Serilog;
using Serilog.Core;

namespace SBME.Api.Services;

public static class ServiceConfigurations
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Simple Binary Message Encoding Schema",
                Version = "1",
                Description = "The Simple Binary Message Encoding Scheme (SBMES) is a method for encoding and decoding messages.",
                Contact = new OpenApiContact
                {
                    Name = "M. Sahebhonar"
                }
            });
        });
    }

    public static void ConfigureServices(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.TryAddScoped<IMessageBuilder, MessageBuilder>();
        services.TryAddScoped<IHeaderCollectionBuilder, HeaderCollectionBuilder>();
        services.TryAddScoped<ISimpleBinaryMessageEncoder, SimpleBinaryMessageEncoder>();
        services.Configure<AppSettings>(configurationManager.GetSection(nameof(AppSettings)));
    }

    public static Logger LoggConfiguration()
    {
        var date = DateTime.Today.Date.ToString("yyyy-dd-MM");
        var logger = new LoggerConfiguration()
            .WriteTo.File($"../logs/log-{date}.txt")
            .CreateLogger();
        return logger;
    }
    
}