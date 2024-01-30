using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    public TMP_Text turnTextP1;
    public TMP_Text turnTextP2;

    public TMP_Text p1Text;
    public TMP_Text p2Text;

    public Image p1Panel;
    public Image p2Panel;

    [SerializeField] GameObject[] p2GameObjects;

    private Color disabledColor;

    [SerializeField] GameObject winScreenCanvas;
    [SerializeField] GameObject p1Victory;
    [SerializeField] GameObject p2Victory;
    [SerializeField] Button restartButton;
    [SerializeField] Button menuButton;

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

        disabledColor = new Color(1f, 1f, 1f, 0.5f);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.currentMode == GameManager.gamemode.SinglePlayer)
        {
            foreach (GameObject obj in p2GameObjects)
            {
                obj.SetActive(false);
            }
        }

        p1Panel.color = GameManager.instance.p1OutfitColor;
        p2Panel.color = GameManager.instance.p2OutfitColor;

        restartButton.onClick.AddListener(restartLevel);
        menuButton.onClick.AddListener(returnToMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void uiTurnChange(bool player1Turn)
    {
        if (player1Turn)
        {
            p1Text.color = Color.white;
            turnTextP1.color = Color.white;

            p2Text.color = disabledColor;
            turnTextP2.color = disabledColor;
        }
        else
        {
            p1Text.color = disabledColor;
            turnTextP1.color = disabledColor;

            p2Text.color = Color.white;
            turnTextP2.color = Color.white;
        }
    }

    public void restartLevel()
    {
        GameManager.instance.changeScene("Game Screen", true);
    }

    public void returnToMenu()
    {
        GameManager.instance.changeScene("Title Screen");
    }

    public void winEvent(string winningPlayer)
    {
        winScreenCanvas.SetActive(true);

        if(winningPlayer == "Player 1")
        {
            p1Victory.SetActive(true);
            p2Victory.SetActive(false);
        }
        else if(winningPlayer == "Player 2")
        {
            p1Victory.SetActive(false);
            p2Victory.SetActive(true);
        }
    }
}
