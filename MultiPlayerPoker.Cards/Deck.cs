using System;
using System.Collections.Generic;
using System.Text;

namespace MultiPlayerPoker.Cards
{
  public static class Deck
  {
    private static Card[] _unshuffledDeck =
    {
      new Card("A", "H"),
      new Card("2", "H"),
      new Card("3", "H"),
      new Card("4", "H"),
      new Card("5", "H"),
      new Card("6", "H"),
      new Card("7", "H"),
      new Card("8", "H"),
      new Card("9", "H"),
      new Card("10", "H"),
      new Card("J", "H"),
      new Card("Q", "H"),
      new Card("K", "H"),
      new Card("A", "D"),
      new Card("2", "D"),
      new Card("3", "D"),
      new Card("4", "D"),
      new Card("5", "D"),
      new Card("6", "D"),
      new Card("7", "D"),
      new Card("8", "D"),
      new Card("9", "D"),
      new Card("10", "D"),
      new Card("J", "D"),
      new Card("Q", "D"),
      new Card("K", "D"),
      new Card("A", "C"),
      new Card("2", "C"),
      new Card("3", "C"),
      new Card("4", "C"),
      new Card("5", "C"),
      new Card("6", "C"),
      new Card("7", "C"),
      new Card("8", "C"),
      new Card("9", "C"),
      new Card("10", "C"),
      new Card("J", "C"),
      new Card("Q", "C"),
      new Card("K", "C"),
      new Card("A", "S"),
      new Card("2", "S"),
      new Card("3", "S"),
      new Card("4", "S"),
      new Card("5", "S"),
      new Card("6", "S"),
      new Card("7", "S"),
      new Card("8", "S"),
      new Card("9", "S"),
      new Card("10", "S"),
      new Card("J", "S"),
      new Card("Q", "S"),
      new Card("K", "S")
    };

    public static Card[] Unshuffled()
    {
      return _unshuffledDeck;
    }

    public static Card[] Shuffle()
    {
      var deckToShuffle = new Card[52];
      _unshuffledDeck.CopyTo(deckToShuffle, 0);

      // Implement Fisher-Yates shuffle
      // https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle

      var random = new Random();

      for (var i = 0; i < deckToShuffle.Length - 2; i++)
      {
        var posToExchange = random.Next(i, deckToShuffle.Length);

        // swap the cards
        var temp = deckToShuffle[posToExchange];
        deckToShuffle[posToExchange] = deckToShuffle[i];
        deckToShuffle[i] = temp;
      }

      return deckToShuffle;
    }
  }
}
