using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnEarthObjects : MonoBehaviour
{
    [SerializeField] private LevelData earthData;
    [SerializeField] private GameObject coin;
    [SerializeField] private GameObject giganticTube;
    [SerializeField] private int minSpawnAmount;
    [SerializeField] private int maxSpawnAmount;
    [SerializeField] private int startSpawnPoint;
    [SerializeField] private int endSpawnPoint;
    [SerializeField] private int giganticTubeSpawnPosition;

    private List<int> limitPositions;
    private List<int> currentSpawnPositions;
    private List<GameObject> earthObjects;

    private void OnEnable()
    {
        earthObjects = new List<GameObject>();
        currentSpawnPositions = new List<int>();
        limitPositions = new List<int>
        {
            giganticTubeSpawnPosition,
            giganticTubeSpawnPosition * -1
        };

        int whichPrefab = Random.Range(0, earthData.spawnObjects.Count);
        int amountOfSpawnObjects = Random.Range(minSpawnAmount, maxSpawnAmount);
        for (int i = 0; i < amountOfSpawnObjects; i++)
        {
            currentSpawnPositions.Add(Random.Range(startSpawnPoint, endSpawnPoint));
        }

        currentSpawnPositions.Distinct().ToList();

        int randomCoinPosition = Random.Range(startSpawnPoint, endSpawnPoint);
        if (!currentSpawnPositions.Contains(randomCoinPosition))
        {
            earthObjects.Add(ObjectPoolScript.Spawn(coin, new Vector3(randomCoinPosition, transform.position.y + 0.25f, transform.position.z), Quaternion.identity));
        }

        foreach (var n in limitPositions)
        {
            earthObjects.Add(ObjectPoolScript.Spawn(giganticTube, new Vector3(n, transform.position.y + 1f, transform.position.z), giganticTube.transform.rotation));
        }

        foreach (var n in currentSpawnPositions)
        {
            earthObjects.Add(ObjectPoolScript.Spawn(earthData.spawnObjects[whichPrefab], new Vector3(n, transform.position.y + 1f, transform.position.z), earthData.spawnObjects[whichPrefab].transform.rotation));
        }
    }

    private void OnDestroy()
    {
        foreach (var n in earthObjects)
            ObjectPoolScript.Despawn(n);
    }

    private void OnDisable()
    {
        foreach (var n in earthObjects)
            ObjectPoolScript.Despawn(n);
    }
}