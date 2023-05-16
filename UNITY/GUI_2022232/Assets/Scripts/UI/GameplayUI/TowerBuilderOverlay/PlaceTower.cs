using System;
using TowerDefense.Data.Towers;
using TowerDefense.Gameplay.Core;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaceTower : NetworkBehaviour
{
    private void Start()
    {
        _camera = Camera.main;

        _currentMap = PlayerInput.currentActionMap;
        _cancelBuildingAction = _currentMap.FindAction("Build");
        _cancelBuildingAction.performed += OnCancelBuilding;

        _acceptBuildingAction = _currentMap.FindAction("PlaceBuild");
        _acceptBuildingAction.performed += OnAcceptBuilding;
    }

    public void HandleBuildingPlacementRequest(TowerProperties towerToPlace)
    {
        _towerToPlace = towerToPlace;

        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            if ((_placeableMask.value & (1 << hit.collider.gameObject.layer)) > 0)
            {
                _towerModel = Instantiate(_towerToPlace.TowerModel, hit.point, Quaternion.identity);
                //PlaceBuildingServerRPC(GetTowerListIndex(towerToPlace), hit.point);
            }
        }
    }

    private void Update()
    {
        //if (IsServer) { return; }
        if (_towerModel == null) { return; }

        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            _towerModel.transform.position = hit.point;

            _isPlaceable = (_placeableMask.value & (1 << hit.collider.gameObject.layer)) > 0;
            if (_isPlaceable)
            {
                ChangeColor(_validMaterial);
            }
            else
            {
                ChangeColor(_invalidMaterial);
            }
        }
    }

   

    private void OnCancelBuilding(InputAction.CallbackContext context)
    {
        Destroy(_towerModel);
        _towerModel = null;
    }

    private void OnAcceptBuilding(InputAction.CallbackContext context)
    {
        if (_towerModel == null && _towerToPlace == null) { return; }
        

        if (_isPlaceable)
        {
            PlaceBuildingServerRPC(GetTowerListIndex(_towerToPlace), _towerModel.transform.position);
        }
        else
        {
            return;
        }

        Destroy(_towerModel);
        _towerModel = null;
        _towerToPlace = null;
    }


    [ServerRpc(RequireOwnership = false)]
    public void PlaceBuildingServerRPC(int TowerIndex, Vector3 spawnPoint)
    {
        TowerProperties towerToPlace = GetTowerPropFromIndex(TowerIndex);

        if (GameController.Instance.Money >= towerToPlace.TowerCost)
        {

            GameObject inst = Instantiate(towerToPlace.TowerObject, spawnPoint, Quaternion.identity);
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

    private void ChangeColor(Material newMaterial)
    {
        if(_towerModel == null) { return; }
        
        foreach (Transform modelPart in _towerModel.transform)
        {
            Material[] mats = new Material[modelPart.GetComponent<Renderer>().materials.Length];

            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = newMaterial;
            }

            modelPart.GetComponent<Renderer>().materials = mats;
        }
    }

    //Tower placeing
    private Camera _camera;
    private GameObject _towerModel = null;
    private TowerProperties _towerToPlace = null;
    private bool _isPlaceable = false;
    [SerializeField] private LayerMask _placeableMask = new LayerMask();
    //Input actions 
    private InputAction _cancelBuildingAction;
    private InputAction _acceptBuildingAction;
    private InputActionMap _currentMap;
    [SerializeField] private PlayerInput PlayerInput;
    //materials for the build
    [SerializeField] private Material _invalidMaterial;
    [SerializeField] private Material _validMaterial;
    //Buildable towers
    [SerializeField] private TowerTypeListSO TowerList;
    
}
