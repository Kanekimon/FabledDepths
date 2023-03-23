using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CardDeck
{
    List<RoomCard> _cards = new List<RoomCard>();
    Stack<RoomCard> _playingDeck;

    string _playerId;
    string _deckName;

    public List<RoomCard> Cards { get { return _cards; } }
    public Stack<RoomCard> PlayingDeck { get { return _playingDeck; } }

    public string DeckName { get { return _deckName; } set { _deckName = value; } }
    public string PlayerId { get { return _playerId; } }

    public void ShuffleDeck()
    {
        List<RoomCard> cs = _cards.ToList();

        for (int i = cs.Count-1; i > 0; i--)
        {
            int random = UnityEngine.Random.Range(1, i);

            RoomCard tmp = cs[i];
            cs[i] = cs[random];
            cs[random] = tmp;
        }

        _playingDeck = new Stack<RoomCard>(cs);
    }


    public List<RoomCard> DrawCards(int numberOfCards)
    {
        List<RoomCard> drawn = new List<RoomCard>();
        for (int i = 0; i < numberOfCards; i++)
        {
            RoomCard cardDrawn = _playingDeck.Pop();

            // Move the drawn card to the bottom of the deck
            Stack<RoomCard> tempStack = new Stack<RoomCard>();
            tempStack.Push(cardDrawn);
            while (_playingDeck.Count > 0)
            {
                tempStack.Push(_playingDeck.Pop());
            }
            _playingDeck = tempStack;

            drawn.Add(cardDrawn);
        }
        return drawn;
    }


    public List<RoomCard> GetCardsOrderedByName()
    {
        return _cards.ToList().OrderBy(c => c.Name).ToList();
    }

    public void AddCard(RoomCard card)
    {
        _cards.Add(card);
    }

    public void AddCards(List<RoomCard> cards)
    {
        _cards.AddRange(cards);
    }
}

