using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Data.Towers;
using TowerDefense.Gameplay.Core;
using UnityEditor.Build.Content;
using UnityEngine;

public class PlaceTower : MonoBehaviour
{
    private void Start()
    {
        _camera = Camera.main;
    }

    public void PlaceBuilding(TowerProperties towerToPlace)
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f,0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) 
        {
            if ((_placeableMask.value & (1 << hit.collider.gameObject.layer)) > 0)
            {
                if (GameController.Instance.Money >= towerToPlace.TowerCost)
                {
                    Instantiate(towerToPlace.TowerRender, hit.point, Quaternion.identity);
                    GameController.Instance.DecrementMoney(towerToPlace.TowerCost);
                }
                else
                {
                    Debug.Log("Not enough money.");
                }
            }  
        }        
    }

    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _placeableMask = new LayerMask();
}
