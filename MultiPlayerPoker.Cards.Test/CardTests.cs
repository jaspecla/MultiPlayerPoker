using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiPlayerPoker.Cards.Test
{
  [TestClass]
  public class CardTests
  {

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Card_WithInvalidRank_ShouldThrowException()
    {
      var card = new Card("X", "H");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Card_WithInvalidSuit_ShouldThrowException()
    {
      var card = new Card("3", "X");
    }

    [TestMethod]
    public void Cards_ShouldReturnRankValue()
    {
      var card = new Card("2", "C");
      Assert.AreEqual(2, card.RankValue);
    }

    [TestMethod]
    public void FaceCards_ShouldReturnRankValue()
    {
      var card = new Card("K", "S");
      Assert.AreEqual(13, card.RankValue);
    }

  }
}
