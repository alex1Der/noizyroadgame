using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "LevelData")]
public class LevelData : ScriptableObject
{
    public GameObject terrainPrefab;
    public int maxSpawnAmount;
    public float currentSpawnSide;
    public List<GameObject> spawnObjects = new List<GameObject>();
}
