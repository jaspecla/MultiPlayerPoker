using MultiPlayerPoker.Cards;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MultiPlayerPoker.Game
{
  public class GameEventBroker
  {
    public event EventHandler<GameEventArgs> GameReady;
    public event EventHandler<GameEventArgs> TableReady;

    public event EventHandler<GameEventArgs> PlayerWasSeated;
    public event EventHandler<GameErrorEventArgs> FailSeatPlayer;
    
    public event EventHandler<GameEventArgs> PlayerLeft;
    public event EventHandler<GameErrorEventArgs> FailPlayerLeft;

    public event EventHandler<GameEventArgs> PlayerWasPaused;
    public event EventHandler<GameErrorEventArgs> FailPausePlayer;

    public event EventHandler<GameEventArgs> PlayerWasUnpaused;
    public event EventHandler<GameErrorEventArgs> FailUnpausePlayer;

    public event EventHandler<GameEventArgs> PlayerDidBet;
    public event EventHandler<GameErrorEventArgs> FailPlayerBet;

    public event EventHandler<GameEventArgs> PlayerDidBlind;
    public event EventHandler<GameErrorEventArgs> FailPlayerBlind;

    public event EventHandler<GameEventArgs> PlayerDidFold;
    public event EventHandler<GameErrorEventArgs> FailPlayerFold;

    public event EventHandler<GameEventArgs> ActionOnPlayer;
    public event EventHandler<GameEventArgs> BettingCompleted;
    public event EventHandler<GameEventArgs> Showdown;
    public event EventHandler<GameEventArgs> ReadyForNewHand;
    public event EventHandler<GameEventArgs> PlayerWonMoney;
    
    public event EventHandler<GameEventArgs> PlayerCardDealt;
    public event EventHandler<GameEventArgs> CommunityCardsDealt;

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

    public void SendGameReady()
    {
      var handler = GameReady;
      PublishGameEvent(handler);
    }

    public void SendTableReady()
    {
      var handler = TableReady;
      PublishGameEvent(handler);
    }

    public void SendActionOnPlayer(Player player)
    {
      var handler = ActionOnPlayer;
      PublishGameEvent(handler, player);
    }

    public void SendBettingCompleted()
    {
      var handler = BettingCompleted;
      PublishGameEvent(handler);
    }

    public void SendShowdown()
    {
      var handler = Showdown;
      PublishGameEvent(handler);
    }

    public void SendReadyForNewHand()
    {
      var handler = ReadyForNewHand;
      PublishGameEvent(handler);
    }

    public void SendPlayerWasSeated(Player player)
    {
      var handler = PlayerWasSeated;
      PublishGameEvent(handler, player);
    }

    public void SendFailSeatPlayer(string message, Player player, Exception ex = null)
    {
      var handler = FailSeatPlayer;
      PublishGameError(handler, message, player, ex);
    }

    public void SendPlayerLeft(Player player)
    {
      var handler = PlayerLeft;
      PublishGameEvent(handler, player);
    }

    public void SendFailPlayerLeft(string message, Player player, Exception ex = null)
    {
      var handler = FailPlayerLeft;
      PublishGameError(handler, message, player, ex);
    }

    public void SendPlayerWasPaused(Player player)
    {
      var handler = PlayerWasPaused;
      PublishGameEvent(handler, player);
    }

    public void SendFailPausePlayer(string message, Player player, Exception ex = null)
    {
      var handler = FailPausePlayer;
      PublishGameError(handler, message, player, ex);
    }

    public void SendPlayerWasUnpaused(Player player)
    {
      var handler = PlayerWasUnpaused;
      PublishGameEvent(handler, player);
    }

    public void SendFailUnpausePlayer(string message, Player player, Exception ex = null)
    {
      var handler = FailUnpausePlayer;
      PublishGameError(handler, message, player, ex);
    }

    public void SendPlayerDidBet(Player player, int amount)
    {
      var handler = PlayerDidBet;
      PublishGameEvent(handler, player, amount);
    }

    public void SendFailPlayerBet(string message, Player player, Exception ex = null)
    {
      var handler = FailPlayerBet;
      PublishGameError(handler, message, player, ex);
    }

    public void SendPlayerDidBlind(Player player, int amount)
    {
      var handler = PlayerDidBlind;
      PublishGameEvent(handler, player, amount);
    }

    public void SendFailPlayerBlind(string message, Player player, Exception ex = null)
    {
      var handler = FailPlayerBlind;
      PublishGameError(handler, message, player, ex);
    }

    public void SendPlayerDidFold(Player player)
    {
      var handler = PlayerDidFold;
      PublishGameEvent(handler, player);
    }

    public void SendFailPlayerFold(string message, Player player)
    {
      var handler = FailPlayerFold;
      PublishGameError(handler, message, player);
    }

    public void SendPlayerWonMoney(Player player, int amount)
    {
      var handler = PlayerWonMoney;
      PublishGameEvent(handler, player, amount);
    }

    public void SendPlayerCardDelt(Player player, Card card)
    {
      var handler = PlayerCardDealt;
      PublishGameEvent(handler, player, cards: new Card[] { card });
    }

    public void SendCommunityCardsDealt(Player player, Card[] cards)
    {
      var handler = CommunityCardsDealt;
      PublishGameEvent(handler, player, cards: cards);
    }

  }
}
