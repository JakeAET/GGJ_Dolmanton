using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.SocialPlatforms;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [SerializeField] float nearPlayerRange = 6;

    public bool turnBasedActive;

    public Player activePlayer;
    public List<GameObject> playerObjs = new List<GameObject>();

    public int activePlayerCount = 1;
    public int activeTurnIndex;
    public List<int> turnOrder = new List<int>();
    public List<int> currentTurnOrder = new List<int>();

    public List<Color> playerSkinColors = new List<Color>();
    public List<Color> playerOutfitColors = new List<Color>();

    public bool allowLaunch;
    public bool launchFinished;

    public Vector3 levelStartPos;
    public Vector3 levelEdgePos;
    public Vector3 levelGoalPos;

    public bool firstPlayerSpawned = false;

    [SerializeField] GameObject playerInstance;

    private TMP_Text turnText;

    private List<CinemachineVirtualCamera> vcams = new List<CinemachineVirtualCamera>();

    private CamController camControllerRef;

    public bool colorsInitialized;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
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

    void nextTurn()
    {
        launchFinished = false;
        camControllerRef.zoomInZoomOut(15);

        // If not solo game
        if (activePlayerCount > 1)
        {
            // Determine next turn
            if(activeTurnIndex + 1 >= turnOrder.Count)
            {
                // Reset to first players turn
                activeTurnIndex = 0;
            }
            else
            {
                // Shift to next players turn
                activeTurnIndex++;
            }

            // Set new active player
            activePlayer = playerObjs[turnOrder[activeTurnIndex]].GetComponent<Player>();

            // Check player gameobject is active
            if (!playerObjs[turnOrder[activeTurnIndex]].activeInHierarchy)
            {
                playerObjs[turnOrder[activeTurnIndex]].SetActive(true);
                UIManager.instance.playerSliderObjs[turnOrder[activeTurnIndex]].SetActive(true);
            }

            // Switch camera and UI to active player
            camControllerRef.switchCam(turnOrder[activeTurnIndex]);
            activePlayer.turnCount++;
            newTurnOrder();
            UIManager.instance.uiTurnChange(activePlayer);
        }
        else
        {
            activePlayer.turnCount++;
            UIManager.instance.uiTurnChange(activePlayer);
        }

        allowLaunch = true;
        StartCoroutine(turnBehavior());
    }

    IEnumerator turnBehavior()
    {
        yield return new WaitUntil(doneWaitingForFirstPlayer);

        yield return new WaitUntil(turnEnded);

        activePlayer.switchFace(true);
        camControllerRef.zoomInZoomOut(5);

        if (turnBasedActive)
        {
            if (nearPlayer(activePlayer, nearPlayerRange))
            {
                AudioManager.instance.playProximity(activePlayer.playerName);
            }
            else
            {
                AudioManager.instance.playCatchphrase(activePlayer.playerName);
            }
        }

        yield return new WaitForSeconds(2f);

        nextTurn();

        yield return null;
    }

    public void initializeLevel(Vector3 startPos, GameObject[] playerVcams, GameObject camController)
    {
        // Reset game data
        Time.timeScale = 1;
        playerObjs = new List<GameObject>();
        vcams = new List<CinemachineVirtualCamera>();
        activeTurnIndex = 0;

        levelStartPos = startPos;

        camControllerRef = camController.GetComponent<CamController>();

        // Pick starting order
        turnOrder = determineStartingOrder(activePlayerCount);
        currentTurnOrder = turnOrder;

        // Create players based on player count
        for (int i = 0; i < activePlayerCount; i++)
        {
            // Set up player
            var newPlayer = Instantiate(playerInstance, levelStartPos, Quaternion.Euler(new Vector3(0, 0, 60)));
            newPlayer.name = "Player " + (i + 1);
            assignLayerMask(newPlayer, "Player" + (i + 1));
            Player thisPlayer = newPlayer.GetComponent<Player>();
            thisPlayer.playerName = "Player " + (i + 1);
            thisPlayer.skinColor = playerSkinColors[i];
            thisPlayer.outfitColor = playerOutfitColors[i];
            thisPlayer.golfRB.constraints = RigidbodyConstraints2D.FreezeAll;
            playerObjs.Add(newPlayer);

            // Set up player camera
            vcams.Add(playerVcams[i].GetComponent<CinemachineVirtualCamera>());
            vcams[i].Follow = thisPlayer.slingshotPoint.transform;
            vcams[i].LookAt = thisPlayer.slingshotPoint.transform;

            // Is this player starting?
            if(i == turnOrder[0])
            {
                activePlayer = thisPlayer;
                activePlayer.turnCount++;
            }

            newPlayer.SetActive(false);
        }

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

    public void activateFirstPlayer()
    {
        activePlayer.switchFace(true);
        playerObjs[currentTurnOrder[0]].SetActive(true);
        UIManager.instance.playerSliderObjs[turnOrder[0]].SetActive(true);

        allowLaunch = true;
        turnBasedActive = true;
        firstPlayerSpawned = true;
    }

    private bool doneWaitingForFirstPlayer()
    {
        return firstPlayerSpawned;
    }

    public void goalReached(Player winningPlayer)
    {
        StopAllCoroutines();
        firstPlayerSpawned = false;
        turnBasedActive = false;
        launchFinished = false;
        activePlayer = null;

        camControllerRef.switchCam(turnOrder[activeTurnIndex]);
        camControllerRef.zoomInZoomOut(5);

        Debug.Log(winningPlayer.playerName + " won in " + winningPlayer.turnCount + " turns!");
    }

    List<int> determineStartingOrder(int numPlayers)
    {
        List<int> initList = new List<int>();
        List<int> playerOrder = new List<int>();

        for (int i = 0; i < numPlayers; i++)
        {
            initList.Add(i);
        }

        for (int i = 0; i < numPlayers; i++)
        {
            int index = UnityEngine.Random.Range(0, initList.Count);
            int num = initList[index];
            initList.Remove(num);
            playerOrder.Add(num);
        }

        return playerOrder;
    }

    private void assignLayerMask(GameObject player, string layerName)
    {
        foreach (Transform child in player.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layerName);
        }
    }

    public void changeScene(string sceneName, bool continueMusic = false)
    {
        AudioManager.instance.sceneChanged(sceneName, continueMusic);
        SceneManager.LoadScene(sceneName);
    }

    public void newTurnOrder()
    {
        List<int> newTurnOrder = new List<int>();

        for (int i = 0; i < activePlayerCount; i++)
        {
            if(i + 1 < activePlayerCount)
            {
                newTurnOrder.Add(currentTurnOrder[i + 1]);
            }
            else
            {
                newTurnOrder.Add(currentTurnOrder[0]);
            }
        }

        currentTurnOrder = newTurnOrder;
    }

    private bool nearPlayer(Player targetPlayer, float range)
    {
        GameObject[] gos;
        List<GameObject> players = new List<GameObject>();
        GameObject targetPlayerObj = null;

        gos = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject g in gos)
        {
            if (g.activeInHierarchy)
            {
                if (g.GetComponentInParent<Player>().playerName != targetPlayer.playerName)
                {
                    players.Add(g);
                }
                else
                {
                    targetPlayerObj = g;
                }
            }
        }

        if (players.Count <= 0)
        {
            //Debug.Log("Did not land near another player");
            return false;
        }

        float closestDistance = Vector3.Distance(players[0].transform.position, targetPlayerObj.transform.position);
        foreach (GameObject g in players)
        {
            float distance = Vector3.Distance(g.transform.position, targetPlayerObj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
            }
        }

        Debug.Log("Nearest player " + closestDistance + " units away. Range is " + range + " units. Return " + (closestDistance <= range));
        return closestDistance <= range;
    }
}
