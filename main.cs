// TO-DO:
// Sacrifice System - This'll be hard - DONE!
// Reformat cards to match ideas - medium
// get on with the rest of the project - really hard
// make a map generator - DONE!
internal class Program
{
    private static void Main(string[] args)
    {
            Deck deck = new Deck();
            Game game = new Game();
            Map map = new Map();
            deck.ShowDeck("No Pause");
            deck.ShowHand("No Pause");
            deck.UnlockCard(004);
            deck.AddCard(004, deck.deck);
            deck.BattleSetup(4);
            deck.ShowDeck("No Pause");
            deck.ShowHand("No Pause");
            game.CardSelect(deck);
            game.Battle(deck, 5, 3);
    }
}