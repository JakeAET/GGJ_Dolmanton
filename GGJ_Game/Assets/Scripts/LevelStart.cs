using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour
{
    public Transform startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.initializeLevel(startingPosition.position);
    }
}