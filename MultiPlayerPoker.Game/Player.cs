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

    public void BuyInPlayer()
    {
      Bankroll -= BuyIn;
      ChipStack = BuyIn;

      _playerStateMachine.Fire(Trigger.SeatedAtTable);
    }

    public void LeaveTable()
    {
      _playerStateMachine.Fire(Trigger.LeaveTable);
    }

    public bool CanPause()
    {
      return _playerStateMachine.CanFire(Trigger.SitOut);
    }

    public void Pause()
    {
      _playerStateMachine.Fire(Trigger.SitOut);
    }

    public bool CanUnpause()
    {
      return _playerStateMachine.CanFire(Trigger.SitIn);
    }

    public void Unpause()
    {
      _playerStateMachine.Fire(Trigger.SitIn);
    }

    public void Bet(int amount)
    {
      ChipStack -= amount;
      CurrentBet += amount;
      _playerStateMachine.Fire(_betTrigger, amount);
    }

    public void Blind(int amount)
    {
      ChipStack -= amount;
      CurrentBet += amount;
    }

    public void Fold()
    {
      _playerStateMachine.Fire(Trigger.Fold);
    }

    public void ActionOn()
    {
      _playerStateMachine.Fire(Trigger.ActionOn);
    }

    public void DealCard(Card card)
    {
      HoleCards.Append(card);
    }

    public void AwardMoney(int amount)
    {
      ChipStack += amount;
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
