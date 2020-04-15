using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(MultiPlayerPoker.Functions.Startup))]
namespace MultiPlayerPoker.Functions
{
  public class Startup : FunctionsStartup
  {
    public override void Configure(IFunctionsHostBuilder builder)
    {
      var connectionString = System.Environment.GetEnvironmentVariable("CosmosDbConnection", EnvironmentVariableTarget.Process);

      builder.Services.AddSingleton((s) =>
      {
        var clientOptions = new CosmosClientOptions()
        {
          SerializerOptions = new CosmosSerializationOptions()
          {
            PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
          }
        };
        return new CosmosClient(connectionString, clientOptions);
      });

    }
  }
}
