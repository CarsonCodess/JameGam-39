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
        for(int i = 0; i < _enemiesThisRound; i++){
            SpawnEnemy();
        }
    }
    
    public void EnemyKilled()
    {
        _enemiesKilled++;
    }

    public void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPositions.Count-1);
        Vector2 enemyPosition = spawnPositions[spawnIndex].transform.position;
        GameObject enemySpawn = enemies.GetRandomObject();
        Instantiate(enemySpawn, enemyPosition, Quaternion.identity);

    }
}
