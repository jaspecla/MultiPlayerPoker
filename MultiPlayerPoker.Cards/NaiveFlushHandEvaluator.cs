using System;
using System.Collections.Generic;
using System.Text;

namespace MultiPlayerPoker.Cards
{
  public class NaiveFlushHandEvaluator
  {
    public HandEvaluatorResult Evaluate(List<Card> hand)
    {

      if (hand.Count != 5)
      {
        throw new ArgumentException("Hands to evaluate must be 5 cards.", nameof(hand));
      }

      var cardComparer = new CardComparer();
      hand.Sort(cardComparer);

      var result = new HandEvaluatorResult();
      for (int i = 1; i < hand.Count; i++)
      {
        if (hand[i].Suit != hand[i-1].Suit)
        {
          result.Hand = HandType.INVALID_HAND;
          return result;
        }
      }

      result.Hand = HandType.Flush;
      result.Determinant = hand[hand.Count - 1].RankValue;

      return result;

    }

  }
}
