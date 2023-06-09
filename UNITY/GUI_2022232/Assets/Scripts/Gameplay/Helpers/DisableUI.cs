using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableUI : MonoBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] GameObject Weapons;
    GameObject Cameras;
    // Start is called before the first frame update
    void Start()
    {
        Cameras = GameObject.Find("Cameras");
        if (Cameras != null ) Cameras.SetActive(false);
        //Cameras.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) 
        { 
            UI.SetActive(!UI.activeSelf);
            Weapons.SetActive(!Weapons.activeSelf);
            Cameras.SetActive(!Cameras.activeSelf);
        }
    }
}
