using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiPlayerPoker.Cards.Test
{
  [TestClass]
  public class DeckTests
  {
    [TestMethod]
    public void Shuffle_ShouldInclude_EveryCard()
    {
      var unshuffledDeck = Deck.Unshuffled();
      var shuffledDeck = Deck.Shuffle();

      foreach (var card in shuffledDeck)
      {
        Assert.IsTrue(unshuffledDeck.Contains(card));
      }

    }

    [TestMethod]
    public void Shuffle_ShouldNotInclude_RepeatedCards()
    {
      var seenCards = new List<Card>();
      var shuffledDeck = Deck.Shuffle();

      foreach (var card in shuffledDeck)
      {
        Assert.IsFalse(seenCards.Contains(card));
        seenCards.Add(card);
        Assert.IsTrue(seenCards.Contains(card));
      }
    }
  }
}
