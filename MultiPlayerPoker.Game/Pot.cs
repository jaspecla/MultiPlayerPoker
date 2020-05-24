using MultiPlayerPoker.Cards;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiPlayerPoker.Game
{
  public class Pot
  {
    public int Value { get; set; }
    public List<Player> EligiblePlayers { get; private set; }

    public Pot()
    {
      EligiblePlayers = new List<Player>();
    }

    public void Initialize(List<Player> players)
    {
      Value = 0;
      foreach (var player in players)
      {
        if (player.IsInHand())
        {
          EligiblePlayers.Add(player);
        }
      }
    }

    public List<Player> GetWinners(Card[] communityCards)
    {
      var results = new Dictionary<Player, HandEvaluatorResult>();
      var handEvaluator = new RecursiveHandEvaluator();

      var playersInHand = new List<Player>();
      foreach (var player in EligiblePlayers)
      {
        if (player.IsInHand())
        {
          playersInHand.Add(player);
        }
      }

      foreach (var player in playersInHand)
      {
        var playerHand = new List<Card>();
        playerHand.AddRange(communityCards);
        playerHand.AddRange(player.HoleCards);

        results.Add(player, handEvaluator.EvaluateHand(playerHand));
      }

      Player winningPlayer = null;
      List<Player> choppedPot = null;
      foreach (var player in playersInHand)
      {
        if (winningPlayer == null)
        {
          winningPlayer = player;
          continue;
        }

        if (results[player] > results[winningPlayer])
        {
          winningPlayer = player;
          choppedPot = null;
        }

        else if (results[player] == results[winningPlayer])
        {
          if (choppedPot == null)
          {
            choppedPot = new List<Player>();
            choppedPot.Add(player);
            choppedPot.Add(winningPlayer);
          }
          else
          {
            choppedPot.Add(player);
          }
        }

      }

      if (choppedPot != null)
      {
        return choppedPot;
      }
      else
      {
        return new List<Player> { winningPlayer };
      }
    }
}
