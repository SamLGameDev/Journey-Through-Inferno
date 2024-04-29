using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DropTarotCard : MonoBehaviour
{
    public BasicAttributes attributes;

    public void DropCard(Player Killer)
    {
   
        if (Random.Range(0.0001f, 101) < attributes.cardDropChance + Killer.cardDropChance)
        {
            Debug.Log("here");
            TarotCards card;
            int i = 0;
            do
            {
                card = attributes.droppableCards.ElementAt(Random.Range(0, attributes.droppableCards.Count));
                i++;
            }
            while (Killer.droppableCards.Contains(card) || Killer.tarotCards.Contains(card) || i == 1000);
            Killer.droppableCards.Add(card);
        }
    }

}
