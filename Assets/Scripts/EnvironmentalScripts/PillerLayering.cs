using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillerLayering : MonoBehaviour
{
    [SerializeField]
    private Counter<GameObject> playerInstances;

    private List<GameObject> players;

    private SpriteRenderer sr;

    [SerializeField]
    private Counter<GameObject> pillars;
    // Start is called before the first frame update

    private void OnEnable()
    {
        pillars.Add(gameObject);
    }
    void Start()
    {
        players = new List<GameObject>(playerInstances.GetItems());
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        setPlayerRenderingRelationshipWithPillar();
    }

    private void setPlayerRenderingRelationshipWithPillar()
    {
        foreach (GameObject player in players)
        {
            GameObject closestPillar = getPillarClosestToPlayer(player);
            setSortOrderOfPlayer(player, closestPillar);
        }
    }

    private static void setSortOrderOfPlayer(GameObject player, GameObject closestPillar)
    {
        if (player.transform.position.y > closestPillar.transform.position.y)
        {
            player.GetComponent<SpriteRenderer>().sortingOrder = closestPillar.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }
        else
        {
            player.GetComponent<SpriteRenderer>().sortingOrder = closestPillar.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
    }

    private GameObject getPillarClosestToPlayer(GameObject player)
    {
        GameObject closestPillar = null;
        foreach (GameObject pillar in pillars.GetItems())
        {
            if (closestPillar == null || (player.transform.position - pillar.transform.position).magnitude < (player.transform.position - closestPillar.transform.position).magnitude)
            {
                closestPillar = pillar;
            }
        }

        return closestPillar;
    }
    private void OnDisable()
    {
        pillars.Remove(gameObject);
    }
}
