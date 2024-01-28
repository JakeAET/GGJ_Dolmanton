using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(startCamFunction());
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && GameManager.instance.currentTurn != GameManager.turn.Win)
        {
            GameManager.instance.goalReached(collision.GetComponentInParent<Player>().playerName);
        }
    }

    IEnumerator startCamFunction()
    {
        CamController cam = FindObjectOfType<CamController>();

        Vector3 pos = transform.position;
        cam.virtualCam3.transform.position = pos;

        cam.virtualCam3.LookAt = transform;
        cam.virtualCam3.Follow = transform;

        yield return new WaitForSeconds(2f);

        cam.switchCam(GameManager.instance.activePlayer.playerName);
        cam.zoomInZoomOut(5);

        yield return null;
    }
}
