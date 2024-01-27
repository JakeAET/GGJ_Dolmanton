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

    private Color disabledColor;

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
        p1Panel.color = GameManager.instance.p1OutfitColor;
        p2Panel.color = GameManager.instance.p2OutfitColor;
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
}
