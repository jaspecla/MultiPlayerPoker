using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiPlayerPoker.Game
{
  public class PlayerCollection : IEnumerable<Player>
  {
    private LinkedList<Player> _players;
    private LinkedListNode<Player> _currentActivePlayer;
    private LinkedListNode<Player> _playerOnButton;

   public int AmountToGo { get; set; }

    private readonly GameEventBroker _eventBroker;
    private readonly GameActions _actions;

    public Player CurrentActivePlayer
    {
      get
      {
        return _currentActivePlayer.Value;
      }
    }

    public Player PlayerOnButton
    {
      get
      {
        return _playerOnButton.Value;
      }
    }

    public PlayerCollection(GameEventBroker eventBroker, GameActions actions)
    {
      _players = new LinkedList<Player>();

      _eventBroker = eventBroker;
      _eventBroker.PlayerWasSeated += OnPlayerSeated;
      _eventBroker.PlayerLeft += OnPlayerLeft;
      _eventBroker.ActionOnPlayer += OnActionOnPlayer;
      _eventBroker.PlayerCardDealt += OnPlayerDealtCard;
      _eventBroker.PlayerWonMoney += OnPlayerWonMoney;
      _eventBroker.PlayerDidBet += OnPlayerAddedMoney;
      _eventBroker.PlayerDidBlind += OnPlayerAddedMoney;

      _actions = actions;

      _actions.TryPausePlayerDelegate += OnTryPausePlayer;
      _actions.TryUnpausePlayerDelegate += OnTryUnpausePlayer;
      _actions.TryPlayerBetDelegate += OnTryPlayerBet;
      _actions.TryPlayerFoldDelegate += OnTryPlayerFold;
      _actions.TryPlayerBlindDelegate += OnTryPlayerBlind;

    }

    public void SeatPlayer(Player player)
    {
      _players.AddLast(player);
      if (_players.Count == 1)
      {
        _currentActivePlayer = _players.First;
        _playerOnButton = _players.First;
      }
    }

    private void OnPlayerSeated(object sender, GameEventArgs args)
    {
      var player = _players.Find(args.Player);
      if (player != null)
      {
        player.Value.BuyInPlayer();
      }
    }

    private void OnPlayerLeft(object sender, GameEventArgs args)
    {
      var player = _players.Find(args.Player);
      if (player != null)
      {
        player.Value.LeaveTable();
      }
    }

    private bool OnTryPausePlayer(Player player)
    {
      var playerToPause = _players.Find(player);

      if (playerToPause == null)
      {
        _eventBroker.SendFailPausePlayer($"Player {player.DisplayName} is not at this table.", player);
        return false;
      }

      if (!playerToPause.Value.CanPause())
      { 
        _eventBroker.SendFailPausePlayer($"Player {player.DisplayName} cannot currently sit out the game.", player);
        return false;
      }

      playerToPause.Value.Pause();

      _eventBroker.SendPlayerWasPaused(player);

      return true;
    }

    private bool OnTryUnpausePlayer(Player player)
    {
      var playerToUnpause = _players.Find(player);

      if (playerToUnpause == null)
      {
        _eventBroker.SendFailUnpausePlayer($"Player {player.DisplayName} is not at this table.", player);
        return false;
      }

      if (!playerToUnpause.Value.CanUnpause())
      {
        _eventBroker.SendFailPausePlayer($"Player {player.DisplayName} cannot currently resume the game.", player);
        return false;
      }

      playerToUnpause.Value.Unpause();

      _eventBroker.SendPlayerWasUnpaused(player);

      return true;
    }

    private bool OnTryPlayerBet(Player player, int amount)
    {
      var playerToBet = _players.Find(player);

      if (playerToBet == null)
      {
        _eventBroker.SendFailPlayerBet($"Player {player.DisplayName} tried to bet, but was not seated at this table.", player);
        return false;
      }

      if (playerToBet.Value != CurrentActivePlayer)
      {
        _eventBroker.SendFailPlayerBet($"Player {player.DisplayName} tried to bet, but the action was not on them.", player);
        return false;
      }

      if (amount > playerToBet.Value.ChipStack)
      {
        _eventBroker.SendFailPlayerBet($"Player {player.DisplayName} tried to bet {amount} but they only have {player.ChipStack} in their stack.", player);
        return false;
      }

      playerToBet.Value.Bet(amount);

      _eventBroker.SendPlayerDidBet(player, amount);

      return true;

    }

    private bool OnTryPlayerFold(Player player)
    {
      var playerToFold = _players.Find(player);
      if (playerToFold == null)
      {
        _eventBroker.SendFailPlayerFold($"Player {player.DisplayName} tried to fold but they are not seated at this table.", player);

      }

      if (playerToFold.Value != CurrentActivePlayer)
      {
        _eventBroker.SendFailPlayerFold($"Player {player.DisplayName} tried to fold, but the action was not on them.", player);
        return false;
      }

      playerToFold.Value.Fold();
      _eventBroker.SendPlayerDidFold(player);
      return true;
    }

    private void OnPlayerAddedMoney(object sender, GameEventArgs args)
    {
      if (args.Amount > AmountToGo)
      {
        AmountToGo = args.Amount;
      }
    }

    private void OnActionOnPlayer(object sender, GameEventArgs args)
    {
      var playerToActionOn = _players.Find(args.Player);

      if (playerToActionOn == null)
      {
        return;
      }

      // Betting has completed
      if (playerToActionOn.Value.CurrentBet == AmountToGo)
      {
        // Reset current bets
        foreach (var player in _players)
        {
          player.CurrentBet = 0;
        }
        _eventBroker.SendBettingCompleted();
        return;
      }

      playerToActionOn.Value.ActionOn();
    }

    private bool OnTryPlayerBlind(Player player, int amount, string blindType)
    {
      var playerToBlind = _players.Find(player);
      
      if (playerToBlind == null)
      {
        _eventBroker.SendFailPlayerBlind($"Player {player.DisplayName} tried to blind, but is not seated at this table.", player);
      }

      if (amount > playerToBlind.Value.ChipStack)
      {
        _eventBroker.SendFailPlayerBlind($"Player needs {amount} to meet the {blindType} blind but only has {playerToBlind.Value.ChipStack} in their chip stack.", player);
        return false;
      }

      // TODO: something with state if they go all in?

      playerToBlind.Value.Blind(amount);
      _eventBroker.SendPlayerDidBlind(player, amount);
      return true;

    }

    private void OnPlayerDealtCard(object sender, GameEventArgs args)
    {
      var playerToDealTo = _players.Find(args.Player);

      if (playerToDealTo == null)
      {
        return;
      }

      playerToDealTo.Value.DealCard(args.Cards[0]);
    }

    private void OnPlayerWonMoney(object sender, GameEventArgs args)
    {
      var winningPlayer = _players.Find(args.Player);

      if (winningPlayer == null)
      {
        return;
      }

      winningPlayer.Value.AwardMoney(args.Amount);
    }


    public void RemovePlayer(Player player)
    {
      _players.Remove(player);
    }

    public void RemovePlayersNotInGame()
    {
      // Clean out any players who have left
      foreach (var player in _players)
      {
        if (!player.IsInGame())
        {
          _players.Remove(player);
        }
      }

    }

    public void MoveButton()
    {
      _currentActivePlayer = _playerOnButton;
      NextActivePlayer();
      _playerOnButton = _currentActivePlayer;
    }

    public void NextActivePlayer()
    {
      LinkedListNode<Player> nextActivePlayer = null;
      var current = _currentActivePlayer.Next;
      while (nextActivePlayer == null)
      {
        if (current == null)
        {
          current = _players.First;
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

    public IEnumerator<Player> GetEnumerator()
    {
      return _players.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return _players.GetEnumerator();
    }
  }


}
