using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class TitleManager : MonoBehaviour
{
    [Header("Sprite References")]

    [SerializeField] Sprite[] numSprites;

    [Header("Panel References")]

    [SerializeField] Transform[] skinPanels;
    [SerializeField] Transform[] outfitPanels;

    [SerializeField] GameObject startPanel;
    private bool startPanelActive = true;
    [SerializeField] GameObject[] customPanels;
    private List <bool> customPanelActive = new List<bool>();

    private Vector3 panelEndPos; 

    [Header("Image References")]

    [SerializeField] Image numSprite;
    [SerializeField] Image[] skinImgs;
    [SerializeField] Image[] outfitImgs;

    private List<Image[]> playerImgArrays = new List<Image[]>();

    [SerializeField] Image[] p1Images;
    [SerializeField] Image[] p2Images;
    [SerializeField] Image[] p3Images;
    [SerializeField] Image[] p4Images;

    [Header("Button References")]

    [SerializeField] Button startButton;
    [SerializeField] GameObject plusButtonObj;
    private Button plusButton;
    [SerializeField] GameObject minusButtonObj;
    private Button minusButton;
    [SerializeField] Button[] confirmButtons;

    [SerializeField] GameObject[] customizeButtonObjs;
    private List<Button> playerCustomizeButtons = new List<Button>();

    [SerializeField] ToggleGroup[] skinToggleGroups;
    [SerializeField] ToggleGroup[] outfitToggleGroups;

    private void Awake()
    {
        DOTween.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        playerImgArrays.Add(p1Images);
        playerImgArrays.Add(p2Images);
        playerImgArrays.Add(p3Images);
        playerImgArrays.Add(p4Images);

        plusButton = plusButtonObj.GetComponent<Button>();
        minusButton = minusButtonObj.GetComponent<Button>();

        foreach (GameObject obj in customizeButtonObjs)
        {
            playerCustomizeButtons.Add(obj.GetComponent<Button>());
        }

        for (int i = 0; i < 4; i++)
        {
            bool panelBool = false;
            customPanelActive.Add(panelBool);
        }

        if (GameManager.instance.activePlayerCount == 4)
        {
            plusButton.interactable = false;
            foreach (Image img in plusButtonObj.GetComponentsInChildren<Image>())
            {
                Color color = img.color;
                color.a = 0.5f;
                img.color = color;
            }
        }
        else if (GameManager.instance.activePlayerCount == 1)
        {
            minusButton.interactable = false;
            foreach (Image img in minusButtonObj.GetComponentsInChildren<Image>())
            {
                Color color = img.color;
                color.a = 0.5f;
                img.color = color;
            }
        }

        panelEndPos = customPanels[0].GetComponent<RectTransform>().position;
        Vector3 newPos = panelEndPos;
        newPos.y -= 500f;

        for (int i = 0; i < 4; i++)
        {
            customPanels[i].GetComponent<RectTransform>().position = newPos;    
        }



        for (int i = 0; i < 4; i++)
        {
            skinInitialize(i);
            skinUpdate(i);

            outfitInitialize(i);
            outfitUpdate(i);
        }

        numSprite.sprite = numSprites[GameManager.instance.activePlayerCount - 1];
        updateHiddenPlayers();

        startButton.onClick.AddListener(startGame);
        plusButton.onClick.AddListener(plusPlayer);
        minusButton.onClick.AddListener(minusPlayer);

        playerCustomizeButtons[0].onClick.AddListener(delegate { customizeActivate(0); });
        playerCustomizeButtons[1].onClick.AddListener(delegate { customizeActivate(1); });
        playerCustomizeButtons[2].onClick.AddListener(delegate { customizeActivate(2); });
        playerCustomizeButtons[3].onClick.AddListener(delegate { customizeActivate(3); });

        foreach (Button b in confirmButtons)
        {
            b.onClick.AddListener(confirmCustom);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame()
    {
        if(GameManager.instance.activePlayerCount >= 1)
        {
            AudioManager.instance.Play("p1_start");
        }

        if (GameManager.instance.activePlayerCount >= 2)
        {
            AudioManager.instance.Play("p2_start");
        }

        if (GameManager.instance.activePlayerCount >= 3)
        {
            AudioManager.instance.Play("p3_start");
        }

        if (GameManager.instance.activePlayerCount >= 4)
        {
            AudioManager.instance.Play("p4_start");
        }

        GameManager.instance.changeScene("Game Screen");
    }

    public void confirmCustom()
    {
        for (int i = 0; i < customPanelActive.Count; i++)
        {
            // Close custom panel
            if (customPanelActive[i])
            {
                customPanels[i].GetComponent<RectTransform>().DOMoveY(panelEndPos.y - 500, 0.5f);
                customPanelActive[i] = false;
                foreach (Image img in customizeButtonObjs[i].GetComponentsInChildren<Image>())
                {
                    Color color = img.color;
                    color.a = 0.8f;
                    img.color = color;
                }
            }

            //Open start panel
            startPanel.GetComponent<RectTransform>().DOMoveY(0, 0.5f);
            startPanelActive = true;
        }
    }

    public void customizeActivate(int pNum)
    {
        if (!customPanelActive[pNum])
        {
            customPanels[pNum].GetComponent<RectTransform>().DOMoveY(6,0.5f);
            foreach (Image img in customizeButtonObjs[pNum].GetComponentsInChildren<Image>())
            {
                Color color = img.color;
                color.a = 0.3f;
                img.color = color;
            }

            // Close start panel
            if (startPanelActive)
            {
                startPanel.GetComponent<RectTransform>().DOMoveY(panelEndPos.y - 500, 0.5f);
                startPanelActive = false;
            }

            for (int i = 0; i < customPanelActive.Count; i++)
            {
                // Close custom panel
                if (customPanelActive[i] && i != pNum)
                {
                    customPanels[i].GetComponent<RectTransform>().DOMoveY(panelEndPos.y - 500, 0.5f);
                    foreach (Image img in customizeButtonObjs[i].GetComponentsInChildren<Image>())
                    {
                        Color color = img.color;
                        color.a = 0.8f;
                        img.color = color;
                    }

                    customPanelActive[i] = false;
                }
            }

            customPanelActive[pNum] = true;
        }
    }

    public void plusPlayer()
    {
        if(GameManager.instance.activePlayerCount < 4)
        {
            GameManager.instance.activePlayerCount++;

            if (GameManager.instance.activePlayerCount == 4)
            {
                plusButton.interactable = false;
                foreach (Image img in plusButtonObj.GetComponentsInChildren<Image>())
                {
                    Color color = img.color;
                    color.a = 0.5f;
                    img.color = color;
                }
            }

            if(minusButton.interactable == false)
            {
                minusButton.interactable = true;
                foreach (Image img in minusButtonObj.GetComponentsInChildren<Image>())
                {
                    Color color = img.color;
                    color.a = 1f;
                    img.color = color;
                }
            }

            numSprite.sprite = numSprites[GameManager.instance.activePlayerCount - 1];
            updateHiddenPlayers();
        }
    }

    public void minusPlayer()
    {
        if (GameManager.instance.activePlayerCount > 1)
        {
            GameManager.instance.activePlayerCount--;

            if (GameManager.instance.activePlayerCount == 1)
            {
                minusButton.interactable = false;
                foreach (Image img in minusButtonObj.GetComponentsInChildren<Image>())
                {
                    Color color = img.color;
                    color.a = 0.5f;
                    img.color = color;
                }
            }

            if (plusButton.interactable == false)
            {
                plusButton.interactable = true;
                foreach (Image img in plusButtonObj.GetComponentsInChildren<Image>())
                {
                    Color color = img.color;
                    color.a = 1f;
                    img.color = color;
                }
            }

            numSprite.sprite = numSprites[GameManager.instance.activePlayerCount - 1];
            updateHiddenPlayers();
        }
    }

    public void skinInitialize(int pNum)
    {
        skinToggleGroups[pNum].allowSwitchOff = true;

        foreach (Transform child in skinPanels[pNum])
        {
            if (child.GetComponent<Toggle>() != null)
            {
                child.GetComponent<Toggle>().isOn = (child.GetComponent<Toggle>().GetComponentInChildren<Image>().color == GameManager.instance.playerSkinColors[pNum]);
            }
        }

        skinToggleGroups[pNum].allowSwitchOff = false;
    }

    public void outfitInitialize(int pNum)
    {
        outfitToggleGroups[pNum].allowSwitchOff = true;

        foreach (Transform child in outfitPanels[pNum])
        {
            if (child.GetComponent<Toggle>() != null)
            {
                child.GetComponent<Toggle>().isOn = (child.GetComponent<Toggle>().GetComponentInChildren<Image>().color == GameManager.instance.playerOutfitColors[pNum]);
            }
        }

        outfitToggleGroups[pNum].allowSwitchOff = false;
    }

    public void skinUpdate(int pNum)
    {
        foreach (Transform child in skinPanels[pNum])
        {
            if (child.GetComponent<Toggle>() != null)
            {
                if (child.GetComponent<Toggle>().isOn)
                {
                    GameManager.instance.playerSkinColors[pNum] = child.GetComponent<Toggle>().GetComponentInChildren<Image>().color;
                    skinImgs[pNum].color = child.GetComponent<Toggle>().GetComponentInChildren<Image>().color;
                    break;
                }
            }
        }
    }

    public void outfitUpdate(int pNum)
    {
        foreach (Transform child in outfitPanels[pNum])
        {
            if (child.GetComponent<Toggle>() != null)
            {
                if (child.GetComponent<Toggle>().isOn)
                {
                    GameManager.instance.playerOutfitColors[pNum] = child.GetComponent<Toggle>().GetComponentInChildren<Image>().color;
                    outfitImgs[pNum].color = child.GetComponent<Toggle>().GetComponentInChildren<Image>().color;
                    break;
                }
            }
        }
    }

    public void updateHiddenPlayers()
    {
        for (int i = 0; i < 4; i++)
        {
            if(i + 1 <= GameManager.instance.activePlayerCount)
            {
                foreach (Image img in playerImgArrays[i])
                {
                    Color color = img.color;
                    color.a = 1f;
                    img.color = color;
                }

                playerCustomizeButtons[i].interactable = true;
                customizeButtonObjs[i].SetActive(true);
            }
            else
            {
                foreach (Image img in playerImgArrays[i])
                {
                    Color color = img.color;
                    color.a = 0.3f;
                    img.color = color;
                }

                playerCustomizeButtons[i].interactable = false;
                customizeButtonObjs[i].SetActive(false);
            }
        }
    }
}
