using MultiPlayerPoker.Cards;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MultiPlayerPoker.Game
{
  internal class GameEventBroker
  {
    internal List<IGameLogOutput> LogOutputs { get; private set; }

    internal event EventHandler<GameEventArgs> GameReady;
    internal event EventHandler<GameEventArgs> TableReady;

    internal event EventHandler<GameEventArgs> PreFlopReady;
    internal event EventHandler<GameEventArgs> FlopReady;
    internal event EventHandler<GameEventArgs> TurnReady;
    internal event EventHandler<GameEventArgs> RiverReady;

    internal event EventHandler<GameEventArgs> PlayerWasSeated;
    internal event EventHandler<GameErrorEventArgs> FailSeatPlayer;

    internal event EventHandler<GameEventArgs> PlayerLeft;
    internal event EventHandler<GameErrorEventArgs> FailPlayerLeft;

    internal event EventHandler<GameEventArgs> PlayerWasPaused;
    internal event EventHandler<GameErrorEventArgs> FailPausePlayer;

    internal event EventHandler<GameEventArgs> PlayerWasUnpaused;
    internal event EventHandler<GameErrorEventArgs> FailUnpausePlayer;

    internal event EventHandler<GameEventArgs> PlayerDidBet;
    internal event EventHandler<GameErrorEventArgs> FailPlayerBet;

    internal event EventHandler<GameEventArgs> PlayerDidBlind;
    internal event EventHandler<GameErrorEventArgs> FailPlayerBlind;

    internal event EventHandler<GameEventArgs> PlayerDidFold;
    internal event EventHandler<GameErrorEventArgs> FailPlayerFold;

    internal event EventHandler<GameEventArgs> ActionOnPlayer;
    internal event EventHandler<GameEventArgs> BettingCompleted;
    internal event EventHandler<GameEventArgs> Showdown;
    internal event EventHandler<GameEventArgs> ReadyForNewHand;
    internal event EventHandler<GameEventArgs> PlayerWonMoney;

    internal event EventHandler<GameEventArgs> PlayerCardDealt;
    internal event EventHandler<GameEventArgs> CommunityCardsDealt;

    internal GameEventBroker()
    {
      LogOutputs = new List<IGameLogOutput>();
    }

    private void PublishGameEvent(
      EventHandler<GameEventArgs> handler, 
      Player player = null, 
      int amount = 0, 
      Card[] cards = null)
    {
      if (handler != null)
      {
        handler(this, new GameEventArgs(player, amount, cards));
      }
    }

    private void PublishGameError(EventHandler<GameErrorEventArgs> errorHandler, string message, Player player = null, Exception ex = null)
    {
      if (errorHandler != null)
      {
        errorHandler(this, new GameErrorEventArgs(message, player, ex));
      }
    }

    private void SendLogOutput(string message)
    {
      foreach (var output in LogOutputs)
      {
        output.Log(message);
      }
    }

    internal void SendGameReady()
    {
      var handler = GameReady;
      PublishGameEvent(handler);
      SendLogOutput($"Game is ready.");
    }

    internal void SendTableReady()
    {
      var handler = TableReady;
      PublishGameEvent(handler);
      SendLogOutput($"Table is ready.");
    }

    internal void SendPreFlopReady()
    {
      var handler = PreFlopReady;
      PublishGameEvent(handler);
      SendLogOutput($"Pre-flop is ready.");
    }

    internal void SendFlopReady()
    {
      var handler = FlopReady;
      PublishGameEvent(handler);
      SendLogOutput($"Flop is ready.");
    }

    internal void SendTurnReady()
    {
      var handler = TurnReady;
      PublishGameEvent(handler);
      SendLogOutput($"Turn is ready.");
    }

    internal void SendRiverReady()
    {
      var handler = RiverReady;
      PublishGameEvent(handler);
      SendLogOutput($"River is ready.");
    }

    internal void SendActionOnPlayer(Player player)
    {
      var handler = ActionOnPlayer;
      PublishGameEvent(handler, player);
      SendLogOutput($"Action is on player {player}.");
    }

    internal void SendBettingCompleted()
    {
      var handler = BettingCompleted;
      PublishGameEvent(handler);
      SendLogOutput($"Betting is complete for current round.");
    }

    internal void SendShowdown()
    {
      var handler = Showdown;
      PublishGameEvent(handler);
      SendLogOutput($"Showdown for this round.");
    }

    internal void SendReadyForNewHand()
    {
      var handler = ReadyForNewHand;
      PublishGameEvent(handler);
      SendLogOutput($"Ready for new hand.");
    }

    internal void SendPlayerWasSeated(Player player)
    {
      var handler = PlayerWasSeated;
      PublishGameEvent(handler, player);
      SendLogOutput($"Player {player} was seated.");
    }

    internal void SendFailSeatPlayer(string message, Player player, Exception ex = null)
    {
      var handler = FailSeatPlayer;
      PublishGameError(handler, message, player, ex);
      SendLogOutput($"Failed to seat player {player}: {message}");
    }

    internal void SendPlayerLeft(Player player)
    {
      var handler = PlayerLeft;
      PublishGameEvent(handler, player);
      SendLogOutput($"Player {player} has left the game.");
    }

    internal void SendFailPlayerLeft(string message, Player player, Exception ex = null)
    {
      var handler = FailPlayerLeft;
      PublishGameError(handler, message, player, ex);
      SendLogOutput($"Could not remove player {player} from game: {message}");
    }

    internal void SendPlayerWasPaused(Player player)
    {
      var handler = PlayerWasPaused;
      PublishGameEvent(handler, player);
      SendLogOutput($"Player {player} has sat out of the game.");
    }

    internal void SendFailPausePlayer(string message, Player player, Exception ex = null)
    {
      var handler = FailPausePlayer;
      PublishGameError(handler, message, player, ex);
      SendLogOutput($"Could not sit out player {player}: {message}.");
    }

    internal void SendPlayerWasUnpaused(Player player)
    {
      var handler = PlayerWasUnpaused;
      PublishGameEvent(handler, player);
      SendLogOutput($"Player {player} has returned to their game.");
    }

    internal void SendFailUnpausePlayer(string message, Player player, Exception ex = null)
    {
      var handler = FailUnpausePlayer;
      PublishGameError(handler, message, player, ex);
      SendLogOutput($"Could not return player {player} to their game: {message}.");
    }

    internal void SendPlayerDidBet(Player player, int amount)
    {
      var handler = PlayerDidBet;
      PublishGameEvent(handler, player, amount);
      SendLogOutput($"Player {player} bet {amount}.");
    }

    internal void SendFailPlayerBet(string message, Player player, Exception ex = null)
    {
      var handler = FailPlayerBet;
      PublishGameError(handler, message, player, ex);
      SendLogOutput($"Player {player} could not bet: {message}.");
    }

    internal void SendPlayerDidBlind(Player player, int amount)
    {
      var handler = PlayerDidBlind;
      PublishGameEvent(handler, player, amount);
      SendLogOutput($"Player {player} blind bet {amount}.");
    }

    internal void SendFailPlayerBlind(string message, Player player, Exception ex = null)
    {
      var handler = FailPlayerBlind;
      PublishGameError(handler, message, player, ex);
      SendLogOutput($"Player {player} could not blind bet: {message}");
    }

    internal void SendPlayerDidFold(Player player)
    {
      var handler = PlayerDidFold;
      PublishGameEvent(handler, player);
      SendLogOutput($"Player {player} folded.");
    }

    internal void SendFailPlayerFold(string message, Player player)
    {
      var handler = FailPlayerFold;
      PublishGameError(handler, message, player);
      SendLogOutput($"Player {player} could not fold: {message}.");
    }

    internal void SendPlayerWonMoney(Player player, int amount)
    {
      var handler = PlayerWonMoney;
      PublishGameEvent(handler, player, amount);
      SendLogOutput($"Player {player} won {amount}.");
    }

    internal void SendPlayerCardDelt(Player player, Card card)
    {
      var handler = PlayerCardDealt;
      PublishGameEvent(handler, player, cards: new Card[] { card });
      SendLogOutput($"Player {player} was dealt {card}.");
    }

    internal void SendCommunityCardsDealt(Card[] cards)
    {
      var handler = CommunityCardsDealt;
      PublishGameEvent(handler, player: null, cards: cards);
      
      StringBuilder message = new StringBuilder("Community cards were dealt: ");
      foreach (var card in cards)
      {
        message.Append($"{card} ");
      }

      SendLogOutput(message.ToString());
    }

  }
}
