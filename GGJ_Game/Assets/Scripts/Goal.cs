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
        if (collision.CompareTag("Player") && GameManager.instance.turnBasedActive)
        {
            GameManager.instance.goalReached(collision.GetComponentInParent<Player>());
            UIManager.instance.winEvent(collision.GetComponentInParent<Player>().playerName);
        }
    }

    IEnumerator startCamFunction()
    {
        CamController cam = FindObjectOfType<CamController>();

        Vector3 pos = transform.position;
        cam.levelCam.transform.position = pos;

        cam.levelCam.LookAt = transform;
        cam.levelCam.Follow = transform;

        yield return new WaitForSeconds(2f);

        cam.switchCam(GameManager.instance.turnOrder[GameManager.instance.activeTurnIndex]);
        cam.zoomInZoomOut(5);

        yield return null;
    }
}
