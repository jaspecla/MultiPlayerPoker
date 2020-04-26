using System;
using System.Linq;

namespace MultiPlayerPoker.Cards
{
  public class Card
  {
    private static string[] _validRanks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
    private static string[] _validSuits = { "H", "D", "C", "S" };
    public Card(string rank, string suit)
    {
      if (!_validRanks.Contains(rank))
      {
        throw new ArgumentException($"Invalid rank: {rank}");
      }
      if (!_validSuits.Contains(suit))
      {
        throw new ArgumentException($"Invalid suit: {suit}");
      }

      Rank = rank;
      RankValue = Array.FindIndex(_validRanks, x => x == rank) + 2;
      Suit = suit;

    }
    public string Rank { get; private set; }
    public int RankValue { get; private set; }

    public string Suit { get; private set; }

    public static string GetRankForValue(int value)
    {
      try
      {
        return _validRanks[value - 2];
      }
      catch (IndexOutOfRangeException ex)
      {
        throw new ArgumentException($"Value {value} does not correspond to a valid rank.", nameof(value), ex);
      }
    }

    public override string ToString()
    {
      string rankName;
      switch (Rank)
      {
        case ("A"):
          rankName = "Ace";
          break;
        case ("K"):
          rankName = "King";
          break;
        case ("Q"):
          rankName = "Queen";
          break;
        case ("J"):
          rankName = "Jack";
          break;
        default:
          rankName = Rank;
          break;
      }

      string suitName;
      switch (Suit)
      {
        case ("H"):
          suitName = "Hearts";
          break;
        case ("D"):
          suitName = "Diamonds";
          break;
        case ("C"):
          suitName = "Clubs";
          break;
        case ("S"):
          suitName = "Spades";
          break;
        default:
          suitName = "INVALID_SUIT";
          break;
      }

      return ($"{rankName} of {suitName}");
     
    }
  }
}
