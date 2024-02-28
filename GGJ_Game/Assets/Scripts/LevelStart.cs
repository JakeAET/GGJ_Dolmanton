using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour
{
    public Transform startingPosition;
    public Transform levelEdgePos;

    public GameObject[] startVcams;
    public GameObject levelVcam;
    public GameObject tutorial;

    //public GameObject startVcam1;
    //public GameObject startVcam2;
    //public GameObject startVcam3;
    //public GameObject startVcam4;
    public GameObject camController;

    [SerializeField] int numLevelSections;
    [SerializeField] float xSpawnOffset;

    [SerializeField] GameObject startingChunk;
    [SerializeField] GameObject endChunkPrefab;

    [SerializeField] GameObject[] levelChunkPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.initializeLevel(startingPosition.position, startVcams, camController, tutorial);
        GameManager.instance.levelEdgePos = levelEdgePos.transform.position;
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
