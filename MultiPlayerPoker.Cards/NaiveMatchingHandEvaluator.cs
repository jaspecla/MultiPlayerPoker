using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiPlayerPoker.Cards
{
  public class NaiveMatchingHandEvaluator
  {
    public HandEvaluatorResult Evaluate(List<Card> hand)
    {
      if (hand.Count != 5)
      {
        throw new ArgumentException("Hands to evaluate must be 5 cards.", nameof(hand));
      }

      var cardQuantity = new Dictionary<int, int>();

      var kickers = new List<int>();

      foreach (var card in hand)
      {
        if (!cardQuantity.ContainsKey(card.RankValue))
        {
          cardQuantity[card.RankValue] = 1;
        }
        else
        {
          cardQuantity[card.RankValue]++;
        }
      }

      int quadsRankValue = 0;
      int tripsRankValue = 0;
      int topPairRankValue = 0;
      int secondPairRankValue = 0;
      foreach (var rankValue in cardQuantity.Keys)
      {

        if (cardQuantity[rankValue] == 4)
        {
          if (rankValue > quadsRankValue)
          {
            quadsRankValue = rankValue;
          }
        }

        if (cardQuantity[rankValue] == 3)
        {
          if (rankValue > tripsRankValue)
          {
            tripsRankValue = rankValue;
          }
        }

        if (cardQuantity[rankValue] == 2)
        {
          if (rankValue > topPairRankValue)
          {
            secondPairRankValue = topPairRankValue;
            topPairRankValue = rankValue;
          }
          else if (rankValue < topPairRankValue && rankValue > secondPairRankValue)
          {
            secondPairRankValue = rankValue;
          }
        }

        if (cardQuantity[rankValue] == 1)
        {
          kickers.Add(rankValue);
        }
      }

      // Get the kickers in descending order
      kickers.Sort();
      kickers.Reverse();

      var matchingCardEvaluatorResult = new HandEvaluatorResult();

      // Four of a Kind
      if (quadsRankValue > 0)
      {
        matchingCardEvaluatorResult.Hand = HandType.FourOfAKind;
        matchingCardEvaluatorResult.Determinant = quadsRankValue;
        matchingCardEvaluatorResult.Kicker = kickers.First();
        return matchingCardEvaluatorResult;
      }

      // Full House
      if (tripsRankValue > 0 && topPairRankValue > 0)
      {
        matchingCardEvaluatorResult.Hand = HandType.FullHouse;
        matchingCardEvaluatorResult.Determinant = tripsRankValue;
        matchingCardEvaluatorResult.SubDeterminant = topPairRankValue;
        return matchingCardEvaluatorResult;
      }

      // Three of a Kind
      if (tripsRankValue > 0)
      {
        matchingCardEvaluatorResult.Hand = HandType.ThreeOfAKind;
        matchingCardEvaluatorResult.Determinant = tripsRankValue;
        matchingCardEvaluatorResult.Kicker = kickers[0];
        matchingCardEvaluatorResult.SecondKicker = kickers[1];
        return matchingCardEvaluatorResult;
      }

      // Two Pair
      if (topPairRankValue > 0 && secondPairRankValue > 0)
      {
        matchingCardEvaluatorResult.Hand = HandType.TwoPair;
        matchingCardEvaluatorResult.Determinant = topPairRankValue;
        matchingCardEvaluatorResult.SubDeterminant = secondPairRankValue;
        matchingCardEvaluatorResult.Kicker = kickers.First();
        return matchingCardEvaluatorResult;
      }

      // One Pair
      if (topPairRankValue > 0)
      {
        matchingCardEvaluatorResult.Hand = HandType.Pair;
        matchingCardEvaluatorResult.Determinant = topPairRankValue;
        matchingCardEvaluatorResult.Kicker = kickers[0];
        matchingCardEvaluatorResult.SecondKicker = kickers[1];
        matchingCardEvaluatorResult.ThirdKicker = kickers[2];
        return matchingCardEvaluatorResult;
      }

      // High Card
      matchingCardEvaluatorResult.Hand = HandType.High;
      matchingCardEvaluatorResult.Determinant = kickers[0];
      matchingCardEvaluatorResult.Kicker = kickers[1];
      matchingCardEvaluatorResult.SecondKicker = kickers[2];
      matchingCardEvaluatorResult.ThirdKicker = kickers[3];
      matchingCardEvaluatorResult.FourthKicker = kickers[4];

      return matchingCardEvaluatorResult;
    }

  }
}
