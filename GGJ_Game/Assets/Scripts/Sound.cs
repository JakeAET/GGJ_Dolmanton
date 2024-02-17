using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    //By Jake Gollub

    public string name; // this will be used for calling specific sounds

    public AudioClip clip;

    [Range(0, 256)]
    public int priority = 128;
    [Range(0f, 1f)]
    public float volume = 1;
    [Range(.1f, 3f)]
    public float pitch = 1;
    public bool loop;

    // type of sound
    public enum type { music, sfx, catchphrase, proximity, victory, chant};
    public enum playerNum { NA, one, two, three, four };
    public type soundType;
    public playerNum soundPlayerNum;

    [HideInInspector]
    public AudioSource source;
}