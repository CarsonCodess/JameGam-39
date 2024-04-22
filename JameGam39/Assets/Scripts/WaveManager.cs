using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private WeightedObjectList<GameObject> enemies = new WeightedObjectList<GameObject>();
    [SerializeField] private List<Transform> spawnPositions = new List<Transform>();
    [SerializeField] private Vector2 randomSpawnTime;
    private int _enemiesKilled;

}
