using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TapToStart : MonoBehaviour
{

    public void nextScene()
    {
        changeScene("Title Screen", true);
    }

    void changeScene(string sceneName, bool continueMusic = false)
    {
        AudioManager.instance.sceneChanged(sceneName, continueMusic);
        SceneManager.LoadScene(sceneName);
    }
}
