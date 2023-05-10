using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Data.Towers;
using TowerDefense.Gameplay.Core;
using Unity.Netcode;
//using UnityEditor.Build.Content;
using UnityEngine;

public class PlaceTower : NetworkBehaviour
{
    private void Start()
    {
        _camera = Camera.main;
    }

    public void HandleBuildingPlacementRequest(TowerProperties towerToPlace)
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f,0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) 
        {
            if ((_placeableMask.value & (1 << hit.collider.gameObject.layer)) > 0)
            {
                PlaceBuildingServerRPC(GetTowerListIndex(towerToPlace), hit.point);
            }  
        }        
    }
    [ServerRpc(RequireOwnership = false)]
    public void PlaceBuildingServerRPC(int TowerIndex, Vector3 spawnPoint)
    {
        TowerProperties towerToPlace = GetTowerPropFromIndex(TowerIndex);

        if (GameController.Instance.Money >= towerToPlace.TowerCost)
        {
           
            GameObject inst = Instantiate(towerToPlace.TowerRender, spawnPoint, Quaternion.identity);
            NetworkObject placingTowerNetworked = inst.GetComponent<NetworkObject>();
            placingTowerNetworked.Spawn(true);
            
            GameController.Instance.DecrementMoney(towerToPlace.TowerCost);
        }
        else
        {
            Debug.Log("Not enough money.");
        }
    }


    //helpers
    private int GetTowerListIndex(TowerProperties SoToFind)
    {
        return TowerList.TowerSOList.IndexOf(SoToFind);
    }

    private TowerProperties GetTowerPropFromIndex(int index)
    {
        return TowerList.TowerSOList[index];
    }

    [SerializeField] private Camera _camera;
    [SerializeField] private TowerTypeListSO TowerList;
    [SerializeField] private LayerMask _placeableMask = new LayerMask();
}
