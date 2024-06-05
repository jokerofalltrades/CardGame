using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class Deck 
{
  // Loads all variables to the instance of Deck
  public List<string[]> deck;
  public List<string[]> unlockedcards;
  public List<string[]> freedeck;
  public List<string[]> hand;
  public List<string[]> totalcards;
  public Random rand = new Random();
  // Initialising Script
  public Deck()
    {
      InitialiseLists();
      InitialiseFreeDeck();
      InitialiseDeck();
      InitialiseHand(4);
    }
  // Script Initalsing the Lists
  public void InitialiseLists() 
    {
      // Defines all lists
        deck = new List<string[]> {};
        freedeck = new List<string[]> {};
        hand = new List<string[]> {};
        //All Cards are formatted Name(0), Attack(1), Health(2), Ability(3), Sac Req(4), Sac Worth (5), CardID(6)
        unlockedcards = new List<string[]> {};
        totalcards = new List<string[]>
        {
          new string[] { "Duck","0","1","None","0","1","000" },
          new string[] { "Pig","1","3","None","1","1","001" },
          new string[] { "Horse","2","2","None","2","1","002" },
          new string[] { "Swan","1","5","Distracting","2","1","003" },
          new string[] { "Rat","2","1","Infectious","2","1","004" }
        };
    }
  // Script Initalsing Deck
  public void InitialiseDeck()
    {
      UnlockCard(001);
      UnlockCard(002);
      UnlockCard(003);
      AddCard(001,deck);
      AddCard(002,deck);
      AddCard(003,deck);
    }
  // Script Initalsing freedeck
  public void InitialiseFreeDeck()
    {
        UnlockCard(000);
        // Freedeck contains only the duck
        for (int i = 0; i < 8; i++)
        {
          AddCard(000,freedeck);
        }
    }
  // Script Initalsing Hand
  public void InitialiseHand(int HandSize)
    {
        // Hand is initailised using what cards are in deck
        for (int i = 0; i < HandSize; i++)
        {
          int e = rand.Next(0,deck.Count);
          int v = rand.Next(0,freedeck.Count);
          if (i < HandSize/3) 
            {
            hand.Add(freedeck[v]);
            freedeck.RemoveAt(v);
            }
          else 
            {
            hand.Add(deck[e]);
            deck.RemoveAt(e);
            }
        }
    }
  // Script printing out the list 'cardList', enumerated
  private void DisplayCards(List<string[]> cardList) 
    {
      int index = 1;
      foreach (var card in cardList) 
      {
        string cardDetails = string.Join(", ", card.Where(item => item != "None" && item != card[6]));
        Console.WriteLine($"{index}. {cardDetails}");
        index++;
      }
    }
  // Script printing out the list hand, enumerated
  public void ShowHand(string noPause = null)
    {
      if (noPause != "No Clear")
      {
        Console.Clear();
      }
      DisplayCards(hand);
      if (noPause == null)
      {
        Thread.Sleep(5000);
      }
    }
  // Script printing out the list deck, enumerated
  public void ShowDeck(string noPause = null)
    {
      if (noPause != "No Clear")
      {
        Console.Clear();
      }
      DisplayCards(deck);
      if (noPause == null)
      {
        Thread.Sleep(5000);
      }
    }
  // Script adding to the eligible card pool
  public void UnlockCard(int CardID) 
    {
      if (CardID < totalcards.Count && !(unlockedcards.Contains(totalcards[CardID])))
      {
      unlockedcards.Add(totalcards[CardID]);
      }
    }
  // Script adding a card to the deck
  public void AddCard(int CardID, List<string[]> cardList) 
    {
      if (unlockedcards.Contains(totalcards[CardID]))
      {
      cardList.Add(totalcards[CardID]);
      }
    }
  // Script Drawing a card
  public void DrawCard(List<string[]> cardList)
    {
      int randomCard = rand.Next(0,cardList.Count);
      hand.Add(cardList[randomCard]);
      cardList.RemoveAt(randomCard);
    }
  // Script resetting up the hand and decks
  public void BattleSetup(int handSize)
    {
      ReturnCardsToDeck();
      InitialiseHand(handSize);
    }
  // Script returning Cards to the Deck
  public void ReturnCardsToDeck() 
    {
      foreach (var card in hand)
      {
        if (card[0] != "Duck")
        {
          deck.Add(card);
        }
        else 
        {
          freedeck.Add(card);
        }
      }
      hand.Clear();
    }

  public void ReturnCardToDeck(int cardID) 
  {
    {
      string[] card = hand[cardID-1];
      if (card[0] != "Duck")
      {
        deck.Add(card);
      }
      else 
      {
        freedeck.Add(card);
      }
    }
    hand.RemoveAt(cardID-1);
  }
  
  public void PlayCard(Game game)
    {
      Console.Clear();
      string result1 = (game.CardSlot1[0] != null) ? game.CardSlot1[0] : "Empty";
      string result2 = (game.CardSlot2[0] != null) ? game.CardSlot2[0] : "Empty";
      string result3 = (game.CardSlot3[0] != null) ? game.CardSlot3[0] : "Empty";
      string result4 = (game.CardSlot4[0] != null) ? game.CardSlot4[0] : "Empty";
      Console.WriteLine($"Card Slot 1: {result1}\nCard Slot 2: {result2}\nCard Slot 3: {result3}\nCard Slot 4: {result4}\n\nThese are the cards you have in your hand:");
      ShowHand("No Clear");
      Console.WriteLine("What number card would you like to choose?");
      int numberOfCard = Convert.ToInt32(Console.ReadLine()) - 1;
      Console.WriteLine("What card slot would you like to put it in?");
      int cardSlotNum = Convert.ToInt32(Console.ReadLine());
      if (cardSlotNum == 1 && result1 == "Empty")
      {
        bool cardPlayed = game.CheckCardSacrifice(numberOfCard,this);
        if (cardPlayed == true)
        {
          game.CardSlot2 = game.CardtobePlayed;
          Array.Clear(game.CardtobePlayed);
        }
      }
      else if (cardSlotNum == 2 && result2 == "Empty")
      {
        bool cardPlayed = game.CheckCardSacrifice(numberOfCard,this);
        if (cardPlayed == true)
        {
          game.CardSlot2 = game.CardtobePlayed;
          Array.Clear(game.CardtobePlayed);
        }
      }
      else if (cardSlotNum == 3 && result3 == "Empty")
      {
        bool cardPlayed = game.CheckCardSacrifice(numberOfCard,this);
        if (cardPlayed == true)
        {
          game.CardSlot3 = game.CardtobePlayed;
          Array.Clear(game.CardtobePlayed);
        }
      }
      else if (cardSlotNum == 4 && result4 == "Empty")
      {
        bool cardPlayed = game.CheckCardSacrifice(numberOfCard,this);
        if (cardPlayed == true)
        {
          game.CardSlot4 = game.CardtobePlayed;
          Array.Clear(game.CardtobePlayed);
        }
      }
    }
}