using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsStart : MonoBehaviour
{

    public GameObject Settings;
    public GameObject AudioSettings;
    public GameObject LoadSettings;

    public void OpenSettingsAudio() // button click to open Audio Settings
    {
        Settings.SetActive(true);
        AudioSettings.SetActive(true);
        LoadSettings.SetActive(false);
    }
    
    public void OpenSettingsLoad() // button click to open Load Settings
    {
        Settings.SetActive(true);
        AudioSettings.SetActive(false);
        LoadSettings.SetActive(true);
    }
    
    public void CloseSettings() // button click to close Settings
    {
        Settings.SetActive(false);
    }
    
}
