using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Room : MonoBehaviour
{
    [SerializeField] private int minEnemyCount = 3;
    [SerializeField] private int maxEnemyCount = 5;
    [SerializeField] private List<GameObject> enemyPrefabs;
    
    [HideInInspector] public bool isColliding = false;
    public Exit exitToFit;
    public List<Corridor> corridors;

    private List<SpawnPoint> spawnPoints;
    
    public List<GameObject> enemies = new List<GameObject>();
    private bool isSpawned = false;
    private Door entranceDoor = null;
    
    private void Awake()
    {
        exitToFit = GetRandomExit();
        spawnPoints = GetComponentsInChildren<SpawnPoint>().ToList();
    }

    public void SpawnEnemies(Door ent)
    {
        if (isSpawned) return;
        
        isSpawned = true;

        entranceDoor = ent;
        
        foreach (var c in corridors)
        {
            c.entrance.GetComponent<Exit>().door.GetComponent<Door>().isLocked = true;
        }
        
        spawnPoints.Shuffle();
        for (int i = 0; i < Random.Range(minEnemyCount, maxEnemyCount + 1); i++)
        {
            var enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], spawnPoints[i].gameObject.transform.position,
                Quaternion.identity);
            enemy.GetComponent<EnemyStats>().room = this;
            enemies.Add(enemy);
        }
    }
    
    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);

        //Handle game flow when there are not any enemies
        if (enemies.Count == 0)
        {
            foreach (var c in corridors)
            {
                c.entrance.GetComponent<Exit>().door.GetComponent<Door>().isLocked = false;
            }
            
            entranceDoor.isLocked = false;
        }
    }
    /// <summary>
    /// Get all exits in a room
    /// </summary>
    /// <returns>All exit component in a room prefab</returns>
    public List<Exit> GetExits()
    {
        return GetComponentsInChildren<Exit>().ToList();    
    }

    /// <summary>
    /// Get an exit with a certain direction
    /// </summary>
    /// <param name="direction">Direction to get an exit with</param>
    /// <returns>A room with a certain direction</returns>
    public Exit GetRandomExit()
    {
        var exits = this.GetExits();
        return exits[Random.Range(0, exits.Count)];
    }

    private void OnCollisionStay(Collision other)
    {
        isColliding = true;
    }
    
    private void OnCollisionExit(Collision other)
    {
        isColliding = false;
    }
}