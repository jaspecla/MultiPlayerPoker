using MultiPlayerPoker.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultiPlayerPoker.Game
{
  internal class Table
  {
    internal PlayerCollection Players { get; private set; }
    internal PotCollection Pots { get; private set; }

    private GameProperties _properties;
    private GameEventBroker _eventBroker;
    private GameActions _actions;

    private Dealer _dealer;

    internal Table(GameProperties properties)
    {
      _properties = properties;

      _eventBroker = properties.EventBroker;
      _eventBroker.GameReady += OnGameReady;
      _eventBroker.PreFlopReady += OnPreFlopReady;
      _eventBroker.FlopReady += OnFlopReady;
      _eventBroker.TurnReady += OnTurnOrRiverReady;
      _eventBroker.RiverReady += OnTurnOrRiverReady;
      _eventBroker.PlayerDidBet += OnPlayerBet;
      _eventBroker.PlayerDidFold += OnPlayerFold;

      _actions = properties.Actions;
      _actions.TrySeatPlayerDelegate += OnTrySeatPlayer;
      _actions.TryLeavePlayerDelegate += OnTryLeavePlayer;

      Players = new PlayerCollection(properties);
      _dealer = new Dealer();
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
      _actions.TryPlayerBlind(Players.CurrentActivePlayer, _properties.SmallBlind, "small");

      Players.NextActivePlayer();
      _actions.TryPlayerBlind(Players.CurrentActivePlayer, _properties.BigBlind, "big");

      Players.NextActivePlayer();
      _eventBroker.SendActionOnPlayer(Players.CurrentActivePlayer);

      
    }

    private void OnPreFlopReady(object sender, GameEventArgs args)
    {
      // Deal the cards
      _dealer.Shuffle();

      foreach (var player in Players)
      {
        _eventBroker.SendPlayerCardDelt(player, _dealer.NextCard());
      }
    }

    private void OnFlopReady(object sender, GameEventArgs args)
    {
      Card[] flop = new Card[3];
      flop[0] = _dealer.NextCard();
      flop[1] = _dealer.NextCard();
      flop[2] = _dealer.NextCard();

      _eventBroker.SendCommunityCardsDealt(flop);
    }

    private void OnTurnOrRiverReady(object sender, GameEventArgs args)
    {
      Card[] cards = new Card[1];
      cards[0] = _dealer.NextCard();

      _eventBroker.SendCommunityCardsDealt(cards);
    }

    private bool OnTrySeatPlayer(Player player)
    {
      if (Players.Count() >= _properties.MaxPlayers)
      {
        _eventBroker.SendFailSeatPlayer($"This table seats a maximum of {_properties.MaxPlayers}.", player);
        return false;
      }

      if (player.Bankroll < _properties.MinBuyIn)
      {
        _eventBroker.SendFailSeatPlayer($"This table requires a minimum buy-in of {_properties.MinBuyIn} and this player only has {player.Bankroll}.", player);
        return false;
      }

      if (player.BuyIn < _properties.MinBuyIn)
      {
        _eventBroker.SendFailSeatPlayer($"This table requires a minimum buy-in of {_properties.MinBuyIn} and this player is only buying in for {player.BuyIn}.", player);
        return false;
      }

      if (player.BuyIn > _properties.MaxBuyIn)
      {
        _eventBroker.SendFailSeatPlayer($"This table has a maximum buy-in of {_properties.MaxBuyIn} and this player is buying in for {player.BuyIn}.", player);
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
