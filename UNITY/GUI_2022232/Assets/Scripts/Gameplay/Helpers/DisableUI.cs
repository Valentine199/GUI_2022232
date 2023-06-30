using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DisableUI : NetworkBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] GameObject Weapons;
    GameObject Cameras;
    // Start is called before the first frame update
    void Start()
    {
        Cameras = GameObject.Find("Cameras");
        //Cameras.SetActive(false);
        //Cameras.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Cameras == null) return;
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (!IsOwner) return;
            UI.SetActive(true);
            Weapons.SetActive(true);
            Cameras.SetActive(false);            
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (!IsOwner) return;
            UI.SetActive(false);
            Weapons.SetActive(false);
            Cameras.SetActive(true);
        }
    }
}
