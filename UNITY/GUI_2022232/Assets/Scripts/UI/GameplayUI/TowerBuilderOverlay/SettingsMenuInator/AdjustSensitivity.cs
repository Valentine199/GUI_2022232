using System.Collections;
using System.Collections.Generic;
using TMPro;
using TowerDefense.Gameplay.Core;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class AdjustSensitivity : MonoBehaviour
{
    [SerializeField] TMP_InputField sensiInput;
    [SerializeField] Component player;
    PlayerController playerController;
    

    private void Start()
    {
        playerController = (PlayerController)player;

        Load();
        //if (!PlayerPrefs.HasKey("sensitivity"))
        //{
        //    PlayerPrefs.SetFloat("sensitivity", 10f);
        //    Load();
        //}
        //else
        //{
        //    Load();
        //}
        
    }
    public void ChangeSensi()
    {
        playerController.MouseSensitivity = float.Parse(sensiInput.text);
        sensiInput.text = playerController.MouseSensitivity.ToString();
        
        //volumeText.text = $"Volume:{(int)(volumeSlider.value * 100)}";
        Save();
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("sensitivity", float.Parse(sensiInput.text));
    }

    void Load()
    {
        sensiInput.text = PlayerPrefs.GetFloat("sensitivity").ToString();
    }
}
