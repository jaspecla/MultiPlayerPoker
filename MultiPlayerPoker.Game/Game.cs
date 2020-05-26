using MultiPlayerPoker.Cards;
using Stateless;
using Stateless.Graph;
using System;

namespace MultiPlayerPoker.Game
{

  internal class Game
  {
    private enum State
    {
      NewGame,
      HandInProgress,
      PreFlop,
      Flop,
      Turn,
      River,
      HandComplete
    };

    private enum Trigger
    {
      PlayerSeated,
      PlayerLeft,
      TableReady,
      GameReady,
      PlayerBet,
      PlayerFold,
      BettingComplete,
      ReadyForNewHand
    };

    private readonly GameEventBroker _eventBroker;
    private readonly StateMachine<State, Trigger> _gameStateMachine;

    internal Game(GameProperties properties)
    {
      _gameStateMachine = new StateMachine<State, Trigger>(State.NewGame);

      _eventBroker = properties.EventBroker;
      _eventBroker.TableReady += OnTableReady;
      _eventBroker.BettingCompleted += OnBettingComplete;

      _gameStateMachine.Configure(State.NewGame)
        .Permit(Trigger.TableReady, State.HandInProgress);

      _gameStateMachine.Configure(State.HandInProgress)
        .OnEntry(() => BeginGame())
        .Permit(Trigger.GameReady, State.PreFlop);

      _gameStateMachine.Configure(State.PreFlop)
        .OnEntry(() => PreFlopReady())
        .SubstateOf(State.HandInProgress)
        .Permit(Trigger.BettingComplete, State.Flop);

      _gameStateMachine.Configure(State.Flop)
        .OnEntry(() => FlopReady())
        .SubstateOf(State.HandInProgress)
        .Permit(Trigger.BettingComplete, State.Turn);

      _gameStateMachine.Configure(State.Turn)
        .OnEntry(() => TurnReady())
        .SubstateOf(State.HandInProgress)
        .Permit(Trigger.BettingComplete, State.River);

      _gameStateMachine.Configure(State.River)
        .OnEntry(() => RiverReady())
        .SubstateOf(State.HandInProgress)
        .Permit(Trigger.BettingComplete, State.HandComplete)
        .OnExit(t => Showdown());

      _gameStateMachine.Configure(State.HandComplete)
        .Permit(Trigger.ReadyForNewHand, State.HandInProgress);
    }

    private void BeginGame()
    {
      _eventBroker.SendGameReady();
    }


    private void OnTableReady(object sender, GameEventArgs args)
    {
      _gameStateMachine.Fire(Trigger.TableReady);
    }

    private void PreFlopReady()
    {
      _eventBroker.SendPreFlopReady();
    }

    private void FlopReady()
    {
      _eventBroker.SendFlopReady();
    }

    private void TurnReady()
    {
      _eventBroker.SendTurnReady();
    }

    private void RiverReady()
    {
      _eventBroker.SendRiverReady();
    }

    private void OnBettingComplete(object sender, GameEventArgs e)
    {
      _gameStateMachine.Fire(Trigger.BettingComplete);
    }

    private void Showdown()
    {
      _eventBroker.SendShowdown();
    }

  }
}
