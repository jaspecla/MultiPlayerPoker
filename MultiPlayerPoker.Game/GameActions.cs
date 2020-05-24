using MultiPlayerPoker.Cards;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiPlayerPoker.Game
{
  public class GameActions
  {
    public delegate void SeatPlayer(Player player);
    public delegate void LeavePlayer(Player player);
    public delegate void PausePlayer(Player player);
    public delegate void UnpausePlayer(Player player);
    public delegate void PlayerBet(Player player, int amount);
    public delegate void PlayerFold(Player player);
    public delegate void PlayerDealCard(Player player, Card card);
    public delegate void DealCommunityCards(Card[] cards);

    public SeatPlayer TrySeatPlayerDelegate { get; set; }
    public LeavePlayer TryLeavePlayerDelegate { get; set; }
    public PausePlayer TryPausePlayerDelegate { get; set; }
    public UnpausePlayer TryUnpausePlayerDelegate { get; set; }
    public PlayerBet TryPlayerBetDelegate { get; set; }
    public PlayerFold TryPlayerFoldDelegate { get; set; }
    public PlayerDealCard TryPlayerDealCardDelegate { get; set; }
    public DealCommunityCards TryDealCommunityCardsDelgate { get; set; }

    public void TrySeatPlayer(Player player)
    {
      TrySeatPlayerDelegate?.Invoke(player);
    }

    public void TryLeavePlayer(Player player)
    {
      TryLeavePlayerDelegate?.Invoke(player);
    }

    public void TryPausePlayer(Player player)
    {
      TryPausePlayerDelegate?.Invoke(player);
    }

    public void TryPlayerBet(Player player, int amount)
    {
      TryPlayerBetDelegate?.Invoke(player, amount);
    }

    public void TryPlayerFold(Player player)
    {
      TryPlayerFoldDelegate?.Invoke(player);
    }

    public void TryPlayerDealCard(Player player, Card card)
    {
      TryPlayerDealCardDelegate?.Invoke(player, card);
    }

    public void TryDealCommunityCards(Player player, Card[] cards)
    {
      TryDealCommunityCardsDelgate?.Invoke(cards);
    }

  }


}
