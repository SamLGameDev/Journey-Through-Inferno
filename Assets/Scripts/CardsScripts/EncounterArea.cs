using Fungus;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EncounterArea : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    public Counter<GameObject> Enemys;
    public string dislayText;
    [SerializeField]
    private GameObject Canvas;
    [SerializeField]
    private TextMeshProUGUI textbox;
    [SerializeField]
    private GameObject box;
    private bool hasTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject enemy in Enemys.GetItems())
        {
            if (enemy == null) continue;
            EnablerAStar(false, enemy);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered && Canvas != null)
        {
            Canvas.SetActive(true);
            hasTriggered = true;
            GameManager.instance._events.GetComponent<EventSystem>().SetSelectedGameObject(box);
            Time.timeScale = 0;
        }
        else if (collision.CompareTag("Player") && !hasTriggered)
        {
            foreach (GameObject enemy in Enemys.GetItems())
            {
                if (enemy == null || enemy.GetComponent<EntityHealthBehaviour>().isBoss)
                {
                    continue;
                }
                EnablerAStar(true, enemy);
            }
        }


    }
    public void ActivateEnemy()
    {
        foreach (GameObject enemy in Enemys.GetItems())
        {
            if (enemy == null || enemy.GetComponent<EntityHealthBehaviour>().isBoss)
            {
                continue;
            }
            EnablerAStar(true, enemy);
        }
        Canvas.SetActive(false);
        Time.timeScale = 1;
        
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
