using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject player;
    [SerializeField]
    private GameEvent OnEnterCollider;
    [SerializeField]
    private GameEvent OnExitCollider;
    [SerializeField]
    private int ressurectedHealth;
    [SerializeField]
    private int CostOfReviving;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnEnterCollider.Raise();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("grave bugs");
        if (collision.CompareTag("Player"))
        {
            Debug.Log("bugssss");
            if (collision.GetComponent<Player_movement>().RevivePlayer)
            {
                Debug.Log("grave");
                player.SetActive(true);
                EntityHealthBehaviour health = player.GetComponent<EntityHealthBehaviour>();
                health.currentHealth = ressurectedHealth;
                OnExitCollider.Raise();
                if (player.name == "Player 2")
                {
                    EntityHealthBehaviour.player2.IsAlive = true;
                }
                else
                {
                    EntityHealthBehaviour.player1.IsAlive = true;
                }
                health = collision.GetComponent<EntityHealthBehaviour>();
                if (health.currentHealth <= CostOfReviving)
                {
                    health.currentHealth = 1;
                }
                else
                {
                    health.ApplyDamage(CostOfReviving);
                }
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        OnExitCollider.Raise();
    }
}
