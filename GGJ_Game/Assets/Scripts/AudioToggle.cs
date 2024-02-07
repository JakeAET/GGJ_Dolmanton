using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioToggle : MonoBehaviour
{
    public string type;

    [SerializeField] GameObject offObj;
    [SerializeField] GameObject onObj;

    public void toggleSetting(bool on)
    {
        if (on)
        {
            offObj.SetActive(false);
            onObj.SetActive(true);
        }
        else
        {
            offObj.SetActive(true);
            onObj.SetActive(false);
        }
    }
}
