using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class regeneration : MonoBehaviour
{
    [SerializeField]
    private GameEvent onRegen;
    [SerializeField]
    private GameEvent OnExit;
    [SerializeField]
    private Counter<GameObject> playerInstances;

    private void OnTriggerEnter2D(Collider2D collision)
    { 
       if (collision != transform.parent && (bool)HasCard(TarotCards.possibleModifiers.HealthRegen)[0])
        {
            Debug.Log("Regen Start");
            onRegen.Raise();
        } 
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        OnExit.Raise();
    }
    private object[] HasCard(TarotCards.possibleModifiers modifier)
    {
        try
        {
            foreach (GameObject Player in playerInstances.GetItems())
            {
                foreach (TarotCards card in Player.GetComponent<Player_movement>().stats.tarotCards)
                {
                    if (card.possibleMods == modifier)
                    {
                        return new object[] { true, card };
                    }
                }

            }
        }
        catch
        {
            return new object[] { false };
        }
        return new object[] { false };
    }
}
