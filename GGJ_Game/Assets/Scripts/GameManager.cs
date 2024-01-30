using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public enum gamemode { SinglePlayer, TwoPlayer };
    public gamemode currentMode;

    public enum turn { Solo, Player1, Player2, Win};
    public turn currentTurn;

    public Player activePlayer;
    private GameObject player1Obj;
    private GameObject player2Obj;

    public Color p1SkinColor;
    public Color p1OutfitColor;
    public Color p2SkinColor;
    public Color p2OutfitColor;

    public bool allowLaunch;
    public bool launchFinished;

    private Vector3 levelStartPos;
    [SerializeField] GameObject playerInstance;

    private float turnCountP1;
    private float turnCountP2;
    private TMP_Text turnTextP1;
    private TMP_Text turnTextP2;

    private CinemachineVirtualCamera vcam1;
    private CinemachineVirtualCamera vcam2;

    private CamController camControllerRef;

    [SerializeField] AudioClip[] p1CatchPhrases;
    [SerializeField] AudioClip[] p2CatchPhrases;

    public bool colorsInitialized;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void firstTurn()
    {
        if (currentMode == gamemode.SinglePlayer)
        {
            currentTurn = turn.Player1;
            turnCountP1 = 1;
            //camControllerRef.switchCam("Player 1");
        }
        else if (currentMode == gamemode.TwoPlayer)
        {
            int pick = UnityEngine.Random.Range(0, 2);

            if (pick == 0)
            {
                currentTurn = turn.Player1;
                //camControllerRef.switchCam("Player 1");
                turnCountP1 = 1;
                turnCountP2 = 0;
                UIManager.instance.uiTurnChange(true);
            }
            else
            {
                currentTurn = turn.Player2;
                //camControllerRef.switchCam("Player 2");
                turnCountP1 = 0;
                turnCountP2 = 1;
                UIManager.instance.uiTurnChange(false);
            }
        }

        turnTextP1.text = "Turn: " + turnCountP1;
        turnTextP2.text = "Turn: " + turnCountP2;
    }

    void nextTurn()
    {
        launchFinished = false;
        camControllerRef.zoomInZoomOut(15);

        if (currentMode != gamemode.SinglePlayer)
        {
            if (currentTurn == turn.Player1)
            {
                if (!player2Obj.activeInHierarchy)
                {
                    player2Obj.SetActive(true);
                }

                turnCountP2++;
                turnTextP2.text = "Turn: " + turnCountP2;
                currentTurn = turn.Player2;
                camControllerRef.switchCam("Player 2");
                activePlayer = player2Obj.GetComponent<Player>();
                UIManager.instance.uiTurnChange(false);
            }
            else
            {
                if (!player1Obj.activeInHierarchy)
                {
                    player1Obj.SetActive(true);
                }

                turnCountP1++;
                turnTextP1.text = "Turn: " + turnCountP1;
                currentTurn = turn.Player1;
                camControllerRef.switchCam("Player 1");
                activePlayer = player1Obj.GetComponent<Player>();
                UIManager.instance.uiTurnChange(true);
            }
        }
        else
        {
            turnCountP1++;
            turnTextP1.text = "Turn: " + turnCountP1;
        }

        allowLaunch = true;
        //CamController.instance.zoomInZoomOut(5);
        StartCoroutine(turnBehavior());
    }

    IEnumerator turnBehavior()
    {
        yield return new WaitUntil(turnEnded);

        activePlayer.switchFace(true);
        camControllerRef.zoomInZoomOut(5);

        if(activePlayer.playerName == "Player 1")
        {
            GetComponent<AudioSource>().PlayOneShot(p1CatchPhrases[UnityEngine.Random.Range(0, p1CatchPhrases.Length)]);
        }
        else if(activePlayer.playerName == "Player 2")
        {
            GetComponent<AudioSource>().PlayOneShot(p2CatchPhrases[UnityEngine.Random.Range(0, p2CatchPhrases.Length)]);
        }

        yield return new WaitForSeconds(2f);

        nextTurn();

        yield return null;
    }

    public void initializeLevel(Vector3 startPos, GameObject startVcam1, GameObject startVcam2, GameObject camController)
    {
        levelStartPos = startPos;
        turnTextP1 = UIManager.instance.turnTextP1;
        turnTextP2 = UIManager.instance.turnTextP2;

        camControllerRef = camController.GetComponent<CamController>();
        firstTurn();

        if (currentMode == gamemode.SinglePlayer)
        {
            player1Obj = Instantiate(playerInstance, levelStartPos, Quaternion.identity);
            player1Obj.name = "Player 1";
            assignLayerMask(player1Obj, "Player1");
            player1Obj.GetComponent<Player>().playerName = "Player 1";
            player1Obj.GetComponent<Player>().skinColor = p1SkinColor;
            player1Obj.GetComponent<Player>().outfitColor = p1OutfitColor;
            activePlayer = player1Obj.GetComponent<Player>();
            //startVcam1.SetActive(true);
            vcam1 = startVcam1.GetComponent<CinemachineVirtualCamera>();
            vcam1.Follow = player1Obj.GetComponent<Player>().slingshotPoint.transform;
            vcam1.LookAt = player1Obj.GetComponent<Player>().slingshotPoint.transform;
        }
        else if (currentMode == gamemode.TwoPlayer)
        {
            player1Obj = Instantiate(playerInstance, levelStartPos, Quaternion.identity);
            player1Obj.name = "Player 1";
            assignLayerMask(player1Obj, "Player1");
            player1Obj.GetComponent<Player>().playerName = "Player 1";
            player1Obj.GetComponent<Player>().skinColor = p1SkinColor;
            player1Obj.GetComponent<Player>().outfitColor = p1OutfitColor;
            //startVcam1.SetActive(true);
            vcam1 = startVcam1.GetComponent<CinemachineVirtualCamera>();
            vcam1.Follow = player1Obj.GetComponent<Player>().slingshotPoint.transform;
            vcam1.LookAt = player1Obj.GetComponent<Player>().slingshotPoint.transform;

            if (currentTurn != turn.Player1)
            {
                player1Obj.SetActive(false);
            }
            else
            {
                activePlayer = player1Obj.GetComponent<Player>();
            }

            player2Obj = Instantiate(playerInstance, levelStartPos, Quaternion.identity);
            player2Obj.name = "Player 2";
            assignLayerMask(player2Obj, "Player2");
            player2Obj.GetComponent<Player>().playerName = "Player 2";
            player2Obj.GetComponent<Player>().skinColor = p2SkinColor;
            player2Obj.GetComponent<Player>().outfitColor = p2OutfitColor;
            //startVcam2.SetActive(true);
            vcam2 = startVcam2.GetComponent<CinemachineVirtualCamera>();
            vcam2.Follow = player2Obj.GetComponent<Player>().slingshotPoint.transform;
            vcam2.LookAt = player2Obj.GetComponent<Player>().slingshotPoint.transform;

            if (currentTurn != turn.Player2)
            {
                player2Obj.SetActive(false);
            }
            else
            {
                activePlayer = player2Obj.GetComponent<Player>();
            }
        }

        allowLaunch = true;
        StartCoroutine(turnBehavior());
    }

    bool turnEnded()
    {
        if (!launchFinished)
        {
            return false;
        }

        float xVelocity = Mathf.Abs(activePlayer.golfRB.velocity.x);
        float yVelocity = Mathf.Abs(activePlayer.golfRB.velocity.y);

        //Debug.Log("xVelocity: " + xVelocity + "  yVelocity: " + yVelocity);

        if (xVelocity <= 0.1f && yVelocity <= 0.1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void goalReached(string winningPlayer)
    {
        StopAllCoroutines();
        camControllerRef.zoomInZoomOut(5);
        currentTurn = turn.Win;
        launchFinished = false;
        activePlayer = null;

        if (winningPlayer == "Player 1")
        {
            Debug.Log(winningPlayer + " won in " + turnCountP1 + " turns!");
        }
        else if (winningPlayer == "Player 2")
        {
            Debug.Log(winningPlayer + " won in " + turnCountP2 + " turns!");
        }
    }

    private void assignLayerMask(GameObject player, string layerName)
    {
        foreach (Transform child in player.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }
}
