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
    //[SerializeField] TMP_InputField userName;
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private Button hostBtn;
    //SetPlayerNames names = SetPlayerNames.Instance();

    private void Awake()
    {
        if (PlayerPrefs.GetString("ip")!=null)
        {
            ipAddress.text = PlayerPrefs.GetString("ip");
        }
        //if (PlayerPrefs.GetString("username") != null)
        //{
        //    userName.text = PlayerPrefs.GetString("username");
        //}
        UnityTransport unity = networkManager.GetComponent<UnityTransport>();
        //unity.ConnectionData.Address = ipAddress.text;
        this.enabled = false;
        serverBtn.onClick.AddListener(() => {            
            //unity.ConnectionData.Address = ipAddress.text;
            NetworkManager.Singleton.StartServer();           
        });

        clientBtn.onClick.AddListener(() => {
            PlayerPrefs.SetString("ip", ipAddress.text);
            //PlayerPrefs.SetString("username", userName.text);
            //names.AddPlayer(userName.text);
            unity.ConnectionData.Address = ipAddress.text;
            //names.name = userName.text;
            //names.AddPlayer(userName.text);
            NetworkManager.Singleton.StartClient();
            
        });

        hostBtn.onClick.AddListener(() => {
            PlayerPrefs.SetString("ip", ipAddress.text);
            //PlayerPrefs.SetString("username", userName.text);
            //names.AddPlayer(userName.text);
            //unity.ConnectionData.Address = ipAddress.text;
            //names.name = userName.text;
            //names.AddPlayer(userName.text);
            NetworkManager.Singleton.StartHost();
            
        });
    }
}
