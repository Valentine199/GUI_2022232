using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdjustAudio : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] TMP_Text volumeText;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume", 0.1f);
            Load();
        }
        else
        {
            Load();
        }
    }
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        volumeText.text = $"Volume:{(int)(volumeSlider.value*100)}"; 
        Save();
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("volume",volumeSlider.value);
    }

    void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
    }
}
