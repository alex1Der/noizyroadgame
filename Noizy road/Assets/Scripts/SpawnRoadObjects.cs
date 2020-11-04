using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoadObjects : MonoBehaviour
{
    [SerializeField] private LevelData roadData;
    [SerializeField] private int xSpawnOffset;
    [SerializeField] private float ySpawnOffset;
    [SerializeField] private float zSpawnOffset;
    [SerializeField] private int minSpawnValue;
    [SerializeField] private int maxSpawnValue;

    private Vector3 currentSpawnPosition;
    private Quaternion currentSpawnRotation;
    private float zRotationOffset;
    private float xRotationOffset;
    private float yRotationOffset;

    private void OnEnable()
    {
        zRotationOffset = 180f;
        xRotationOffset = 180f;
        xRotationOffset = 180f;
        yRotationOffset = 90f;
        roadData.currentSpawnSide = Random.Range(0, 2) * 2 - 1; // generate 1 or -1
        currentSpawnPosition = new Vector3(xSpawnOffset * roadData.currentSpawnSide, transform.position.y + ySpawnOffset, transform.position.z + zSpawnOffset);
        if (roadData.currentSpawnSide == -1)
            currentSpawnRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z - zRotationOffset, transform.rotation.w);
        else
            currentSpawnRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z + zRotationOffset, transform.rotation.w); ;

        StartCoroutine("SpawnCars");
    }

    IEnumerator SpawnCars()
    {
        while (true)
        {
            int whichPrefab = Random.Range(0, roadData.spawnObjects.Count);
            ObjectPoolScript.Spawn(roadData.spawnObjects[whichPrefab], currentSpawnPosition, currentSpawnRotation);
            yield return new WaitForSeconds(Random.Range(minSpawnValue, maxSpawnValue));
        }
    }
}
