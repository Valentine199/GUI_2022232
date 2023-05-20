using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class WeaponSwitching : NetworkBehaviour
{
    public int selectedWeapon = 0;


    void Start()
    {
        selectedWeapon = 0;
        SelectWeapon();
    }

    
    void Update()
    {
        if (!IsOwner) return;

        int prev = selectedWeapon;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedWeapon = 2;
        }
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    selectedWeapon = 3;
        //}

        if (prev != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
        //Debug.Log("KInt:"+selectedWeapon);
        SelectWeaponServerRpc(selectedWeapon);
                
    }

    [ServerRpc(RequireOwnership = false)]
    void SelectWeaponServerRpc(int xd)
    {
        //Debug.Log("Közép:" + selectedWeapon);
        SelectWeaponClientRpc(xd);
    }

    [ClientRpc]
    void SelectWeaponClientRpc(int xd)
    {
        if (IsOwner) return;
        //Debug.Log("Bent" + xd);
        selectedWeapon = xd;
        int i = 0;
        foreach (Transform weapon in transform)
        {
            
            
            
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }

    }
}
