using System;
using System.Collections;
using System.Collections.Generic;
using Item;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton //
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject playerPrefab;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start()
    {
        DungeonGeneration.Instance.OnLevelGenerationComplete += SpawnPlayer;
        DungeonGeneration.Instance.OnLevelGenerationComplete += (f) => Physics.autoSimulation = true;
        DungeonGeneration.Instance.OnLevelGenerationComplete += SpawnItems;
    }

    private void SpawnPlayer(Vector3 spawnPos)
    {
        Instantiate(playerPrefab, spawnPos, Quaternion.identity);
    }

    private void SpawnItems(Vector3 spawnPos)
    {
        ItemManager.Instance.SpawnItem("Axe", new Vector3(0, 2, -2), false, 1);
    }
}
