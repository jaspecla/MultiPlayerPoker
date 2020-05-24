using MultiPlayerPoker.Cards;
using Stateless;
using System;
using System.Collections.Generic;
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

    public string Id { get; set; }
    public string DisplayName { get; set; }
    public Card[] HoleCards { get; set; }
    public int ChipStack { get; set; }
    public int CurrentBet { get; set; }
    public int Bankroll { get; set; }
    public int BuyIn { get; set; }

    public Player()
    {
      HoleCards = new Card[2];

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
          if (bet == ChipStack)
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

    public void Seat()
    {
      _playerStateMachine.Fire(Trigger.SeatedAtTable);
    }

    public void RemoveFromGame()
    {
      _playerStateMachine.Fire(Trigger.LeaveTable);
    }

    public bool IsInHand()
    {
      return _playerStateMachine.IsInState(State.InHand) || _playerStateMachine.IsInState(State.Action);
    }

    public void Bet(int amount)
    {
      _playerStateMachine.Fire(_betTrigger, amount);
    }

    public void Fold()
    {
      _playerStateMachine.Fire(Trigger.Fold);
    }
  }
}
