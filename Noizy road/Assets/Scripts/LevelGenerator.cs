using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private int maxDifference;
    [SerializeField] private int maxAmountOfTerrainPrefabs;
    [SerializeField] private GameObject player;
    [SerializeField] private List<LevelData> terrainData = new List<LevelData>();

    private List<GameObject> currentTerrainList = new List<GameObject>();
    private int currentAmountOfTerrainPrefabs;
    private int prefabCounter;

    public Vector3 currentPosition;

    private void Start()
    {
        prefabCounter = 0;

        for (int i = 0; i < maxAmountOfTerrainPrefabs; i++)
        {
            SpawnTerrainPrefab(true, currentPosition);
        }
        maxAmountOfTerrainPrefabs = currentTerrainList.Count;
    }

    public void SpawnTerrainPrefab(bool isStart, Vector3 playerPosition)
    {
        if((currentPosition.z - playerPosition.z < maxDifference) || isStart)
        {
            int whichTerrainPrefab = Random.Range(0, terrainData.Count);
            int terrainInSuccess = Random.Range(1, terrainData[whichTerrainPrefab].maxSpawnAmount);

            for (int i = 0; i < terrainInSuccess; i++)
            {
                GameObject newTerrain = ObjectPoolScript.Spawn(terrainData[whichTerrainPrefab].terrainPrefab, currentPosition, Quaternion.identity);
                currentTerrainList.Add(newTerrain);

                if (!player.activeSelf)
                {
                    if (terrainData[whichTerrainPrefab].terrainPrefab.name == "Earth")
                    {
                        if (prefabCounter >= 5)
                        {
                            player.transform.position = new Vector3(currentPosition.x, currentPosition.y + 1.5f, currentPosition.z);
                            player.SetActive(true);
                        }

                        prefabCounter++;
                    }
                }

                if(!isStart)
                {
                    if(currentTerrainList.Count > maxAmountOfTerrainPrefabs)
                    {
                        ObjectPoolScript.Despawn(currentTerrainList[0]);
                        currentTerrainList.RemoveAt(0);
                    }
                }
                currentPosition.z++;
            }
        }
    }
}
