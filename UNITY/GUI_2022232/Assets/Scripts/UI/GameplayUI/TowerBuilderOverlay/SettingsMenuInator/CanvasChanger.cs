using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasChanger : MonoBehaviour
{
    [SerializeField] GameObject main;
    [SerializeField] GameObject options;

    private void Start()
    {
        options.SetActive(false);
    }

    public void SwitchToMain()
    {        
        options.SetActive(false);
        main.SetActive(true);
    }
    public void SwitchToOptions()
    {
        main.SetActive(false);
        options.SetActive(true);
    }

    public void SelfDisable(string gameObjectToDisable)
    {
        gameObject.transform.Find(gameObjectToDisable).gameObject.SetActive(false);
    }

    public void EnableCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void DisableCursor() 
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
