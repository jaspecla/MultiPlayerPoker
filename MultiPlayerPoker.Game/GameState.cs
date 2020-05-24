using MultiPlayerPoker.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiPlayerPoker.Game
{
  public class GameState
  {
    public Table GameTable { get; set; }
    public int NextCardInDeck { get; set; }
    public int BigBlind { get; set; }
    public int SmallBlind { get; set; }
    public int AmountToContinue { get; set; }
    public Card[] CardsInDeck { get; set; }
    public Card[] CommunityCards { get; set; }
    public List<Pot> Pots { get; set; }
    public Pot CurrentPot { get; set; }

    private readonly RecursiveHandEvaluator _handEvaluator;
    
    public GameState()
    {
      _handEvaluator = new RecursiveHandEvaluator();

      NextCardInDeck = 0;

      GameTable = new Table();
      CommunityCards = new Card[5];

      Pots = new List<Pot>();
    }

    public void InitializeGame()
    {
      ButtonIndex = 0;
    }

    public void BeginGame()
    {
      // TODO: Rotate button
      AmountToContinue = BigBlind;

      var mainPot = new Pot();
      mainPot.Initialize(Players);
      Pots.Add(mainPot);

      CurrentPot = mainPot;

      // TODO: Take blinds
      // TODO: Make player after blinds player to act
    }

    public void Shuffle()
    {
      CardsInDeck = Deck.Shuffle(); 
    }


    public void RemovePlayer(Player player)
    {
      player.RemoveFromGame();
      Players.Remove(player);
    }

    public void Bet(Player player, int bet)
    {
      if (player != PlayerToAct)
      {
        throw new BetException($"Player {player.Id} attempted to bet {bet} but it was not their turn to act.");
      }

      if (bet < player.ChipStack)
      {
        throw new BetException($"Player {player.Id} attempted to bet {bet} but they only had {player.ChipStack} in their chip stack.");
      }

      if ((bet + player.CurrentBet) < AmountToContinue)
      {
        if (bet < player.ChipStack)
        {
          throw new BetException($"Player {player.Id} attempted to bet {bet} for a total of {bet + player.CurrentBet} but {AmountToContinue} is required to continue and they are not all in.");
        }
        else // player is all-in
        {
          player.Bet(bet);
          CurrentPot.Value += bet;

          var newSidePot = new Pot();
          newSidePot.Initialize(Players);
          CurrentPot = newSidePot;
          Pots.Add(newSidePot);

          return;
        }
      }

      player.Bet(bet);
      CurrentPot.Value += bet;

      if (bet > AmountToContinue)
      {
        AmountToContinue = bet;
      }
    }

    public void Fold(Player player)
    {
      player.Fold();
    }

    public void Showdown()
    {
      foreach (var pot in Pots)
      {
        var winners = pot.GetWinners(CommunityCards);
        foreach (var winner in winners)
        {
          winner.ChipStack += (pot.Value / winners.Count);
        }
      }
    }

    private void NextPlayer()
    {

    }

  }
}
