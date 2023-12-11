using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Pathfinding : MonoBehaviour
{
    [SerializeField] private bool coop;
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    private Vector2 target;
    [SerializeField] private float speed;
    private int mapWidth = 50;
    private int mapHeight = 50;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private List<Transform> enemies;
    [SerializeField] private List<Transform> players;

    // Start is called before the first frame update
    void Start()
    {
        Pathfind();
    }
    /// <summary>
    /// uses move towards to chase the closest player
    /// </summary>
    private void Pathfind()
    {
        mapObjects[,] grid = CreateGrid();
        for (int i = 0; i < mapWidth; i++) 
        {
            for (int j = 0; j < mapHeight; j++)
            {
                Debug.Log(grid[i, j]);
            }
        }


    }
    private mapObjects[,] CreateGrid()
    {
        mapObjects[,] grid = new mapObjects[mapWidth, mapHeight];
        Vector2Int[] obsticalpos = GetObstacleArrayPositions();
        Vector2Int[] enemypos = GetRoundedPositions(enemies);
        Vector2Int[] playerpos = GetRoundedPositions(players);
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                bool found = false;
                foreach (Vector2Int pos in obsticalpos) 
                {
                    if (pos == new Vector2Int(x, y))
                    {
                        grid[x, y] = mapObjects.Blocker;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    grid[x, y] = mapObjects.Empty;
                }


            }
            
        }
        foreach(Vector2 pos in enemypos)
        {
            grid[(int)pos.x, (int)pos.y] = mapObjects.Enemy;
        }
        foreach (Vector2 pos in playerpos)
        {
            grid[(int)pos.x, (int)pos.y] = mapObjects.Player;
        }
        return grid;

    }
    private Vector2Int[] GetObstacleArrayPositions()
    {
        Vector2Int[] positions = new Vector2Int[5];
       for (int i = 0; i < 5; i++)
        {
            Vector2 pos = new Vector2(Random.Range(-25, 25) + 0.5f, Random.Range(-25, 25) + 0.5f);
            Instantiate(obstacle, pos, Quaternion.identity);
            positions[i] = new Vector2Int(Mathf.RoundToInt(pos.x +25 - 0.5f), Mathf.RoundToInt(pos.y + 25 - 0.5f));
            
        }
       return positions;
    }
    private Vector2Int[] GetRoundedPositions(List<Transform> Objects)
    {
        Vector2Int[] positions = new Vector2Int[Objects.Count];
        int i = 0;
        foreach(Transform obj in Objects)
        {
            Vector2Int pos = new Vector2Int(Mathf.RoundToInt(obj.position.x + 25), Mathf.RoundToInt(obj.position.y + 25));
            positions[i] = pos;
            i++;

        }
        return positions;

        
    }

    private enum mapObjects 
    {
        Empty,
        Enemy,
        Blocker,
        Player
    }

    // Update is called once per frame
    void Update()
    {

    }
}
