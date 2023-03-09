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
        for(int i = 0; i < numberOfCards; i++)
        {
            RoomCard cardDrawn = _playingDeck.Pop();
            drawn.Add(cardDrawn);
            Stack<RoomCard> tmp = new Stack<RoomCard>(_playingDeck.Reverse());
            _playingDeck.Clear();
            _playingDeck.Push(cardDrawn);
            while (tmp.Count > 0)
                _playingDeck.Push(tmp.Pop());
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

}

