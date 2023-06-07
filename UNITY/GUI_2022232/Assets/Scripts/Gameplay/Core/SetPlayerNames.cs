using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class SetPlayerNames : NetworkBehaviour
{
    [SerializeField] TMP_Text playerName;
    [SerializeField] TMP_InputField playerNameInput;

    //NetworkVariable<string> playerName = new NetworkVariable<string>();
    //string namex;

    //static SetPlayerNames instance;
    //protected SetPlayerNames() { }
    //public static SetPlayerNames Instance()
    //{
    //    if (instance == null)
    //    {
    //        instance = new SetPlayerNames();
    //    }
    //    return instance;
    //}
    //public void AddPlayer(string name)
    //{
    //    playerName.Value = name;
    //}

    //public override void OnNetworkSpawn()
    //{   
    //    NetworkObject networkObject = GetComponent<NetworkObject>();
    //    AddPlayerServerRpc(namex,networkObject);
    //}   

    [ServerRpc(RequireOwnership = false)]
    private void AddPlayerServerRpc(string name)
    {        
        AddPlayerClientRpc(name);
    }

    [ClientRpc]
    private void AddPlayerClientRpc(string name)
    {
        playerName.text = name;
        //if (!IsOwner) return;
        //((GameObject)networkObject).GetComponent<SetPlayerNames>().SetName();    
    }

    public void SyncName()
    {
        AddPlayerServerRpc(playerNameInput.text);
    }
}
