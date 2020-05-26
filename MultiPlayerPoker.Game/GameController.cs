using System;
using System.Collections.Generic;
using System.Text;

namespace MultiPlayerPoker.Game
{
  public class GameController
  {
    private GameProperties _properties;
    private Game _game;
    private Table _table;

    internal GameController(GameProperties properties, Game game, Table table)
    {
      _properties = properties;
      _game = game;
      _table = table;
    }

    public void AddLogOutput(IGameLogOutput logOutput)
    {
      if (logOutput != null)
      {
        _properties.EventBroker.LogOutputs.Add(logOutput);
        _properties.Actions.LogOutputs.Add(logOutput);
      }
    }

    public bool JoinGame(Player player)
    {
      return _properties.Actions.TrySeatPlayer(player);
    }

    public bool LeaveGame(Player player)
    {
      return _properties.Actions.TryLeavePlayer(player);
    }

    public bool SitIn(Player player)
    {
      return _properties.Actions.TryUnpausePlayer(player);
    }

    public bool SitOut(Player player)
    {
      return _properties.Actions.TryPausePlayer(player);
    }

    public bool Bet(Player player, int amount)
    {
      return _properties.Actions.TryPlayerBet(player, amount);
    }

    public bool Fold(Player player)
    {
      return _properties.Actions.TryPlayerFold(player);
    }

  }
}
