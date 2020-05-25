using MultiPlayerPoker.Cards;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MultiPlayerPoker.Game
{
  public class GameEventArgs : EventArgs
  {
    public Player Player { get; private set; }
    public int Amount { get; private set; }
    public Card[] Cards { get; private set; }
    
    public GameEventArgs(Player player = null, int amount = 0, Card[] cards = null)
    {
      Player = player;
      Amount = amount;
      Cards = cards;
    }
  }
}
