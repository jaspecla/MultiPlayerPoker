using System;
using System.Collections.Generic;
using System.Text;

namespace MultiPlayerPoker.Game
{
  public class Table
  {
    public LinkedList<Player> Players { get; private set; }
    private LinkedListNode<Player> _currentActivePlayer;
    private LinkedListNode<Player> _playerOnButton;

    public int MaxPlayers { get; private set; }
    public int MinBuyIn { get; private set; }
    public int MaxBuyIn { get; private set; }

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
      // Clean out any players who have left
      foreach (var player in Players)
      {
        if (!player.IsInGame())
        {
          Players.Remove(player);
        }
      }

      // Move the button
      _currentActivePlayer = _playerOnButton;
      NextActivePlayer();
      _playerOnButton = _currentActivePlayer;

      // Get blinds
      NextActivePlayer();
      
    }

    private void OnTrySeatPlayer(Player player)
    {
      if (Players.Count >= MaxPlayers)
      {
        _eventBroker.SendFailSeatPlayer($"This table seats a maximum of {MaxPlayers}.", player);
        return;
      }

      if (player.Bankroll < MinBuyIn)
      {
        _eventBroker.SendFailSeatPlayer($"This table requires a minimum buy-in of {MinBuyIn} and this player only has {player.Bankroll}.", player);
        return;
      }

      if (player.BuyIn < MinBuyIn)
      {
        _eventBroker.SendFailSeatPlayer($"This table requires a minimum buy-in of {MinBuyIn} and this player is only buying in for {player.BuyIn}.", player);
        return;
      }

      if (player.BuyIn > MaxBuyIn)
      {
        _eventBroker.SendFailSeatPlayer($"This table has a maximum buy-in of {MaxBuyIn} and this player is buying in for {player.BuyIn}.", player);
        return;
      }

      Players.AddLast(player);
      _eventBroker.SendPlayerWasSeated(player);

      if (Players.Count == 1)
      {
        _playerOnButton = Players.First;
      }

      if (Players.Count >= 2)
      {
        _eventBroker.SendTableReady();
      }

    }

    private void OnTryLeavePlayer(Player player)
    {
      if (!Players.Contains(player))
      {
        _eventBroker.SendFailPlayerLeft($"Player {player} attempted to leave, but is not at this table.", player);
        return;
      }

      _eventBroker.SendPlayerLeft(player);
    }

    private void NextActivePlayer()
    {
      LinkedListNode<Player> nextActivePlayer = null;
      var current = _currentActivePlayer.Next;
      while (nextActivePlayer == null)
      {
        if (current == null)
        {
          current = Players.First;
        }

        if (current == _currentActivePlayer)
        {
          throw new Exception("There are no other active players at the table.");
          // TODO: Real exception?  How to deal with this?
        }

        if (current.Value.IsInHand())
        {
          nextActivePlayer = current;
        } 
        else
        {
          current = current.Next;
        }
      }

      _currentActivePlayer = nextActivePlayer;

      
    }

  }
}
