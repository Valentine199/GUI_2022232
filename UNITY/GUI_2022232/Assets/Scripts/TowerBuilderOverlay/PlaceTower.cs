using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceTower : MonoBehaviour
{

    private void Start()
    {
        _camera = Camera.main;
    }

    public void PlaceBuilding(GameObject towerToPlace)
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f,0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) 
        {
            if (hit.collider.gameObject.layer == 3)
            {
                Instantiate(towerToPlace, hit.point, Quaternion.identity);
            }  
        }        
    }



    private Camera _camera;
    [SerializeField] private GameObject _towerToPlace;
    //[SerializeField] private LayerMask placeableMask = new LayerMask();
}
