using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultiPlayerPoker.Game
{
  public class Table
  {

    public PlayerCollection Players { get; private set; }
    public PotCollection Pots { get; private set; }

    public int MaxPlayers { get; private set; }
    public int MinBuyIn { get; private set; }
    public int MaxBuyIn { get; private set; }
    public int BigBlind { get; private set; }
    public int SmallBlind { get; private set; }

    private GameEventBroker _eventBroker;
    private GameActions _actions { get; set; }

    public Table(GameEventBroker eventBroker, GameActions actions, int maxPlayers, int minBuyIn, int maxBuyIn)
    {
      _eventBroker = eventBroker;
      _eventBroker.GameReady += OnGameReady;
      _eventBroker.PlayerDidBet += OnPlayerBet;
      _eventBroker.PlayerDidFold += OnPlayerFold;

      _actions = actions;
      _actions.TrySeatPlayerDelegate += OnTrySeatPlayer;
      _actions.TryLeavePlayerDelegate += OnTryLeavePlayer;

      MaxPlayers = maxPlayers;
      MinBuyIn = minBuyIn;
      MaxBuyIn = maxBuyIn;
    }

    private void OnGameReady(object sender, GameEventArgs args)
    {

      Players.RemovePlayersNotInGame();

      // Reset pot collection
      Pots = new PotCollection(Players.ToList(), _eventBroker);

      // Move the button
      Players.MoveButton();

      // Get blinds
      // TODO: What if they don't have enough to make blinds?
      Players.NextActivePlayer();
      _actions.TryPlayerBlind(Players.CurrentActivePlayer, SmallBlind, "small");

      Players.NextActivePlayer();
      _actions.TryPlayerBlind(Players.CurrentActivePlayer, BigBlind, "big");

      Players.NextActivePlayer();
      _eventBroker.SendActionOnPlayer(Players.CurrentActivePlayer);
      
    }

    private bool OnTrySeatPlayer(Player player)
    {
      if (Players.Count() >= MaxPlayers)
      {
        _eventBroker.SendFailSeatPlayer($"This table seats a maximum of {MaxPlayers}.", player);
        return false;
      }

      if (player.Bankroll < MinBuyIn)
      {
        _eventBroker.SendFailSeatPlayer($"This table requires a minimum buy-in of {MinBuyIn} and this player only has {player.Bankroll}.", player);
        return false;
      }

      if (player.BuyIn < MinBuyIn)
      {
        _eventBroker.SendFailSeatPlayer($"This table requires a minimum buy-in of {MinBuyIn} and this player is only buying in for {player.BuyIn}.", player);
        return false;
      }

      if (player.BuyIn > MaxBuyIn)
      {
        _eventBroker.SendFailSeatPlayer($"This table has a maximum buy-in of {MaxBuyIn} and this player is buying in for {player.BuyIn}.", player);
        return false;
      }

      Players.SeatPlayer(player);
      _eventBroker.SendPlayerWasSeated(player);

      if (Players.Count() >= 2)
      {
        _eventBroker.SendTableReady();
      }

      return true;

    }

    private bool OnTryLeavePlayer(Player player)
    {
      if (!Players.Contains(player))
      {
        _eventBroker.SendFailPlayerLeft($"Player {player} attempted to leave, but is not at this table.", player);
        return false;
      }

      _eventBroker.SendPlayerLeft(player);
      return true;
    }

    private void OnPlayerBet(object sender, GameEventArgs args)
    {
      Players.NextActivePlayer();
      _eventBroker.SendActionOnPlayer(Players.CurrentActivePlayer);
    }

    private void OnPlayerFold(object sender, GameEventArgs args)
    {
      Players.NextActivePlayer();
      _eventBroker.SendActionOnPlayer(Players.CurrentActivePlayer);
    }


  }
}
