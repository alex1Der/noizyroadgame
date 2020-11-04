using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRiverObjects : MonoBehaviour
{
    [SerializeField] private LevelData riverData;
    [SerializeField] private int xSpawnOffset;
    [SerializeField] private int minSpawnValue;
    [SerializeField] private int maxSpawnValue;
    private Vector3 currentSpawnPosition;

    private void OnEnable()
    {
        riverData.currentSpawnSide *= -1;
        currentSpawnPosition = new Vector3(xSpawnOffset * riverData.currentSpawnSide, transform.position.y + 0.125f, transform.position.z);

        StartCoroutine("SpawnDesks");
    }

    IEnumerator SpawnDesks()
    {
        while (true)
        {
            int whichPrefab = Random.Range(0, riverData.spawnObjects.Count);
            ObjectPoolScript.Spawn(riverData.spawnObjects[whichPrefab], currentSpawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(minSpawnValue, maxSpawnValue));
        }
    }
}
