using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.instance.currentMode == GameManager.gamemode.SinglePlayer)
        {
            p1Outline.enabled = true;
            p2Outline.enabled = false;
        }
        else if (GameManager.instance.currentMode == GameManager.gamemode.TwoPlayer)
        {
            p1Outline.enabled = false;
            p2Outline.enabled = true;
        }

        startButton.onClick.AddListener(startGame);
        p1Button.onClick.AddListener(onePlayer);
        p2Button.onClick.AddListener(twoPlayer);

        p1SkinUpdate();
        p1OutfitUpdate();
        p2SkinUpdate();
        p2OutfitUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onePlayer()
    {
        if(GameManager.instance.currentMode != GameManager.gamemode.SinglePlayer)
        {
            p1Outline.enabled = true;
            p2Outline.enabled = false;
            GameManager.instance.currentMode = GameManager.gamemode.SinglePlayer;

            p2Settings.SetActive(false);
            hideP2(true);
        }
    }

    public void twoPlayer()
    {
        if (GameManager.instance.currentMode != GameManager.gamemode.TwoPlayer)
        {
            p1Outline.enabled = false;
            p2Outline.enabled = true;
            GameManager.instance.currentMode = GameManager.gamemode.TwoPlayer;

            p2Settings.SetActive(true);
            hideP2(false);
        }
    }

    public void startGame()
    {
        SceneManager.LoadScene("Jake's Test");
    }

    public void p1SkinUpdate()
    {
        foreach (Transform child in p1SkinPanel)
        {
            if (child.GetComponent<Toggle>() != null)
            {
                if (child.GetComponent<Toggle>().isOn)
                {
                    GameManager.instance.p1SkinColor = child.GetComponent<Toggle>().GetComponentInChildren<Image>().color;
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
                    GameManager.instance.p1OutfitColor = child.GetComponent<Toggle>().GetComponentInChildren<Image>().color;
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
                    GameManager.instance.p2SkinColor = child.GetComponent<Toggle>().GetComponentInChildren<Image>().color;
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
                    GameManager.instance.p2OutfitColor = child.GetComponent<Toggle>().GetComponentInChildren<Image>().color;
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
}
