using MultiPlayerPoker.Cards;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiPlayerPoker.Game
{
  public class GameActions
  {
    public delegate bool SeatPlayer(Player player);
    public delegate bool LeavePlayer(Player player);
    public delegate bool PausePlayer(Player player);
    public delegate bool UnpausePlayer(Player player);
    public delegate bool PlayerBet(Player player, int amount);
    public delegate bool PlayerBlind(Player player, int amount, string blindType);
    public delegate bool PlayerWonMoney(Player player, int amount, string potType);
    public delegate bool PlayerFold(Player player);
    public delegate bool PlayerDealCard(Player player, Card card);
    public delegate bool DealCommunityCards(Card[] cards);

    public SeatPlayer TrySeatPlayerDelegate { get; set; }
    public LeavePlayer TryLeavePlayerDelegate { get; set; }
    public PausePlayer TryPausePlayerDelegate { get; set; }
    public UnpausePlayer TryUnpausePlayerDelegate { get; set; }
    public PlayerBet TryPlayerBetDelegate { get; set; }
    public PlayerBlind TryPlayerBlindDelegate { get; set; }
    public PlayerFold TryPlayerFoldDelegate { get; set; }
    public PlayerDealCard TryPlayerDealCardDelegate { get; set; }
    public DealCommunityCards TryDealCommunityCardsDelgate { get; set; }

    public bool TrySeatPlayer(Player player)
    {
      if (TrySeatPlayerDelegate == null)
      {
        return false;
      }

      return TrySeatPlayerDelegate(player);
    }

    public bool TryLeavePlayer(Player player)
    {
      if (TryLeavePlayerDelegate == null)
      {
        return false;
      }

      return TryLeavePlayerDelegate(player);
    }

    public bool TryPausePlayer(Player player)
    {
      if (TryPausePlayerDelegate == null)
      {
        return false;
      }

      return TryPausePlayerDelegate(player);
    }

    public bool TryPlayerBet(Player player, int amount)
    {
      if (TryPlayerBetDelegate == null)
      {
        return false;
      }

      return TryPlayerBetDelegate(player, amount);
    }

    public bool TryPlayerBlind(Player player, int amount, string blindType)
    {
      if (TryPlayerBlindDelegate == null)
      {
        return false;
      }

      return TryPlayerBlindDelegate(player, amount, blindType);
    }

    public bool TryPlayerFold(Player player)
    {
      if (TryPlayerFoldDelegate == null)
      {
        return false;
      }

      return TryPlayerFoldDelegate(player);
    }

    public bool TryPlayerDealCard(Player player, Card card)
    {
      if (TryPlayerDealCardDelegate == null)
      {
        return false;
      }
      
      return TryPlayerDealCardDelegate(player, card);
    }

    public bool TryDealCommunityCards(Player player, Card[] cards)
    {
      if (TryDealCommunityCardsDelgate == null)
      {
        return false;
      }

      return TryDealCommunityCardsDelgate(cards);
    }

  }


}
