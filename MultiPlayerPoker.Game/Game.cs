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
    private readonly GameState _gameState;

    private readonly StateMachine<State, Trigger>.TriggerWithParameters<Player> _playerSeatedTrigger;
    private readonly StateMachine<State, Trigger>.TriggerWithParameters<Player> _playerLeftTrigger;
    private readonly StateMachine<State, Trigger>.TriggerWithParameters<Player, int> _betTrigger;
    private readonly StateMachine<State, Trigger>.TriggerWithParameters<Player> _foldTrigger;

    public Game(GameEventBroker eventBroker)
    {
      _gameStateMachine = new StateMachine<State, Trigger>(State.NewGame);
      _gameState = new GameState();

      _eventBroker = eventBroker;
      _eventBroker.PlayerSeated += SeatPlayer;

      _betTrigger = _gameStateMachine.SetTriggerParameters<Player, int>(Trigger.PlayerBet);
      _foldTrigger = _gameStateMachine.SetTriggerParameters<Player>(Trigger.PlayerFold);

      _gameStateMachine.Configure(State.NewGame)
        .OnEntry(() => InitializeGame())
        .InternalTransition(_playerSeatedTrigger, (player, t) => OnPlayerSeated(player))
        .InternalTransition(_playerLeftTrigger, (player, t) => OnPlayerLeft(player))
        .Permit(Trigger.TableReady, State.HandInProgress);

      _gameStateMachine.Configure(State.HandInProgress)
        .InternalTransition(_playerSeatedTrigger, (player, t) => OnPlayerSeated(player))
        .InternalTransition(_playerLeftTrigger, (player, t) => OnPlayerLeft(player))
        .OnEntry(() => BeginGame())
        .Permit(Trigger.GameReady, State.PreFlop);

      _gameStateMachine.Configure(State.PreFlop)
        .SubstateOf(State.HandInProgress)
        .InternalTransition(_betTrigger, (player, bet, t) => OnPlayerBet(player, bet))
        .InternalTransition(_foldTrigger, (player, t) => OnPlayerFold(player))
        .Permit(Trigger.BettingComplete, State.Flop);

      _gameStateMachine.Configure(State.Flop)
        .SubstateOf(State.HandInProgress)
        .InternalTransition(_betTrigger, (player, bet, t) => OnPlayerBet(player, bet))
        .InternalTransition(_foldTrigger, (player, t) => OnPlayerFold(player))
        .Permit(Trigger.BettingComplete, State.Turn);

      _gameStateMachine.Configure(State.Turn)
        .SubstateOf(State.HandInProgress)
        .InternalTransition(_betTrigger, (player, bet, t) => OnPlayerBet(player, bet))
        .InternalTransition(_foldTrigger, (player, t) => OnPlayerFold(player))
        .Permit(Trigger.BettingComplete, State.River);

      _gameStateMachine.Configure(State.River)
        .SubstateOf(State.HandInProgress)
        .InternalTransition(_betTrigger, (player, bet, t) => OnPlayerBet(player, bet))
        .InternalTransition(_foldTrigger, (player, t) => OnPlayerFold(player))
        .Permit(Trigger.BettingComplete, State.HandComplete)
        .OnExit(t => Showdown());

      _gameStateMachine.Configure(State.HandComplete)
        .OnEntry(t => MoveButton())
        .Permit(Trigger.ReadyForNewHand, State.HandInProgress);
    }

    private void InitializeGame()
    {
      _gameState.InitializeGame();
    }

    private void BeginGame()
    {
      _gameState.BeginGame();
    }

    private void OnPlayerSeated(Player player)
    {
      _gameState.SeatPlayer(player);
    }

    private void OnPlayerLeft(Player player)
    {
      _gameState.RemovePlayer(player);
    }

    private void OnPlayerBet(Player player, int bet)
    {
      _gameState.Bet(player, bet);
    }

    private void OnPlayerFold(Player player)
    {
      _gameState.Fold(player);
    }

    private void Showdown()
    {
      _gameState.Showdown();
    }

    private void MoveButton()
    {
      throw new NotImplementedException();
    }
  }
}
