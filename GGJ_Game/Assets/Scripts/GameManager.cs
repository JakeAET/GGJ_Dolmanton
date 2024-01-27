using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public enum gamemode { SinglePlayer, TwoPlayer };
    public gamemode currentMode;

    public enum turn { Solo, Player1, Player2 };
    public turn currentTurn;

    public Player activePlayer;

    private Vector3 levelStartPos;
    [SerializeField] GameObject playerInstance;

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
        }
        else if (currentMode == gamemode.TwoPlayer)
        {
            int pick = UnityEngine.Random.Range(0, 2);

            if (pick == 0)
            {
                currentTurn = turn.Player1;
            }
            else
            {
                currentTurn = turn.Player2;
            }
        }
    }

    void nextTurn()
    {
        if (currentTurn == turn.Player1)
        {
            currentTurn = turn.Player2;
        }
        else
        {
            currentTurn = turn.Player1;
        }
    }

    IEnumerator turnBehavior()
    {
        if (currentTurn == turn.Solo)
        {

        }
        else if (currentTurn == turn.Player1)
        {

        }
        else if (currentTurn == turn.Player2)
        {

        }

        return null;
    }

    public void initializeLevel(Vector3 startPos)
    {
        firstTurn();

        levelStartPos = startPos;

        if(currentMode == gamemode.SinglePlayer)
        {
            var player = Instantiate(playerInstance, levelStartPos, Quaternion.identity);
            player.name = "Player 1";
            activePlayer = player.GetComponent<Player>();
        }
        else if (currentMode == gamemode.TwoPlayer)
        {
            var player = Instantiate(playerInstance, levelStartPos, Quaternion.identity);
            player.name = "Player 1";
            if (currentTurn == turn.Player2)
            {
                player.SetActive(false);
            }
            else
            {
                activePlayer = player.GetComponent<Player>();
            }

            player = Instantiate(playerInstance, levelStartPos, Quaternion.identity);
            player.name = "Player 2";
            if (currentTurn == turn.Player1)
            {
                player.SetActive(false);
            }
            else
            {
                activePlayer = player.GetComponent<Player>();
            }

        }
    }
}
