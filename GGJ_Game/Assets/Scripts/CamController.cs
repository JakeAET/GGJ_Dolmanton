using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamController : MonoBehaviour
{
    public static CamController instance { get; private set; }

    private Animator animator;

    [SerializeField] CinemachineVirtualCamera virtualCam1;
    [SerializeField] CinemachineVirtualCamera virtualCam2;

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

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchCam(string player)
    {
        if(player == "Player 1")
        {
            animator.Play("player 1");
            //virtualCam1.Priority = 1;
            //virtualCam2.Priority = 0;
        }
        else if(player == "Player 2")
        {
            animator.Play("player 2");
            //virtualCam1.Priority = 0;
            //virtualCam2.Priority = 1;
        }
    }
}
