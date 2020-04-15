using System;
using System.Collections.Generic;
using System.Text;

namespace MultiPlayerPoker.Cards
{
  public class CardComparer : IComparer<Card>
  {
    public int Compare(Card x, Card y)
    {
      return x.RankValue.CompareTo(y.RankValue);
    }
  }
}
