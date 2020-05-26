using MultiPlayerPoker.Cards;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiPlayerPoker.Game
{
  internal class Pot
  {
    internal int Value { get; private set; }
    internal List<Player> EligiblePlayers { get; private set; }
    internal string PotType { get; private set; }

    private readonly GameEventBroker _eventBroker;

    internal Pot(GameEventBroker eventBroker, string potType)
    {
      _eventBroker = eventBroker;

      PotType = potType;

      EligiblePlayers = new List<Player>();
    }

    internal void Initialize(List<Player> players)
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

    internal void AddValue(int value)
    {
      Value += value;
    }

    internal void Showdown(List<Card> communityCards)
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
      List<Player> playersInPot = null;
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
          playersInPot = new List<Player> { winningPlayer };
        }

        else if (results[player] == results[winningPlayer])
        {
          if (playersInPot == null)
          {
            playersInPot = new List<Player>
            {
              player,
              winningPlayer
            };
          }
          else
          {
            playersInPot.Add(player);
          }
        }

      }

      foreach (var player in playersInPot)
      {
        _eventBroker.SendPlayerWonMoney(player, (Value / playersInPot.Count));
      }
    }
  }
}
