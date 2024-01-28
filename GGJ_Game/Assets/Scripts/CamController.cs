using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamController : MonoBehaviour
{
    public static CamController instance { get; private set; }

    [SerializeField] Animator animator;

    [SerializeField] CinemachineVirtualCamera virtualCam1;
    [SerializeField] CinemachineVirtualCamera virtualCam2;
    public CinemachineVirtualCamera virtualCam3;

    [SerializeField] GameObject bg1;

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

        animator.Play("goal");

        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (CinemachineCore.Instance.IsLive(virtualCam1))
        //{
        //    Vector3 currentCamPos = virtualCam1.transform.position;
        //    currentCamPos.z = 0;
        //    bg1.transform.position = currentCamPos;
        //}
        //else if (CinemachineCore.Instance.IsLive(virtualCam2))
        //{
        //    Vector3 currentCamPos = virtualCam2.transform.position;
        //    currentCamPos.z = 0;
        //    bg1.transform.position = currentCamPos;
        //}
        //else if (CinemachineCore.Instance.IsLive(virtualCam3))
        //{
        //    Vector3 currentCamPos = virtualCam3.transform.position;
        //    currentCamPos.z = 0;
        //    bg1.transform.position = currentCamPos;
        //}
    }

    public void switchCam(string player)
    {
        if(player == "Player 1")
        {
            animator.Play("player 1");
            virtualCam1.Priority = 1;
            virtualCam2.Priority = 0;
            virtualCam3.Priority = 0;
        }
        else if(player == "Player 2")
        {
            animator.Play("player 2");
            virtualCam1.Priority = 0;
            virtualCam2.Priority = 1;
            virtualCam3.Priority = 0;
        }
    }

    public void zoomInZoomOut(float zoomSize)
    {
        if (CinemachineCore.Instance.IsLive(virtualCam1))
        {
            virtualCam1.m_Lens.OrthographicSize = zoomSize;
            virtualCam2.m_Lens.OrthographicSize = 15f;
        }
        else if (CinemachineCore.Instance.IsLive(virtualCam2))
        {
            virtualCam2.m_Lens.OrthographicSize = zoomSize;
            virtualCam1.m_Lens.OrthographicSize = 15f;

        }
    }
}
