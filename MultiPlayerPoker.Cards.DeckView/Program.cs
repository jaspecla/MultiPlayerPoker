using System;

namespace MultiPlayerPoker.Cards.DeckView
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Shuffling the deck...");
      var shuffledDeck = Deck.Shuffle();

      for (var i = 0; i < shuffledDeck.Length; i++)
      {
        Console.WriteLine($"{i + 1}: {shuffledDeck[i]}");
      }

    }
  }
}
