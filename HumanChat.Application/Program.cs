using System.Reflection;
using System.Text.Json.Serialization;
using HumanChat.Application.Infrastructure;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

const string applicationName = "HumanChat";

var host = new HostBuilder().ConfigureFunctionsWebApplication().ConfigureServices((context, services) =>
{
    var assembly = Assembly.GetExecutingAssembly();

    services.Configure<JsonOptions>(options =>
    {
        options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.SerializerOptions.PropertyNameCaseInsensitive = true;
    });

    services.AddInfrastructure(context.Configuration, assembly, context.HostingEnvironment.IsDevelopment());
    
    if (context.HostingEnvironment.IsDevelopment() || context.HostingEnvironment.IsEnvironment("local"))
        services.AddDevelopmentCorsPolicy();
}).Build();

host.Run();