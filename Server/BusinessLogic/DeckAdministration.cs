using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Server.Models;
using Server.DAL;
using System.Text.Json;
using Server.Utility;

namespace Server.BusinessLogic
{
    class DeckAdministration
    {
        public static Deck getDeck(int userid)
        {
            Deck deck = DeckAccess.getDeck(userid);
            if(deck == null)
            {
                throw new InvalidInputException("Deck not found");
            }
            List<AbstractCard> cards = new List<AbstractCard>();

            List<int> cardsid = DeckAccess.getCardsInDeck(deck.Id);

            if(cardsid == null)
            {
                Console.WriteLine("No Cards in Deck");
                return deck;
            }
            foreach (int card in cardsid)
            {
                deck.addCard(CardAccess.getCard(card));
            }
            return deck;
        }

        public static string getDeckPlain(int userid)
        {
            Deck deck = DeckAccess.getDeck(userid);
            string s = "";
            if (deck == null)
            {
                throw new InvalidInputException("Deck not found");
            }
            List<AbstractCard> cards = new List<AbstractCard>();
            List<int> cardsid = DeckAccess.getCardsInDeck(deck.Id);

            s += deck.Name + ":\n";

            if (cardsid == null)
            {                
                return s;
            }
            foreach (int card in cardsid)
            {
                s += CardAccess.getCard(card).ToString() + "\n";
            }
            return s;
        }

        public static void changeDeck(int userid, List<int> cards)
        {
            foreach (int cardid in cards)
            {
                CardAccess.ownsCard(userid, cardid);
            }
            DeckAccess.removeCardsFromDeck(userid);
            int deckid = DeckAccess.getDeck(userid).Id;

            foreach(int cardid in cards)
            {
                DeckAccess.addCardToDeck(deckid, cardid, userid);
            }

        }
    }
}
