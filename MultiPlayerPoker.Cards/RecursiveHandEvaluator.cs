using System;
using System.Collections.Generic;
using System.Text;

namespace MultiPlayerPoker.Cards
{
  public class RecursiveHandEvaluator
  {
    private List<HandEvaluatorResult> _possibleResults;

    private NaiveMatchingHandEvaluator _matchingHandEvaluator;
    private NaiveStraightHandEvaluator _straightHandEvaluator;
    private NaiveFlushHandEvaluator _flushHandEvaluator;

    public RecursiveHandEvaluator()
    {
      _possibleResults = new List<HandEvaluatorResult>();
      _matchingHandEvaluator = new NaiveMatchingHandEvaluator();
      _straightHandEvaluator = new NaiveStraightHandEvaluator();
      _flushHandEvaluator = new NaiveFlushHandEvaluator();
    }

    public HandEvaluatorResult EvaluateHand(List<Card> hand)
    {
      if (hand.Count < 5)
      {
        throw new ArgumentException("A hand must have at least five cards.", nameof(hand));
      }

      for (int i = 0; i < hand.Count; i++)
      {
        EvaluateGreaterThanFiveCardHand(hand, i);
      }

      HandEvaluatorResult highestPossibleHand = null;
      foreach (var result in _possibleResults)
      {
        if (highestPossibleHand == null || result > highestPossibleHand)
        {
          highestPossibleHand = result;
        }
      }
      return highestPossibleHand;

    }

    private void EvaluateGreaterThanFiveCardHand(List<Card> hand, int cardToRemove)
    {
      // If there are more than 5 cards in the hand, remove each card one at a time, and recursively 
      // determine the best hand from the result
      if (hand.Count > 5)
      {
        var smallerHand = new List<Card>(hand);
        smallerHand.RemoveAt(cardToRemove);

        if (smallerHand.Count > 5)
        {
          for (int i = 0; i < smallerHand.Count; i++)
          {
            EvaluateGreaterThanFiveCardHand(smallerHand, i);
          }
        }
        else
        {
          _possibleResults.Add(EvaluateFiveCardHand(smallerHand));
        }
      }

      if (hand.Count == 5)
      {
        // This hand is exactly five cards, so evaluate it.
        _possibleResults.Add(EvaluateFiveCardHand(hand));
      }
    }

    private HandEvaluatorResult EvaluateFiveCardHand(List<Card> hand)
    {
      var straightOrFlushResult = new HandEvaluatorResult();

      var matchingResult = _matchingHandEvaluator.Evaluate(hand);
      var straightResult = _straightHandEvaluator.Evaluate(hand);
      var flushResult = _flushHandEvaluator.Evaluate(hand);

      // Straight flushes
      if (straightResult.Hand == HandType.Straight && flushResult.Hand == HandType.Flush)
      {
        // Royal Flush
        if (hand[0].Rank == "10" && hand[hand.Count - 1].Rank == "A")
        {
          straightOrFlushResult.Hand = HandType.RoyalFlush;
          straightOrFlushResult.Determinant = flushResult.Determinant;
        }

        // Wheel straight flush
        else if (hand[0].Rank == "2" && hand[hand.Count - 1].Rank == "A")
        {
          straightOrFlushResult.Hand = HandType.StraightFlush;
          straightOrFlushResult.Determinant = 5;
        }

        else
        {
          straightOrFlushResult.Hand = HandType.StraightFlush;
          straightOrFlushResult.Determinant = flushResult.Determinant;
        }
      }
      else if (flushResult.Hand == HandType.Flush)
      {
        straightOrFlushResult = flushResult;
      }
      else if (straightResult.Hand == HandType.Straight)
      {
        straightOrFlushResult = straightResult;
      }
      else
      {
        straightOrFlushResult.Hand = HandType.INVALID_HAND;
      }

      if (matchingResult > straightOrFlushResult)
      {
        return matchingResult;
      }
      else
      {
        return straightOrFlushResult;
      }

    }
  }
}
