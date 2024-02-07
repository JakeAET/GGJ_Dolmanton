using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening.Core.Easing;
using Unity.VisualScripting;
using System.Threading;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds; // sound class containing name, volume, pitch, loop bool, music bool, and the actual audio clip

    public static AudioManager instance;

    public bool sfxMuted;
    public bool musicMuted;

    public UnityEngine.UI.Toggle sfxToggle;
    public UnityEngine.UI.Toggle musicToggle;

    private List<string> p1CatchphraseNames = new List<string>();
    private List<string> p2CatchphraseNames = new List<string>();

    void Awake()
    {

        if (instance != null && instance != this)
        {

            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        // adds an audio source in the inspector for every audio source in the array, allowing you to manipulate each sound from the manager
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.priority = s.priority;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            if(s.soundType == Sound.type.p1Catchphrase)
            {
                p1CatchphraseNames.Add(s.name);
            }

            if (s.soundType == Sound.type.p2Catchphrase)
            {
                p2CatchphraseNames.Add(s.name);
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "Title Screen")
        {
            Play("menu_music");
        }
        else if(SceneManager.GetActiveScene().name == "Game Screen")
        {
            Play("game_music");
        }
    }

    private void Update()
    {
        if(musicToggle == null)
        {
            if (GameObject.FindGameObjectWithTag("music toggle") != null)
            {
                musicToggle = GameObject.FindGameObjectWithTag("music toggle").GetComponent<UnityEngine.UI.Toggle>();
                musicToggle.isOn = !musicMuted;
                musicToggle.onValueChanged.AddListener(delegate
                {
                    muteMusic(!musicToggle.isOn);
                });
                GameObject.FindGameObjectWithTag("music toggle").GetComponent<AudioToggle>().toggleSetting(!musicMuted);

            }
        }

        if(sfxToggle == null)
        {
            if (GameObject.FindGameObjectWithTag("sfx toggle") != null)
            {
                sfxToggle = GameObject.FindGameObjectWithTag("sfx toggle").GetComponent<UnityEngine.UI.Toggle>();
                sfxToggle.isOn = !sfxMuted;
                sfxToggle.onValueChanged.AddListener(delegate
                {
                    muteSFX(!sfxToggle.isOn);
                });
                GameObject.FindGameObjectWithTag("sfx toggle").GetComponent<AudioToggle>().toggleSetting(!sfxMuted);
            }
        }
    }

    // method to call for playing a certain sound
    public void Play(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void PlayCustomPitch(string name, float p)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        //float startPitch = s.source.pitch;
        s.source.pitch = p;
        s.source.Play();
        //s.source.pitch = startPitch;
    }

    // method to call for pausing a certain sound
    public void Pause(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        s.source.Pause();
    }

    public void Stop(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public void muteMusic(bool mute)
    {
        foreach (Sound s in sounds)
        {
            if (s.soundType == Sound.type.music)
            {
                s.source.mute = mute;
            }
        }

        musicMuted = mute;

        GameObject.FindGameObjectWithTag("music toggle").GetComponent<AudioToggle>().toggleSetting(!musicMuted);
    }

    public void muteSFX(bool mute)
    {
        foreach (Sound s in sounds)
        {
            if (s.soundType == Sound.type.sfx)
            {
                s.source.mute = mute;
            }
        }

        sfxMuted = mute;

        GameObject.FindGameObjectWithTag("sfx toggle").GetComponent<AudioToggle>().toggleSetting(!sfxMuted);
    }

    public void sceneChanged(string sceneName, bool continueMusic)
    {
        if(sceneName == "Title Screen")
        {
            stopAllSFX();
            Stop("game_music");
            Play("menu_music");
        }

        if(sceneName == "Game Screen")
        {
            stopAllSFX();

            if (!continueMusic)
            {
                Stop("menu_music");
                Play("game_music");
            }
        }
    }

    public void stopAllSFX()
    {
        foreach (Sound s in sounds)
        {
            if (s.soundType == Sound.type.sfx)
            {
                Stop(s.name);
            }
        }
    }

    public void playCatchphrase(string playerName)
    {
        if(playerName == "Player 1")
        {
            Play(p1CatchphraseNames[Random.Range(0, p1CatchphraseNames.Count)]);
        }
        else if(playerName == "Player 2")
        {
            Play(p2CatchphraseNames[Random.Range(0, p2CatchphraseNames.Count)]);
        }
    }
}
