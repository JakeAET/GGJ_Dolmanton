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

    [SerializeField] SpriteRenderer[] skinSprites;
    [SerializeField] SpriteRenderer[] outfitSprites;

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
    }
}
