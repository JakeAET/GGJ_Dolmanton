using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Cinemachine;
using Unity.VisualScripting;
using System.Linq;
using UnityEditor.Rendering.LookDev;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    public TMP_Text turnText;

    public Sprite[] playerTitleSprites;
    public Image playerTitleImg;

    public Image[] playerPanels;
    public List<Image> activePlayerPanels = new List<Image>();

    [SerializeField] float mainPanelHeight;
    [SerializeField] float subPanelHeight;

    //[SerializeField] GameObject[] p2GameObjects;

    [SerializeField] GameObject winScreenCanvas;
    [SerializeField] GameObject p1Victory;
    [SerializeField] GameObject p2Victory;
    [SerializeField] Button restartButton;
    [SerializeField] Button menuButton;

    public GameObject[] playerSliderObjs;
    public List<Slider> playerSliders = new List<Slider>();
    private float minXPos;
    private float maxXPos;

    private Vector3 startGolfHeadPos;

    public List<Vector3> imgPanelPositions = new List<Vector3>();

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

        DOTween.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        initializeSliders();

        for (int i = 0; i < playerPanels.Length; i++)
        {
            if(i < GameManager.instance.activePlayerCount)
            {
                activePlayerPanels.Add(playerPanels[i]);
                playerPanels[i].color = GameManager.instance.playerObjs[GameManager.instance.turnOrder[i]].GetComponent<Player>().outfitColor;
                imgPanelPositions.Add(playerPanels[i].GetComponent<RectTransform>().localPosition);
            }
            else
            {
                playerPanels[i].enabled = false;
                playerPanels[i].GetComponent<Panel>().shadow.enabled = false;
            }
        }

        playerTitleImg.sprite = playerTitleSprites[GameManager.instance.currentTurnOrder[0]];
        turnText.text = "|  Turn: " + GameManager.instance.activePlayer.turnCount;

        restartButton.onClick.AddListener(restartLevel);
        menuButton.onClick.AddListener(returnToMenu);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < playerSliders.Count; i++)
        {
            float targetXPos = GameManager.instance.playerObjs[i].GetComponent<Player>().slingshotPoint.position.x;
            playerSliders[i].value = Mathf.Lerp(0, 100, Mathf.InverseLerp(minXPos, maxXPos, targetXPos));
        }
    }

    public void uiTurnChange(Player player)
    {
        if(GameManager.instance.activePlayerCount > 1)
        {
            StartCoroutine(turnChangeAnim(player));
        }
        else
        {
            turnText.text = "|  Turn: " + player.turnCount;
        }

        //int oldIndex = 0;
        //int newIndex = 0;

        for (int i = 0; i < playerSliderObjs.Length; i++)
        {
            if (i == GameManager.instance.currentTurnOrder[0])
            {
                //oldIndex = playerSliderObjs[i].transform.GetSiblingIndex();
                playerSliderObjs[i].transform.SetAsLastSibling();
                //newIndex = playerSliderObjs[i].transform.GetSiblingIndex();

                Transform headTransform = playerSliderObjs[i].GetComponent<PlayerSlider>().playerHead.transform;
                headTransform.DOMoveY(headTransform.position.y + 10f, 0.2f);
                headTransform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.2f);

                //Debug.Log("Player obj " + i + " index changed from " + oldIndex + " to " + newIndex);

            }
            else
            {
                Transform headTransform = playerSliderObjs[i].GetComponent<PlayerSlider>().playerHead.transform;
                headTransform.DOMoveY(startGolfHeadPos.y, 0.2f);
                headTransform.transform.DOScale(Vector3.one, 0.2f);
            }
        }
    }

    IEnumerator turnChangeAnim(Player player)
    {
        turnText.DOFade(0, 0.2f);
        playerTitleImg.DOFade(0, 0.2f);

        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < activePlayerPanels.Count; i++)
        {
            if(i - 1 >= 0)
            {
                activePlayerPanels[i].GetComponent<RectTransform>().DOLocalMoveY(imgPanelPositions[i - 1].y, 0.5f);

                if (i - 1 != 0)
                {

                    Vector2 newSize = new Vector2(activePlayerPanels[i - 1].GetComponent<RectTransform>().sizeDelta.x, subPanelHeight);
                    activePlayerPanels[i].GetComponent<RectTransform>().DOSizeDelta(newSize, 0.3f);
                }
                else
                {
                    Vector2 newSize = new Vector2(activePlayerPanels[i - 1].GetComponent<RectTransform>().sizeDelta.x, mainPanelHeight);
                    activePlayerPanels[i].GetComponent<Panel>().shadow.DOFade(0, 0.3f);
                    activePlayerPanels[i].GetComponent<RectTransform>().DOSizeDelta(newSize, 0.3f);
                }
                
;            }
            else
            {
                Vector2 newSize = new Vector2(activePlayerPanels[imgPanelPositions.Count - 1].GetComponent<RectTransform>().sizeDelta.x, subPanelHeight);

                DG.Tweening.Sequence topPanelSequence = DOTween.Sequence();
                topPanelSequence.Append(activePlayerPanels[i].GetComponent<Panel>().shadow.DOFade(0.2f, 0));
                topPanelSequence.Append(activePlayerPanels[i].GetComponent<RectTransform>().DOLocalMoveY(activePlayerPanels[i].GetComponent<RectTransform>().localPosition.y + activePlayerPanels[i].GetComponent<RectTransform>().sizeDelta.y, 0.5f));
                topPanelSequence.Append(activePlayerPanels[i].DOFade(0, 0));
                topPanelSequence.Append(activePlayerPanels[i].GetComponent<RectTransform>().DOSizeDelta(newSize, 0f));
                topPanelSequence.Append(activePlayerPanels[i].GetComponent<RectTransform>().DOLocalMoveY(imgPanelPositions[imgPanelPositions.Count - 1].y, 0));
                topPanelSequence.Append(activePlayerPanels[i].GetComponent<RectTransform>().DOLocalMoveX(imgPanelPositions[imgPanelPositions.Count - 1].x - 100, 0));
                topPanelSequence.Append(activePlayerPanels[i].DOFade(1, 0));
                topPanelSequence.Append(activePlayerPanels[i].GetComponent<RectTransform>().DOLocalMoveX(imgPanelPositions[imgPanelPositions.Count - 1].x, 0.2f));

                topPanelSequence.Play();
            }
        }

        yield return new WaitForSeconds(0.3f);

        playerTitleImg.sprite = playerTitleSprites[GameManager.instance.currentTurnOrder[0]];
        turnText.text = "|  Turn: " + player.turnCount;

        activePlayerPanels[0].DOFade(1, 0.2f);
        turnText.DOFade(1, 0.2f);
        playerTitleImg.DOFade(1, 0.2f);

        newPanelOrder();

        yield return null;
    }

    public void restartLevel()
    {
        GameManager.instance.changeScene("Game Screen", true);
    }

    public void returnToMenu()
    {
        GameManager.instance.changeScene("Title Screen");
    }

    // TODO: Add winning screens for all 4 players
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

    public void newPanelOrder()
    {
        List<Image> newPanelOrder = new List<Image>();

        for (int i = 0; i < activePlayerPanels.Count; i++)
        {
            if (i + 1 < activePlayerPanels.Count)
            {
                newPanelOrder.Add(activePlayerPanels[i + 1]);
            }
            else
            {
                newPanelOrder.Add(activePlayerPanels[0]);
            }
        }

        activePlayerPanels = newPanelOrder;
    }

    public void initializeSliders()
    {
        minXPos = GameManager.instance.levelEdgePos.x;
        maxXPos = GameManager.instance.levelGoalPos.x;

        for (int i = 0; i < playerSliderObjs.Length; i++)
        {
            if (i < GameManager.instance.activePlayerCount)
            {
                playerSliders.Add(playerSliderObjs[i].GetComponent<Slider>());
                PlayerSlider sliderRef = playerSliderObjs[i].GetComponent<PlayerSlider>();
                sliderRef.outfit.color = GameManager.instance.playerOutfitColors[i];
                sliderRef.pointer.color = GameManager.instance.playerOutfitColors[i];
                sliderRef.skin.color = GameManager.instance.playerSkinColors[i];
            }
        }

        //int oldIndex = 0;
        //int newIndex = 0;

        for (int i = 0; i < playerSliderObjs.Length; i++)
        {
            if (i == GameManager.instance.currentTurnOrder[0])
            {
                //oldIndex = playerSliderObjs[i].transform.GetSiblingIndex();
                playerSliderObjs[i].transform.SetAsLastSibling();
                //newIndex = playerSliderObjs[i].transform.GetSiblingIndex();

                Transform headTransform = playerSliderObjs[i].GetComponent<PlayerSlider>().playerHead.transform;
                startGolfHeadPos = headTransform.position;
                headTransform.DOMoveY(headTransform.position.y + 10f, 0.2f);
                headTransform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.2f);

                //Debug.Log("Player obj " + i + " index changed from " + oldIndex + " to " + newIndex);
            }
            else
            {
                Transform headTransform = playerSliderObjs[i].GetComponent<PlayerSlider>().playerHead.transform;
                headTransform.transform.DOScale(Vector3.one, 0.2f);
            }
        }
    }
}
