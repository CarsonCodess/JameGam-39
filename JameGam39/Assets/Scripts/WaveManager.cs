using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private WeightedObjectList<GameObject> enemies = new WeightedObjectList<GameObject>();
    [SerializeField] private List<Transform> spawnPositions = new List<Transform>();
    [SerializeField] private Vector2 randomSpawnTime;
    private int _enemiesThisRound;
    private int _enemiesKilled;
    private int _waveNumber;

    public void NewWave()
    {
        _waveNumber++;
        var enemiesPerRoundFunc = Mathf.RoundToInt(3f + 1.15f * _waveNumber * (float) Math.Pow(Math.E, -_waveNumber / 500f) + 4.5f * (float) Math.Sin(_waveNumber * math.PI / 25f));
        _enemiesThisRound = enemiesPerRoundFunc;
    }
    
    public void EnemyKilled()
    {
        _enemiesKilled++;
    }
}
