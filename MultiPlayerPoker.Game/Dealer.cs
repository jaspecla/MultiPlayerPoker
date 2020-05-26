using MultiPlayerPoker.Cards;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiPlayerPoker.Game
{
  internal class Dealer
  {
    private Card[] _deck;
    int _deckIndex;

    internal Dealer()
    {
      Shuffle();
    }

    internal void Shuffle()
    {
      _deck = Deck.Shuffle();
      _deckIndex = 0;
    }

    internal Card NextCard()
    {
      var card = _deck[_deckIndex];
      _deckIndex++;

      return card;
    }


  }
}
