using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class Game
{
  public List<int> randomList = new List<int>();
  public string[] CardSlot1 = new string[] {null,"0"};
  public string[] CardSlot2 = new string[] {null,"0"};
  public string[] CardSlot3 = new string[] {null,"0"};
  public string[] CardSlot4 = new string[] {null,"0"};
  public string[] CardtobePlayed = new string[] {};

  public void CardSelect(Deck deck)
    {
      deck.ReturnCardsToDeck();
      for (int i = 0; i < 3;)
      {
        int RandNum = deck.rand.Next(1,deck.unlockedcards.Count);
        if (!randomList.Contains(RandNum))
          {
          randomList.Add(RandNum);
          i++;  
          }
      }
      string[] card1 = deck.unlockedcards[randomList[0]];
      string[] card2 = deck.unlockedcards[randomList[1]];
      string[] card3 = deck.unlockedcards[randomList[2]];
      string card1info = string.Join(", ", card1.Where(item => item != null && item != card1[6] && item != "None").ToArray());
      string card2info = string.Join(", ", card2.Where(item => item != null && item != card2[6] && item != "None").ToArray());
      string card3info = string.Join(", ", card3.Where(item => item != null && item != card3[6] && item != "None").ToArray());
      Console.Clear();
      Console.WriteLine("You have these cards in your deck:");
      deck.ShowDeck("No Clear");
      Console.WriteLine($"\nYou have reached a card select! Here are your three cards to choose from: \n1. {card1info} \n2. {card2info} \n3. {card3info} \nType the number of the card you would like to select:");
      int CardID = randomList[(Convert.ToInt32(Console.ReadLine())-1)];
      Console.Clear();
      deck.AddCard(CardID,deck.deck);
    }
  public void Battle(Deck deck, int enemyHealth, int turns)
    {
      Console.Clear();
      bool BattleWon = false;
      deck.BattleSetup(4);
      Console.WriteLine($"You have entered a battle!\nYou have to deal {enemyHealth} damage in {turns} turns!\nYou have these cards in your hand:");
      deck.ShowHand("No Clear");
      while (BattleWon == false)
      {
        bool EndTurn = false;
        Console.WriteLine($"Do you want to draw from your deck ({deck.deck.Count} cards left) or your freedeck ({deck.freedeck.Count} cards left)?");
        string deckDraw = Console.ReadLine().ToLower();
        if (deckDraw == "deck" && deck.deck.Count > 0)
        {
          deck.DrawCard(deck.deck);
        }
        else if (deckDraw == "freedeck" && deck.freedeck.Count > 0)
        {
          deck.DrawCard(deck.freedeck);
        }
        while (EndTurn == false)
        {
          Console.Clear();
          Console.WriteLine("You now have these cards in your hand:");
          deck.ShowHand("No Clear");
          Console.WriteLine("What do you want to do? Here are your options: \n1. Play a card \n2. End your turn\nType the number of the action you want to do:");
          string action = Console.ReadLine().ToLower();
          Console.Clear();
          if ((action == "1" || action.Contains("play") || action.Contains("card")) && CardSlot1 != null && CardSlot2 != null && CardSlot3 != null && CardSlot4 != null)
          {
            deck.PlayCard(this);
          }
          if (action == "2" || action.Contains("End") || action.Contains("Turn")) 
          {
            enemyHealth = CalculateDamage(enemyHealth);
            EndTurn = true;
            turns -= 1;
          }
        }
        if (enemyHealth <= 0)
        {
          BattleWon = true;
        }
        if (turns <= 0)
        {
          break;
        }
      }
      if (BattleWon == false)
      {
        Console.WriteLine("You Lost!");
        Environment.Exit(143);
      }
      else 
      {
        Console.WriteLine("You Won!");
      }
    }
  public int CalculateDamage(int enemyHealth)
    {
      if (CardSlot1[0] != null && CardSlot1[1] != "0") 
      {
        Console.WriteLine($"{CardSlot1[0]} dealt {CardSlot1[1]} damage!");
      }
      if (CardSlot2[0] != null && CardSlot2[1] != "0") 
      {
        Console.WriteLine($"{CardSlot2[0]} dealt {CardSlot2[1]} damage!");
      }
      if (CardSlot3[0] != null && CardSlot3[1] != "0") 
      {
        Console.WriteLine($"{CardSlot3[0]} dealt {CardSlot3[1]} damage!");
      }
      if (CardSlot4[0] != null && CardSlot4[1] != "0") 
      {
        Console.WriteLine($"{CardSlot4[0]} dealt {CardSlot4[1]} damage!");
      }
      int damage = Convert.ToInt32(CardSlot1[1]) + Convert.ToInt32(CardSlot2[1]) + Convert.ToInt32(CardSlot3[1]) + Convert.ToInt32(CardSlot4[1]);
      Console.WriteLine($"You dealt {damage} total damage.");
      enemyHealth -= damage;
      return enemyHealth;
    }
  public bool CheckCardSacrifice(int numberOfCard, Deck deck)
    {
      Console.Clear();
      string[] Card = deck.hand[numberOfCard];
      int SacNeeded = Convert.ToInt32(Card[4]);
      int SacGot = 0;
      int TotalSac = 0;
      foreach (var card in deck.hand)
      {
        TotalSac += Convert.ToInt32(card[5]);
      }
      TotalSac -= Convert.ToInt32(Card[5]);
      if (TotalSac >= SacNeeded)
      {
        string[] CardtobePlayed = deck.hand[numberOfCard];
        deck.hand.RemoveAt(numberOfCard);
        while (SacNeeded > SacGot)
        {
          string result = (SacNeeded != 1) ? $"You currently require {SacNeeded} sacrifices but you have only gotten " : $"You currently require {SacNeeded} sacrifice but you have only gotten ";
          string result1 = (SacGot != 1) ?  $"{SacGot} sacrifices." : $"{SacGot} sacrifice.";
          Console.WriteLine(String.Join("", result,result1));
          deck.ShowHand("No Clear"); //Improve error handling and remove the chosen card
          Console.WriteLine("What number card would you like to choose to sacrifice?");
          int CardID = Convert.ToInt32(Console.ReadLine());
          string[] card = deck.hand[CardID-1];
          SacGot += Convert.ToInt32(card[5]);
          deck.ReturnCardToDeck(CardID);
        }
        return true;
      }
      else 
      {
        Console.WriteLine("Sorry, but you do not have enough available sacrifice to play this card.");
        Thread.Sleep(3000);
        return false;
      }
    }
}