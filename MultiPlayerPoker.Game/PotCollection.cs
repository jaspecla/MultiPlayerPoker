using MultiPlayerPoker.Cards;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiPlayerPoker.Game
{
  internal class PotCollection
  {
    internal Pot CurrentPot;

    private readonly GameEventBroker _eventBroker;
    private List<Card> _communityCards;
    private List<Pot> _pots;

    internal PotCollection(List<Player> eligiblePlayers, GameEventBroker eventBroker)
    {
      _eventBroker = eventBroker;

      _eventBroker.PlayerDidBet += OnPlayerAddedMoney;
      _eventBroker.PlayerDidBlind += OnPlayerAddedMoney;
      _eventBroker.CommunityCardsDealt += OnCommunityCardsDealt;
      _eventBroker.Showdown += OnShowdown;

      var mainPot = CreatePot("main", eligiblePlayers);
      _pots = new List<Pot> { mainPot };
      CurrentPot = mainPot;
    }

    internal void CreateSidePot(List<Player> eligiblePlayers)
    {
      var sidePot = CreatePot("side", eligiblePlayers);
      _pots.Add(sidePot);
      CurrentPot = sidePot;
    }

    private void OnPlayerAddedMoney(object sender, GameEventArgs args)
    {
      CurrentPot.AddValue(args.Amount);
    }

    private void OnCommunityCardsDealt(object sender, GameEventArgs args)
    {
      foreach (var card in args.Cards)
      {
        _communityCards.Add(card);
      }
    }

    private void OnShowdown(object sender, GameEventArgs args)
    {
      foreach (var pot in _pots)
      {
        pot.Showdown(_communityCards);
      }
    }

    private Pot CreatePot(string potType, List<Player> eligiblePlayers)
    {
      var pot = new Pot(_eventBroker, potType);
      pot.EligiblePlayers.AddRange(eligiblePlayers);
      return pot;
    }
  }
}
