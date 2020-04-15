using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayerPoker.Functions.Data
{
  public class CosmosGameDb : IGameDb
  {

    private CosmosClient _client;
    private Container _gamesContainer;
    private ILogger<CosmosGameDb> _log;

    public CosmosGameDb(CosmosClient cosmosClient, ILogger<CosmosGameDb> logger)
    {
      _client = cosmosClient;
      _log = logger;

      var pokerDatabaseName = "Poker";
      var gamesContainerName = "Games";

      _gamesContainer = _client.GetContainer(pokerDatabaseName, gamesContainerName);
    }

    public async Task CreateGame(NewGame newGame)
    {

      var createResponse = await _gamesContainer.CreateItemAsync(newGame, new PartitionKey(newGame.Id));

      _log.LogInformation($"Upserted site to db with cost {createResponse.RequestCharge}");
    }

  }
}
