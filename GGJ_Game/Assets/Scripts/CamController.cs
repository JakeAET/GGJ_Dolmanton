using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamController : MonoBehaviour
{
    public static CamController instance { get; private set; }

    [SerializeField] float defaultZoom;

    [SerializeField] Animator animator;

    public CinemachineVirtualCamera[] virtualCams;
    public CinemachineVirtualCamera levelCam;

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

    public void zoomInZoomOut(float zoomSize)
    {
        foreach (CinemachineVirtualCamera cam in virtualCams)
        {
            if (CinemachineCore.Instance.IsLive(cam))
            {
                cam.m_Lens.OrthographicSize = zoomSize;
            }
            else
            {
                cam.m_Lens.OrthographicSize = defaultZoom;
            }
        }
    }
}
