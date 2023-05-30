using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class SetPlayerNames : NetworkBehaviour
{
    [SerializeField] TMP_Text playerName;
    [SerializeField] TMP_InputField nameInput;
    public void SetNames()
    {
        SetNamesServerRpc();
    }
    [ServerRpc(RequireOwnership =false)]
    private void SetNamesServerRpc()
    {
        SetNamesClientRpc();
    }
    [ClientRpc]
    private void SetNamesClientRpc()
    {
        playerName.text = nameInput.text;
    }

    // Start is called before the first frame update
    void Start()
    {
        //nameInput.text = "GGGGGGGGGG";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
