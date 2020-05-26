using System;
using System.Collections.Generic;
using System.Text;

namespace MultiPlayerPoker.Game
{
  internal class GameProperties
  {
    internal GameEventBroker EventBroker { get; set; }
    internal GameActions Actions { get; set; }
    internal int MaxPlayers { get; set; }
    internal int MinBuyIn { get; set; }
    internal int MaxBuyIn { get; set; }
    internal int BigBlind { get; set; }
    internal int SmallBlind { get; set; }
  }
}
