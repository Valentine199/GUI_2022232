using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Data.Towers;
using Unity.VisualScripting;
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
            if (hit.collider.gameObject.layer == 3)
            {
                Instantiate(towerToPlace.TowerRender, hit.point, Quaternion.identity);
            }  
        }        
    }



    [SerializeField]private Camera _camera;
    //[SerializeField] private LayerMask placeableMask = new LayerMask();
}
