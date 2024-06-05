using System.Collections.Generic;
using System;

public class Map
{
    public List<string> availableNodes;
    public List<string> map;
    public Random rand = new Random();
    public int mapNum = 0;

    public Map()
    {
        availableNodes = new List<string> {
    "Unlock and Add Card","Card Select","Battle"
    };
    }

    public void MapGenerate(int mapLength)
    {
        mapNum += 1;
        int i = 0;
        map.Clear();
        while (i < mapLength)
        {
            int e = rand.Next(0, availableNodes.Count);
            map.Add(availableNodes[e]);
            i++;
        }
    }

    public void UseMap(Deck deck, Game game)
    {
        if (map[0] == "Unlock and Add Card")
        {
            deck.UnlockCard(deck.unlockedcards.Count);
            deck.AddCard(deck.unlockedcards.Count, deck.deck);
        }

        if (map[0] == "Card Select")
        {
          game.CardSelect(deck);
        }

        if (map[0] == "Battle")
        {
            int x = mapNum + 2;
            game.Battle(deck,(int)Math.Pow(x,1.35),(int)Math.Pow(x,1.15));
        }
    }
}