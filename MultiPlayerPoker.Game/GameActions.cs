using MultiPlayerPoker.Cards;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiPlayerPoker.Game
{
  internal class GameActions
  {
    internal List<IGameLogOutput> LogOutputs { get; private set; }

    internal delegate bool SeatPlayer(Player player);
    internal delegate bool LeavePlayer(Player player);
    internal delegate bool PausePlayer(Player player);
    internal delegate bool UnpausePlayer(Player player);
    internal delegate bool PlayerBet(Player player, int amount);
    internal delegate bool PlayerBlind(Player player, int amount, string blindType);
    internal delegate bool PlayerFold(Player player);

    internal SeatPlayer TrySeatPlayerDelegate { get; set; }
    internal LeavePlayer TryLeavePlayerDelegate { get; set; }
    internal PausePlayer TryPausePlayerDelegate { get; set; }
    internal UnpausePlayer TryUnpausePlayerDelegate { get; set; }
    internal PlayerBet TryPlayerBetDelegate { get; set; }
    internal PlayerBlind TryPlayerBlindDelegate { get; set; }
    internal PlayerFold TryPlayerFoldDelegate { get; set; }

    internal GameActions()
    {
      LogOutputs = new List<IGameLogOutput>();
    }

    private void SendLogOutput(string message)
    {
      foreach (var output in LogOutputs)
      {
        output.Log(message);
      }
    }

    internal bool TrySeatPlayer(Player player)
    {
      if (TrySeatPlayerDelegate == null)
      {
        return false;
      }

      SendLogOutput($"Trying to seat player {player}.");
      return TrySeatPlayerDelegate(player);
    }

    internal bool TryLeavePlayer(Player player)
    {
      if (TryLeavePlayerDelegate == null)
      {
        return false;
      }

      SendLogOutput($"Trying to remove player {player} from the table.");
      return TryLeavePlayerDelegate(player);
    }

    internal bool TryPausePlayer(Player player)
    {
      if (TryPausePlayerDelegate == null)
      {
        return false;
      }

      SendLogOutput($"Trying to sit out player {player}.");
      return TryPausePlayerDelegate(player);
    }

    internal bool TryUnpausePlayer(Player player)
    {
      if (TryUnpausePlayerDelegate == null)
      {
        return false;
      }

      SendLogOutput($"Trying to return player {player}.");
      return TryUnpausePlayerDelegate(player);
    }


    internal bool TryPlayerBet(Player player, int amount)
    {
      if (TryPlayerBetDelegate == null)
      {
        return false;
      }

      SendLogOutput($"Player {player} is trying to bet {amount}.");
      return TryPlayerBetDelegate(player, amount);
    }

    internal bool TryPlayerBlind(Player player, int amount, string blindType)
    {
      if (TryPlayerBlindDelegate == null)
      {
        return false;
      }

      SendLogOutput($"Player {player} is trying to blind-bet the {blindType} blind for {amount}.");
      return TryPlayerBlindDelegate(player, amount, blindType);
    }

    internal bool TryPlayerFold(Player player)
    {
      if (TryPlayerFoldDelegate == null)
      {
        return false;
      }

      SendLogOutput($"Player {player} is tryig to fold.");
      return TryPlayerFoldDelegate(player);
    }

  }


}
