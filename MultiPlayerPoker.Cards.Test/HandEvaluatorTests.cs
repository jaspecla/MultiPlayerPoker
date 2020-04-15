using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace MultiPlayerPoker.Cards.Test
{
  [TestClass]
  public class HandEvaluatorTests
  {
    private RecursiveHandEvaluator _evaulatorUnderTest;

    [TestInitialize]
    public void Initialize()
    {
      _evaulatorUnderTest = new RecursiveHandEvaluator();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void LessThanFiveCardHand_ShouldThrowException()
    {
      var hand = new List<Card>();

      _evaulatorUnderTest.EvaluateHand(hand);

    }

    [TestMethod]
    public void RecognizeHighCard()
    {
      var hand = new List<Card>
      {
        new Card("A", "H"),
        new Card("4", "D"),
        new Card("9", "C"),
        new Card("10", "S"),
        new Card("J", "C"),
        new Card("3", "C"),
        new Card("2", "H")
      };

      var result = _evaulatorUnderTest.EvaluateHand(hand);
      Assert.AreEqual(HandType.High, result.Hand);
      Assert.AreEqual("A", Card.GetRankForValue(result.Determinant));
      Assert.AreEqual("J", Card.GetRankForValue(result.Kicker));
      Assert.AreEqual("10", Card.GetRankForValue(result.SecondKicker));
      Assert.AreEqual("9", Card.GetRankForValue(result.ThirdKicker));
      Assert.AreEqual("4", Card.GetRankForValue(result.FourthKicker));

    }

    [TestMethod]
    public void RecognizePair()
    {
      var hand = new List<Card>
      {
        new Card("A", "H"),
        new Card("4", "D"),
        new Card("A", "C"),
        new Card("10", "S"),
        new Card("J", "C"),
        new Card("3", "C"),
        new Card("2", "H")
      };

      var result = _evaulatorUnderTest.EvaluateHand(hand);
      Assert.AreEqual(HandType.Pair, result.Hand);
      Assert.AreEqual("A", Card.GetRankForValue(result.Determinant));
      Assert.AreEqual("J", Card.GetRankForValue(result.Kicker));
      Assert.AreEqual("10", Card.GetRankForValue(result.SecondKicker));
      Assert.AreEqual("4", Card.GetRankForValue(result.ThirdKicker));

    }

    [TestMethod]
    public void RecognizeTwoPair()
    {
      var hand = new List<Card>
      {
        new Card("A", "H"),
        new Card("A", "C"),
        new Card("J", "S"),
        new Card("J", "C"),
        new Card("9", "D"),
        new Card("7", "S"),
        new Card("3", "H")
      };

      var result = _evaulatorUnderTest.EvaluateHand(hand);
      Assert.AreEqual(HandType.TwoPair, result.Hand);
      Assert.AreEqual("A", Card.GetRankForValue(result.Determinant));
      Assert.AreEqual("J", Card.GetRankForValue(result.SubDeterminant));
      Assert.AreEqual("9", Card.GetRankForValue(result.Kicker));
    }

    [TestMethod]
    public void RecognizeThreeOfAKind()
    {
      var hand = new List<Card>
      {
        new Card("A", "H"),
        new Card("A", "C"),
        new Card("A", "S"),
        new Card("J", "C"),
        new Card("9", "D"),
        new Card("7", "S"),
        new Card("3", "H")
      };

      var result = _evaulatorUnderTest.EvaluateHand(hand);
      Assert.AreEqual(HandType.ThreeOfAKind, result.Hand);
      Assert.AreEqual("A", Card.GetRankForValue(result.Determinant));
      Assert.AreEqual("J", Card.GetRankForValue(result.Kicker));
      Assert.AreEqual("9", Card.GetRankForValue(result.SecondKicker));

    }

    [TestMethod]
    public void RecognizeFullHouse()
    {
      var hand = new List<Card>
      {
        new Card("A", "H"),
        new Card("A", "C"),
        new Card("A", "S"),
        new Card("J", "C"),
        new Card("J", "D"),
        new Card("7", "S"),
        new Card("3", "H")
      };

      var result = _evaulatorUnderTest.EvaluateHand(hand);
      Assert.AreEqual(HandType.FullHouse, result.Hand);
      Assert.AreEqual("A", Card.GetRankForValue(result.Determinant));
      Assert.AreEqual("J", Card.GetRankForValue(result.SubDeterminant));

    }

    [TestMethod]
    public void RecognizeFourOfAKind()
    {
      var hand = new List<Card>
      {
        new Card("A", "H"),
        new Card("A", "C"),
        new Card("A", "S"),
        new Card("A", "D"),
        new Card("J", "D"),
        new Card("7", "S"),
        new Card("3", "H")
      };

      var result = _evaulatorUnderTest.EvaluateHand(hand);
      Assert.AreEqual(HandType.FourOfAKind, result.Hand);
      Assert.AreEqual("A", Card.GetRankForValue(result.Determinant));
      Assert.AreEqual("J", Card.GetRankForValue(result.Kicker));

    }

    [TestMethod]
    public void RecognizeStraight()
    {
      var hand = new List<Card>
      {
        new Card("7", "H"),
        new Card("8", "C"),
        new Card("9", "S"),
        new Card("10", "D"),
        new Card("J", "D"),
        new Card("7", "S"),
        new Card("3", "H")
      };

      var result = _evaulatorUnderTest.EvaluateHand(hand);
      Assert.AreEqual(HandType.Straight, result.Hand);
      Assert.AreEqual("J", Card.GetRankForValue(result.Determinant));
    }

    [TestMethod]
    public void RecognizeWheelStraight()
    {
      var hand = new List<Card>
      {
        new Card("A", "H"),
        new Card("2", "C"),
        new Card("3", "S"),
        new Card("4", "D"),
        new Card("5", "D"),
        new Card("7", "S"),
        new Card("3", "H")
      };

      var result = _evaulatorUnderTest.EvaluateHand(hand);
      Assert.AreEqual(HandType.Straight, result.Hand);
      Assert.AreEqual("5", Card.GetRankForValue(result.Determinant));
    }


    [TestMethod]
    public void RecognizeFlush()
    {
      var hand = new List<Card>
      {
        new Card("4", "D"),
        new Card("8", "C"),
        new Card("2", "D"),
        new Card("10", "D"),
        new Card("J", "D"),
        new Card("7", "S"),
        new Card("3", "D")
      };

      var result = _evaulatorUnderTest.EvaluateHand(hand);
      Assert.AreEqual(HandType.Flush, result.Hand);
      Assert.AreEqual("J", Card.GetRankForValue(result.Determinant));

    }

    [TestMethod]
    public void RecognizeStraightFlush()
    {
      var hand = new List<Card>
      {
        new Card("7", "H"),
        new Card("8", "H"),
        new Card("9", "H"),
        new Card("10", "H"),
        new Card("J", "H"),
        new Card("7", "S"),
        new Card("3", "C")
      };

      var result = _evaulatorUnderTest.EvaluateHand(hand);
      Assert.AreEqual(HandType.StraightFlush, result.Hand);
      Assert.AreEqual("J", Card.GetRankForValue(result.Determinant));

    }

  }
}
