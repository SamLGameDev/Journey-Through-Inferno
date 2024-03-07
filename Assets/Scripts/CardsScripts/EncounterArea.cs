using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterArea : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    public static List<GameObject> Enemys;
    // Start is called before the first frame update
    void Start()
    {
       Enemys = new List<GameObject>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(GameObject enem in Enemys)
        {
            if (enem == null) continue;
            if (enem.name == collision.name) return;
        }
        if (collision.CompareTag("Enemy"))
        {
            Enemys.Add(collision.gameObject);
            GameManager.instance.enemiesRemaining++;
            if (collision.GetComponent<EntityHealthBehaviour>().isBoss) return;
            EnablerAStar(false, collision.gameObject);
            return;
        }
        if (collision.CompareTag("Player"))
        {
            foreach(GameObject enemy in Enemys)
            {
                if (enemy == null || enemy.GetComponent<EntityHealthBehaviour>().isBoss)
                {
                    continue;
                }
                EnablerAStar(true, enemy);
            }
        }
    }
    private void EnablerAStar(bool setter, GameObject entity)
    {
        entity.GetComponent<Seeker>().enabled = setter;
        entity.GetComponent<AIDestinationSetter>().enabled = setter;
        entity.GetComponent<AIPath>().enabled = setter;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
