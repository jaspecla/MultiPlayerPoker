using MultiPlayerPoker.Cards;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MultiPlayerPoker.Game
{
  internal class GameEventArgs : EventArgs
  {
    internal Player Player { get; private set; }
    internal int Amount { get; private set; }
    internal Card[] Cards { get; private set; }
    
    internal GameEventArgs(Player player = null, int amount = 0, Card[] cards = null)
    {
      Player = player;
      Amount = amount;
      Cards = cards;
    }
  }
}
