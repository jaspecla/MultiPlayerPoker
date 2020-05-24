using MultiPlayerPoker.Cards;
using Stateless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiPlayerPoker.Game
{
  public class Player
  {

    private enum State
    {
      NotInGame,
      AtTable,
      AwaitingHand,
      SittingOut,
      InHand,
      Action,
      AllIn,
      OutOfHand
    }

    private enum Trigger
    {
      SeatedAtTable,
      LeaveTable,
      SitOut,
      SitIn,
      HandDealt,
      ActionOn,
      Bet,
      Fold,
      HandComplete
    }

    private readonly StateMachine<State, Trigger> _playerStateMachine;

    private readonly StateMachine<State, Trigger>.TriggerWithParameters<int> _betTrigger;

    private readonly GameEventBroker _eventBroker;
    private readonly GameActions _actions;

    public string Id { get; set; }
    public string DisplayName { get; set; }
    public Card[] HoleCards { get; set; }
    public int ChipStack { get; set; }
    public int CurrentBet { get; set; }
    public int Bankroll { get; set; }
    public int BuyIn { get; set; }

    public Player(GameEventBroker eventBroker, GameActions actions)
    {
      HoleCards = new Card[2];

      _eventBroker = eventBroker;
      _eventBroker.PlayerWasSeated += OnPlayerSeated;
      _eventBroker.PlayerLeft += OnPlayerLeft;
      _eventBroker.ActionOnPlayer += OnActionOnPlayer;
      _eventBroker.PlayerCardDealt += OnPlayerDealtCard;

      _actions = actions;

      _actions.TryPausePlayerDelegate += OnTryPausePlayer;
      _actions.TryUnpausePlayerDelegate += OnTryUnpausePlayer;
      _actions.TryPlayerBetDelegate += OnTryPlayerBet;
      _actions.TryPlayerFoldDelegate += OnTryPlayerFold;

      _playerStateMachine = new StateMachine<State, Trigger>(State.NotInGame);

      _playerStateMachine.Configure(State.NotInGame)
        .Permit(Trigger.SeatedAtTable, State.AwaitingHand);

      _playerStateMachine.Configure(State.AwaitingHand)
        .Permit(Trigger.SitOut, State.SittingOut)
        .Permit(Trigger.HandDealt, State.InHand)
        .Permit(Trigger.LeaveTable, State.NotInGame);

      _playerStateMachine.Configure(State.SittingOut)
        .Permit(Trigger.SitIn, State.AwaitingHand)
        .Permit(Trigger.LeaveTable, State.NotInGame);

      _playerStateMachine.Configure(State.InHand)
        .Permit(Trigger.ActionOn, State.Action)
        .Permit(Trigger.LeaveTable, State.NotInGame);

      _playerStateMachine.Configure(State.Action)
        .PermitDynamic(_betTrigger, bet =>
        {
          if (ChipStack == 0)
          {
            return State.AllIn;
          }
          else
          {
            return State.InHand;
          }
        })
        .Permit(Trigger.Fold, State.AwaitingHand)
        .Permit(Trigger.LeaveTable, State.NotInGame);

      _playerStateMachine.Configure(State.AllIn)
        .Permit(Trigger.HandComplete, State.AwaitingHand);


    }

    private void OnPlayerSeated(object sender, GameEventArgs args)
    {
      if (args.Player != this)
      {
        return;
      }

      Bankroll -= BuyIn;
      ChipStack = BuyIn;

      _playerStateMachine.Fire(Trigger.SeatedAtTable);
    }

    private void OnPlayerLeft(object sender, GameEventArgs args)
    {
      if (args.Player != this)
      {
        return;
      }

      _playerStateMachine.Fire(Trigger.LeaveTable);
    }

    private void OnTryPausePlayer(Player player)
    {
      if (player != this)
      {
        return;
      }

      if (!_playerStateMachine.CanFire(Trigger.SitOut))
      {
        _eventBroker.SendFailPausePlayer("Player cannot currently sit out the game.", player);
        return;
      }

      _playerStateMachine.Fire(Trigger.SitOut);
      _eventBroker.SendPlayerWasPaused(player);

    }

    private void OnTryUnpausePlayer(Player player) {
      if (player != this)
      {
        return;
      }

      if (!_playerStateMachine.CanFire(Trigger.SitIn))
      {
        _eventBroker.SendFailUnpausePlayer("Player cannot currently return to the game.", player);
        return;
      }

      _playerStateMachine.Fire(Trigger.SitIn);
      _eventBroker.SendPlayerWasUnpaused(player);
    }

    private void OnTryPlayerBet(Player player, int amount)
    {
      if (player != this)
      {
        return;
      }

      if (!_playerStateMachine.IsInState(State.Action))
      {
        _eventBroker.SendFailPlayerBet($"Player {player.DisplayName} tried to bet, but the action was not on them.", player);
        return;
      }

      if (amount > ChipStack)
      {
        _eventBroker.SendFailPlayerBet($"Player {player.DisplayName} tried to bet {amount} but they only have {ChipStack} in their stack.", player);
        return;
      }

      ChipStack -= amount;
      _playerStateMachine.Fire(_betTrigger, amount);
      _eventBroker.SendPlayerDidBet(player, amount);

    }

    private void OnTryPlayerFold(Player player)
    {
      if (player != this)
      {
        return;
      }

      if (!_playerStateMachine.IsInState(State.Action))
      {
        _eventBroker.SendFailPlayerBet($"Player {player.DisplayName} tried to fold, but the action was not on them.", player);
        return;
      }

      _playerStateMachine.Fire(Trigger.Fold);
      _eventBroker.SendPlayerDidFold(player);
    }

    private void OnActionOnPlayer(object sender, GameEventArgs args)
    {
      if (args.Player != this)
      {
        return;
      }

      _playerStateMachine.Fire(Trigger.ActionOn);
    }

    private void OnPlayerDealtCard(object sender, GameEventArgs args)
    {
      if (args.Player != this)
      {
        return;
      }

      this.HoleCards.Append(args.Cards[0]);
    }

    public bool IsInHand()
    {
      return _playerStateMachine.IsInState(State.InHand) || _playerStateMachine.IsInState(State.Action);
    }

    public bool IsInGame()
    {
      return !_playerStateMachine.IsInState(State.NotInGame);
    }
  }
}
