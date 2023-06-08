using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ChangeClothesColor : NetworkBehaviour
{
    [SerializeField] Material material1;
    [SerializeField] Material material2;
    [SerializeField] Material material3;
    [SerializeField] Material material4;
    [SerializeField] Material material5;    
    [SerializeField] TMP_Dropdown dropdownItem;
    public List<List<Material>> materials;
    

    private void Start()
    {
        if (!IsOwner) return;
        
        materials = new List<List<Material>>();
        for (int i = 0; i < 5; i++)
        {
            materials.Add(new List<Material>());
        }
        
        materials[0].Add(material1);
        materials[1].Add(material2);
        materials[2].Add(material3);
        materials[3].Add(material4);
        materials[4].Add(material5);
        //foreach (var item in transform)
        //{
        //    Debug.Log(transform.name);
        //    transform.gameObject.GetComponent<SkinnedMeshRenderer>().materials[0] = list[Random.Range(0, 4)];
        //}
        //int randint = Random.Range(0, 4);
        //for (int i = 0; i < 3; i++)
        //{
        //    transform.GetChild(i).gameObject.GetComponent<SkinnedMeshRenderer>().SetMaterials(materials[randint]);
        //}                    
    }

    public void SyncColor(int listid)
    {
        SyncColorServerRpc(dropdownItem.value);
        //Debug.Log(dropdownItem.value);
    }
    [ServerRpc(RequireOwnership = false)]
    void SyncColorServerRpc(int listid)
    {
        SyncColorClientRpc(listid);
    }
    [ClientRpc]
    private void SyncColorClientRpc(int listid)
    {
        List<List<Material>> materials = new List<List<Material>>();
        for (int i = 0; i < 5; i++)
        {
            materials.Add(new List<Material>());
        }

        materials[0].Add(material1);
        materials[1].Add(material2);
        materials[2].Add(material3);
        materials[3].Add(material4);
        materials[4].Add(material5);
        
        foreach (Transform cloth in transform)
        {
            cloth.gameObject.GetComponent<SkinnedMeshRenderer>().SetMaterials(materials[listid]);
        }
        //for (int i = 0; i < 3; i++)
        //{
        //    transform.GetChild(i).gameObject.GetComponent<SkinnedMeshRenderer>().SetMaterials(materials[listid]);
        //}        
    }

}
