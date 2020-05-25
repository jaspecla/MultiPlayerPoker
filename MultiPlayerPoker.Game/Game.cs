using MultiPlayerPoker.Cards;
using Stateless;
using Stateless.Graph;
using System;

namespace MultiPlayerPoker.Game
{

  public class Game
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

    private readonly StateMachine<State, Trigger>.TriggerWithParameters<Player> _playerSeatedTrigger;
    private readonly StateMachine<State, Trigger>.TriggerWithParameters<Player> _playerLeftTrigger;
    private readonly StateMachine<State, Trigger>.TriggerWithParameters<Player, int> _betTrigger;
    private readonly StateMachine<State, Trigger>.TriggerWithParameters<Player> _foldTrigger;

    public Game(GameEventBroker eventBroker)
    {
      _gameStateMachine = new StateMachine<State, Trigger>(State.NewGame);

      _eventBroker = eventBroker;
      _eventBroker.TableReady += OnTableReady;
      _eventBroker.BettingCompleted += OnBettingComplete;

      _gameStateMachine.Configure(State.NewGame)
        .Permit(Trigger.TableReady, State.HandInProgress);

      _gameStateMachine.Configure(State.HandInProgress)
        .OnEntry(() => BeginGame())
        .Permit(Trigger.GameReady, State.PreFlop);

      _gameStateMachine.Configure(State.PreFlop)
        .SubstateOf(State.HandInProgress)
        .Permit(Trigger.BettingComplete, State.Flop);

      _gameStateMachine.Configure(State.Flop)
        .SubstateOf(State.HandInProgress)
        .Permit(Trigger.BettingComplete, State.Turn);

      _gameStateMachine.Configure(State.Turn)
        .SubstateOf(State.HandInProgress)
        .Permit(Trigger.BettingComplete, State.River);

      _gameStateMachine.Configure(State.River)
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
