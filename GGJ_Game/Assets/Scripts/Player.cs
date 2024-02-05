using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName;

    public Rigidbody2D golfRB;
    public Transform slingshotPoint;

    public Color skinColor;
    public Color outfitColor;

    public int turnCount = 1;

    [SerializeField] Sprite p1FrontFace;
    [SerializeField] Sprite p1SideFace;

    [SerializeField] Sprite p2FrontFace;
    [SerializeField] Sprite p2SideFace;

    [SerializeField] Sprite p3FrontFace;
    [SerializeField] Sprite p3SideFace;

    [SerializeField] Sprite p4FrontFace;
    [SerializeField] Sprite p4SideFace;

    [SerializeField] GameObject frontFace;
    [SerializeField] GameObject sideFace;

    [SerializeField] SpriteRenderer[] skinSprites;
    [SerializeField] SpriteRenderer[] outfitSprites;

    private void Awake()
    {
        frontFace.SetActive(false);
        sideFace.SetActive(true);
    }

    private void Start()
    {
        foreach (SpriteRenderer s in skinSprites)
        {
            s.color = skinColor;
        }

        foreach (SpriteRenderer s in outfitSprites)
        {
            s.color = outfitColor;
        }

        if(playerName == "Player 1")
        {
            frontFace.GetComponent<SpriteRenderer>().sprite = p1FrontFace;
            sideFace.GetComponent<SpriteRenderer>().sprite = p1SideFace;
        }
        else if(playerName == "Player 2")
        {
            frontFace.GetComponent<SpriteRenderer>().sprite = p2FrontFace;
            sideFace.GetComponent<SpriteRenderer>().sprite = p2SideFace;
        }
        else if (playerName == "Player 3")
        {
            frontFace.GetComponent<SpriteRenderer>().sprite = p3FrontFace;
            sideFace.GetComponent<SpriteRenderer>().sprite = p3SideFace;
        }
        else if (playerName == "Player 4")
        {
            frontFace.GetComponent<SpriteRenderer>().sprite = p4FrontFace;
            sideFace.GetComponent<SpriteRenderer>().sprite = p4SideFace;
        }
    }

    public void switchFace(bool faceForward)
    {
        if (faceForward)
        {
            frontFace.SetActive(true);
            sideFace.SetActive(false);
        }
        else
        {
            frontFace.SetActive(false);
            sideFace.SetActive(true);
        }
    }
}
