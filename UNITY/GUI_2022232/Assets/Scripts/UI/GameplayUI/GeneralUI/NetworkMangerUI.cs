using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class NetworkMangerUI : MonoBehaviour
{
    [SerializeField] GameObject networkManager;
    [SerializeField] TMP_InputField ipAddress;
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private Button hostBtn;

    private void Awake()
    {
        //UnityTransport unity = networkManager.GetComponent<UnityTransport>();
        //unity.ConnectionData.Address = ipAddress.text;
        this.enabled = false;
        serverBtn.onClick.AddListener(() => {   
            NetworkManager.Singleton.StartServer();           
        });

        clientBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });

        hostBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
        });
    }
}
