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

    public event EventHandler<GameEventArgs> TrySeatPlayer;
    public event EventHandler<GameEventArgs> PlayerWasSeated;
    public event EventHandler<GameErrorEventArgs> FailSeatPlayer;
    
    public event EventHandler<GameEventArgs> PlayerLeft;

    public event EventHandler<GameEventArgs> TryPausePlayer;
    public event EventHandler<GameEventArgs> PlayerWasPaused;
    public event EventHandler<GameErrorEventArgs> FailPausePlayer;

    public event EventHandler<GameEventArgs> TryUnpausePlayer;
    public event EventHandler<GameEventArgs> PlayerWasUnpaused;
    public event EventHandler<GameErrorEventArgs> FailUnpausePlayer;

    public event EventHandler<GameEventArgs> TryPlayerBet;
    public event EventHandler<GameEventArgs> PlayerDidBet;
    public event EventHandler<GameErrorEventArgs> FailPlayerBet;

    public event EventHandler<GameEventArgs> TryPlayerFold;
    public event EventHandler<GameEventArgs> PlayerDidFold;
    public event EventHandler<GameErrorEventArgs> FailPlayerFold;

    public event EventHandler<GameEventArgs> BettingCompleted;
    public event EventHandler<GameEventArgs> ReadyForNewHand;
    public event EventHandler<GameEventArgs> PlayerWonMoney;
    
    public event EventHandler<GameEventArgs> PlayerCardDealt;
    public event EventHandler<GameEventArgs> CommunityCardsDealt;

    private void PublishGameEvent(EventHandler<GameEventArgs> handler, Player player = null, int amount = 0, Card[] cards = null)
    {
      if (handler != null)
      {
        handler(this, new GameEventArgs(player, amount, cards));
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

    public void SendBettingComplete()
    {
      var handler = BettingCompleted;
      PublishGameEvent(handler);
    }

    public void SendReadyForNewHand()
    {
      var handler = ReadyForNewHand;
      PublishGameEvent(handler);
    }

    public void SeatPlayer(Player player)
    {
      var handler = TrySeatPlayer;
      PublishGameEvent(handler, player);
    }

    public void SendPlayerWasSeated(Player player)
    {
      var handler = PlayerWasSeated;
      PublishGameEvent(handler, player);
    }

    public void SendPlayerLeft(Player player)
    {
      var handler = PlayerLeft;
      PublishGameEvent(handler, player);
    }

    public void SendPlayerSitOut(Player player)
    {
      var handler = PlayerSatOut;
      PublishGameEvent(handler, player);
    }

    public void SendPlayerSitIn(Player player)
    {
      var handler = PlayerSatIn;
      PublishGameEvent(handler, player);
    }

    public void SendPlayerBet(Player player, int amount)
    {
      var handler = PlayerBet;
      PublishGameEvent(handler, player, amount);
    }

    public void SendPlayerFold(Player player)
    {
      var handler = PlayerFolded;
      PublishGameEvent(handler, player);
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
