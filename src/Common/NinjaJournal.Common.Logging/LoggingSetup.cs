using Microsoft.Extensions.Configuration;
using Serilog.Sinks.Elasticsearch;
using Ardalis.GuardClauses;
using Serilog.Exceptions;
using System.Reflection;
using Serilog;

namespace NinjaJournal.Common.Logging;

public static class LoggingSetup
{
    public static void ConfigureLogging(IConfiguration configuration)
    {
        var numberOfReplicas = configuration.GetValue<int>("ElasticConfiguration:NumberOfReplicas");
        var numberOfShards = configuration.GetValue<int>("ElasticConfiguration:NumberOfShards");
        var elasticSearchUri = configuration["ElasticConfiguration:Uri"];
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        Guard.Against.NullOrEmpty(elasticSearchUri, nameof(elasticSearchUri), $"ElasticSearch URI not found ({elasticSearchUri}");
        Guard.Against.NullOrInvalidInput(numberOfReplicas, numberOfReplicas.GetType().ToString(), n => n > 0, "Number of replicas can't be null or negative");
        Guard.Against.NullOrInvalidInput(numberOfShards, numberOfShards.GetType().ToString(), n => n > 0, "Number of shards can't be null or negative");

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()    
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("Environment", environment)
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearchUri))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name?.ToLower().Replace(".", "-")}-{environment.ToLower()}-{DateTime.UtcNow:yy-MM-dd}",
                NumberOfReplicas = numberOfReplicas,
                NumberOfShards = numberOfShards
            })
            .CreateLogger();
    }
}