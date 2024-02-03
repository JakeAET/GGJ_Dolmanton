using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] Transform p1SkinPanel;
    [SerializeField] Transform p1OutfitPanel;

    [SerializeField] Transform p2SkinPanel;
    [SerializeField] Transform p2OutfitPanel;

    [SerializeField] Image p1SkinImg;
    [SerializeField] Image p1OutfitImg;
    [SerializeField] Image p2SkinImg;
    [SerializeField] Image p2OutfitImg;

    [SerializeField] GameObject p2Settings;

    [SerializeField] Button p1Button;
    [SerializeField] Button p2Button;
    [SerializeField] Button startButton;

    [SerializeField] Outline p1Outline;
    [SerializeField] Outline p2Outline;

    [SerializeField] Image[] p2Images;

    [SerializeField] ToggleGroup[] toggleGroups;

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.instance.activePlayerCount == 1)
        {
            p1Outline.enabled = true;
            p2Outline.enabled = false;
            p2Settings.SetActive(false);
            hideP2(true);
        }
        else if (GameManager.instance.activePlayerCount == 2)
        {
            p1Outline.enabled = false;
            p2Outline.enabled = true;
        }

        startButton.onClick.AddListener(startGame);
        p1Button.onClick.AddListener(onePlayer);
        p2Button.onClick.AddListener(twoPlayer);

        if (!GameManager.instance.colorsInitialized)
        {
            p1SkinUpdate();
            p1OutfitUpdate();
            p2SkinUpdate();
            p2OutfitUpdate();
            GameManager.instance.colorsInitialized = true;
        }
        else
        {
            assignActiveColors();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onePlayer()
    {
        if(GameManager.instance.activePlayerCount != 1)
        {
            AudioManager.instance.Play("solo_phrase");
            p1Outline.enabled = true;
            p2Outline.enabled = false;
            GameManager.instance.activePlayerCount = 1;

            p2Settings.SetActive(false);
            hideP2(true);
        }
    }

    public void twoPlayer()
    {
        if (GameManager.instance.activePlayerCount != 2)
        {
            AudioManager.instance.Play("duo_phrase");

            p1Outline.enabled = false;
            p2Outline.enabled = true;
            GameManager.instance.activePlayerCount = 2;

            p2Settings.SetActive(true);
            hideP2(false);
        }
    }

    public void startGame()
    {
        GameManager.instance.changeScene("Game Screen");
    }

    public void p1SkinUpdate()
    {
        foreach (Transform child in p1SkinPanel)
        {
            if (child.GetComponent<Toggle>() != null)
            {
                if (child.GetComponent<Toggle>().isOn)
                {
                    GameManager.instance.playerSkinColors[0] = child.GetComponent<Toggle>().GetComponentInChildren<Image>().color;
                    p1SkinImg.color = child.GetComponent<Toggle>().GetComponentInChildren<Image>().color;
                    break;
                }
            }
        }
    }

    public void p1OutfitUpdate()
    {
        foreach (Transform child in p1OutfitPanel)
        {
            if (child.GetComponent<Toggle>() != null)
            {
                if (child.GetComponent<Toggle>().isOn)
                {
                    GameManager.instance.playerOutfitColors[0] = child.GetComponent<Toggle>().GetComponentInChildren<Image>().color;
                    p1OutfitImg.color = child.GetComponent<Toggle>().GetComponentInChildren<Image>().color;
                    break;
                }
            }
        }
    }

    public void p2SkinUpdate()
    {
        foreach (Transform child in p2SkinPanel)
        {
            if (child.GetComponent<Toggle>() != null)
            {
                if (child.GetComponent<Toggle>().isOn)
                {
                    GameManager.instance.playerSkinColors[1] = child.GetComponent<Toggle>().GetComponentInChildren<Image>().color;
                    p2SkinImg.color = child.GetComponent<Toggle>().GetComponentInChildren<Image>().color;
                    break;
                }
            }
        }
    }

    public void p2OutfitUpdate()
    {
        foreach (Transform child in p2OutfitPanel)
        {
            if (child.GetComponent<Toggle>() != null)
            {
                if (child.GetComponent<Toggle>().isOn)
                {
                    GameManager.instance.playerOutfitColors[1] = child.GetComponent<Toggle>().GetComponentInChildren<Image>().color;
                    p2OutfitImg.color = child.GetComponent<Toggle>().GetComponentInChildren<Image>().color;
                    break;
                }
            }
        }
    }

    public void hideP2(bool hide)
    {
        if (hide)
        {
            foreach (Image img in p2Images)
            {
                Color color = img.color;
                color.a = 0.3f;
                img.color = color;
            }
        }
        else
        {
            foreach (Image img in p2Images)
            {
                Color color = img.color;
                color.a = 1f;
                img.color = color;
            }
        }
    }

    public void assignActiveColors()
    {
        foreach (ToggleGroup t in toggleGroups)
        {
            t.allowSwitchOff = true;
        }

        foreach (Transform child in p1OutfitPanel)
        {
            if (child.GetComponent<Toggle>() != null)
            {
                if (child.GetComponent<Toggle>().GetComponentInChildren<Image>().color != GameManager.instance.playerOutfitColors[0])
                {
                    child.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    child.GetComponent<Toggle>().isOn = true;
                }
            }
        }

        foreach (Transform child in p1SkinPanel)
        {
            if (child.GetComponent<Toggle>() != null)
            {
                if (child.GetComponent<Toggle>().GetComponentInChildren<Image>().color != GameManager.instance.playerSkinColors[0])
                {
                    child.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    child.GetComponent<Toggle>().isOn = true;
                }
            }
        }

        foreach (Transform child in p2OutfitPanel)
        {
            if (child.GetComponent<Toggle>() != null)
            {
                if (child.GetComponent<Toggle>().GetComponentInChildren<Image>().color != GameManager.instance.playerOutfitColors[1])
                {
                    child.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    child.GetComponent<Toggle>().isOn = true;
                }
            }
        }

        foreach (Transform child in p2SkinPanel)
        {
            if (child.GetComponent<Toggle>() != null)
            {
                if (child.GetComponent<Toggle>().GetComponentInChildren<Image>().color != GameManager.instance.playerSkinColors[1])
                {
                    child.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    child.GetComponent<Toggle>().isOn = true;
                }
            }
        }

        foreach (ToggleGroup t in toggleGroups)
        {
            t.allowSwitchOff = false;
        }

        p1OutfitImg.color = GameManager.instance.playerOutfitColors[0];
        p1SkinImg.color = GameManager.instance.playerSkinColors[0];

        p2OutfitImg.color = GameManager.instance.playerOutfitColors[1];
        p2SkinImg.color = GameManager.instance.playerSkinColors[1];
    }
}
