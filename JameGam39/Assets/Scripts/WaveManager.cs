using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    
    public int EnemiesKilledTotal { get; private set; }
    [SerializeField] private WeightedObjectList<GameObject> enemies = new WeightedObjectList<GameObject>();
    [SerializeField] private List<Transform> spawnPositions = new List<Transform>();
    [SerializeField] private Vector2 randomSpawnTime;
    private int _enemiesThisRound;
    private int _enemiesKilled;
    private int _waveNumber;

    private void Awake()
    {
        _waveNumber = -1;
        instance = this;
        NewWave();
    }

    public void NewWave()
    {
        _enemiesKilled = 0;
        _waveNumber++;
        var enemiesPerRoundFunc = Mathf.RoundToInt(3f + 1.15f * _waveNumber * (float) Math.Pow(Math.E, -_waveNumber / 500f) + 4.5f * (float) Math.Sin(_waveNumber * math.PI / 25f));
        _enemiesThisRound = enemiesPerRoundFunc;
        for(var i = 0; i < _enemiesThisRound; i++)
            SpawnEnemy();
    }

    private void Update()
    {
        if (_enemiesKilled >= _enemiesThisRound)
            UpgradeScreen.instance.DisplayUpgradeScreen();
    }

    public void EnemyKilled()
    {
        _enemiesKilled++;
        EnemiesKilledTotal++;
    }

    public void SpawnEnemy()
    {
        var spawnIndex = Random.Range(0, spawnPositions.Count);
        if(_waveNumber >= 7)
            Instantiate(enemies.GetRandomObject(), spawnPositions[spawnIndex].transform.position, Quaternion.identity);
        else
            Instantiate(enemies.GetObject(0), spawnPositions[spawnIndex].transform.position, Quaternion.identity);
    }
}
