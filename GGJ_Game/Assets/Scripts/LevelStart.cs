using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour
{
    public Transform startingPosition;
    public GameObject startVcam1;
    public GameObject startVcam2;

    [SerializeField] int numLevelSections;
    [SerializeField] float xSpawnOffset;

    [SerializeField] GameObject startingChunk;
    [SerializeField] GameObject endChunkPrefab;

    [SerializeField] GameObject[] levelChunkPrefabs;

    // Start is called before the first frame update
    void Awake()
    {
        GameManager.instance.initializeLevel(startingPosition.position, startVcam1, startVcam2);
        generateLevel();
    }

    private void generateLevel()
    {
        Vector3 spawnPos = startingChunk.transform.position;

        List<GameObject> availableChunks = new List<GameObject>();

        foreach (GameObject chunk in levelChunkPrefabs)
        {
            availableChunks.Add(chunk);
        }

        for (int i = 0; i < numLevelSections; i++)
        {
            if(availableChunks.Count == 0)
            {
                break;
            }

            spawnPos.x += xSpawnOffset;

            int randIndex = Random.Range(0, availableChunks.Count);
            Instantiate(availableChunks[randIndex], spawnPos, Quaternion.identity);
            availableChunks.RemoveAt(randIndex);
        }

        spawnPos.x += xSpawnOffset;
        Instantiate(endChunkPrefab, spawnPos, Quaternion.identity);
    }
}
