using System;
using System.Collections.Generic;
using System.Text;

namespace MultiPlayerPoker.Cards
{
  public class NaiveStraightHandEvaluator
  {
    public HandEvaluatorResult Evaluate(List<Card> hand)
    {
      if (hand.Count != 5)
      {
        throw new ArgumentException("Hands to evaluate must be 5 cards.", nameof(hand));
      }

      var result = new HandEvaluatorResult();

      var cardComparer = new CardComparer();
      hand.Sort(cardComparer);

      for (int i = 1; i < hand.Count; i++)
      {
        if (hand[i].RankValue - 1 != hand[i - 1].RankValue)
        {
          // Wheel straight
          if ((i == hand.Count - 1) && (hand[i].Rank == "A") && (hand[0].Rank == "2"))
          {
            result.Hand = HandType.Straight;
            result.Determinant = 5;
            return result;
          }
          else
          {
            result.Hand = HandType.INVALID_HAND;
            return result;
          }
        }
      }

      result.Hand = HandType.Straight;
      result.Determinant = hand[hand.Count - 1].RankValue;

      return result;
    }
  }
}
