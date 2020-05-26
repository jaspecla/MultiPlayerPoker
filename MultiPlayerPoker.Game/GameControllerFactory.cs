using System;
using System.Collections.Generic;
using System.Text;

namespace MultiPlayerPoker.Game
{
  public static class GameControllerFactory
  {
    public static GameController Create(List<IGameLogOutput> logOutputs, int maxPlayers, int minBuyIn, int maxBuyIn, int bigBlind, int smallBlind)
    {
      var properties = new GameProperties
      {
        MaxPlayers = maxPlayers,
        MinBuyIn = minBuyIn,
        MaxBuyIn = maxBuyIn,
        BigBlind = bigBlind,
        SmallBlind = smallBlind
      };

      var eventBroker = new GameEventBroker();
      var actions = new GameActions();

      if (logOutputs != null && logOutputs.Count > 0)
      {
        eventBroker.LogOutputs.AddRange(logOutputs);
        actions.LogOutputs.AddRange(logOutputs);
      }

      properties.EventBroker = eventBroker;
      properties.Actions = actions;

      var game = new Game(properties);
      var table = new Table(properties);

      var gameController = new GameController(properties, game, table);

      return gameController;
    }


  }
}
