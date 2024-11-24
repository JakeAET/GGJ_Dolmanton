using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CamController : MonoBehaviour
{
    public static CamController instance { get; private set; }

    [SerializeField] float defaultZoom;

    [SerializeField] Animator animator;

    public CinemachineVirtualCamera[] virtualCams;
    public CinemachineVirtualCamera levelCam;

    private bool cameraShaking = false;

    //private GameManager gm;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        animator.Play("goal");
        //gm = FindObjectOfType<GameManager>();
    }

    public void switchCam(int playerIndex)
    {
        for (int i = 0; i < virtualCams.Length; i++)
        {
            if(i == playerIndex)
            {
                animator.Play("player " + (i + 1));
                virtualCams[i].Priority = 1;
            }
            else
            {
                virtualCams[i].Priority = 0;
            }
        }
    }

    public void startCameraShake(string playerName, float intensity, float time)
    {
        int playerIndex = 0;

        //Debug.Log(playerName + " Camera Shaking");

        if (!cameraShaking)
        {
            if (playerName == "Player 1")
            {
                playerIndex = 0;

            }
            else if (playerName == "Player 2")
            {
                playerIndex = 1;

            }
            else if (playerName == "Player 3")
            {
                playerIndex = 2;

            }
            else if (playerName == "Player 4")
            {
                playerIndex = 3;

            }

            StartCoroutine(cameraShake(virtualCams[playerIndex], intensity, time));
        }
    }

    public void zoomInZoomOut(float zoomSize, float duration)
    {
        foreach (CinemachineVirtualCamera cam in virtualCams)
        {
            if (CinemachineCore.Instance.IsLive(cam))
            {
                //Debug.Log( cam.name + " size changed to: " + zoomSize);
                DOTween.To(() => cam.m_Lens.OrthographicSize, x => cam.m_Lens.OrthographicSize = x, zoomSize, duration);
                //cam.m_Lens.OrthographicSize = zoomSize;
            }
            else
            {
                //Debug.Log(cam.name + " size changed to: " + defaultZoom);
                //DOTween.To(() => cam.m_Lens.OrthographicSize, x => cam.m_Lens.OrthographicSize = x, defaultZoom, duration);
                //cam.m_Lens.OrthographicSize = defaultZoom;
            }
        }
    }

    IEnumerator cameraShake(CinemachineVirtualCamera cam, float intensity, float time)
    {
        if (CinemachineCore.Instance.IsLive(cam))
        {
            //cameraShaking = true;

            Debug.Log(cam.name + " Camera Shaking for " + time + " at intensity of " + intensity);

            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

            yield return new WaitForSeconds(time);

            if(cinemachineBasicMultiChannelPerlin.m_AmplitudeGain == intensity)
            {
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
                cameraShaking = false;

                Debug.Log(cam.name + " has stopped shaking");
            }
        }

        yield return null;
    }
}
