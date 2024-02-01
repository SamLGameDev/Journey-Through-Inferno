using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterArea : MonoBehaviour
{
    List<GameObject> Enemys;
    // Start is called before the first frame update
    void Start()
    {
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemys.Add(collision.gameObject);
            EnablerAStar(false, collision.gameObject);
            return;
        }
        if (collision.CompareTag("Player"))
        {
            foreach(GameObject enemy in Enemys)
            {
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
