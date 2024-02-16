using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        FindObjectOfType<GameManager>().levelGoalPos = transform.position;
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
            StartCoroutine(winEvent(collision.GetComponentInParent<Player>()));
            //Player player = collision.GetComponentInParent<Player>();
            //player.switchFace(true);
            //AudioManager.instance.playVictory(player.playerName);
            //GameManager.instance.goalReached(player);
            //UIManager.instance.winEvent(player.playerName);
        }
    }

    IEnumerator winEvent(Player player)
    {
        player.switchFace(true);
        float waitTime = AudioManager.instance.playVictory(player.playerName);
        GameManager.instance.goalReached(player);
        UIManager.instance.winEvent(player.playerName);

        yield return new WaitForSeconds(waitTime);

        AudioManager.instance.Play("win_song");

        yield return null;
    }

    IEnumerator startCamFunction()
    {

        yield return new WaitForSeconds(1f);

        CamController cam = FindObjectOfType<CamController>();

        cam.levelCam.GetComponent<Transform>().DOMoveX(transform.position.x - 10f, 4f);

        //Vector3 pos = transform.position;
        //cam.levelCam.transform.position = pos;

        //cam.levelCam.LookAt = transform;
        //cam.levelCam.Follow = transform;

        //DOTween.To(() => cam.levelCam.m_Lens.OrthographicSize, x => cam.levelCam.m_Lens.OrthographicSize = x, 10, 1.5f);
        //cam.levelCam.m_Lens.OrthographicSize = 10;

        yield return new WaitForSeconds(4f);

        AudioManager.instance.Play("level_start");

        yield return new WaitForSeconds(3f);

        cam.switchCam(GameManager.instance.turnOrder[GameManager.instance.activeTurnIndex]);
        cam.zoomInZoomOut(5);

        GameManager.instance.activateFirstPlayer();

        yield return null;
    }
}
