using System;
using System.Collections.Generic;
using TowerDefense.Data.Towers;
using TowerDefense.Gameplay.Core;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandlePlaceCanvas : NetworkBehaviour
{
    private void Awake()
    {
        WaveController.Instance.OnWaveStarted += DisableStartNewButton;
        WaveController.Instance.OnWaveCompleted += EnableStartNewWaveButton;

        _currentMap = PlayerInput.currentActionMap;
        _buildAction = _currentMap.FindAction("Build");
        _buildAction.performed += onBuild;
        List<TowerProperties> towers = TowerList.TowerSOList;
        foreach (TowerProperties prop in towers)
        {
            GameObject instance =  Instantiate(_buildMenuItem);
            HandleOnClick InstanceHandleClick = instance.GetComponent<HandleOnClick>();

            if (InstanceHandleClick != null)
            {
                InstanceHandleClick.MyTower = prop;
                InstanceHandleClick.OnClickSetCanvas += ToggleBuildingsCanvas;
                InstanceHandleClick.OnClickPlaceBuilding += gameObject.GetComponent<PlaceTower>().HandleBuildingPlacementRequest;
            }

            instance.transform.SetParent(_buildBg.transform, false);
        }
    }

    private void onBuild(InputAction.CallbackContext context)
    {
        if(!IsOwner) { return; }
        ToggleBuildingsCanvas();
    }

    private void EnableStartNewWaveButton()
    {
        _startNewWaveButton.SetActive(true);
    }

    private void DisableStartNewButton()
    {
        _startNewWaveButton.SetActive(false);
    }

    public void ToggleBuildingsCanvas()
    {
        _toggle = !_toggle;
        //GetComponent<PlayerController>().CameraFreezed = _toggle;

        if (_toggle)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        
        _selectionCanvas.gameObject.SetActive(_toggle);
        _inGameCanvas.gameObject.SetActive(!_toggle);
        OnBuildingsCanvasToggled?.Invoke();
    }

    public event Action OnBuildingsCanvasToggled;

    [SerializeField] private Canvas _inGameCanvas;
    [SerializeField] private Canvas _selectionCanvas; // The canvas which handles the UI
    [SerializeField] private GameObject _buildBg;     // The direct parent to the build elements  
    //[SerializeField] private TowerProperties[] _towers = new TowerProperties[0];  // A list of all possible buildings | Only usefull if we were to give each player a separate set of buildings
    [SerializeField] private TowerTypeListSO TowerList; //instead of _towers
    [SerializeField] private GameObject _buildMenuItem;  // What to instantiate as a building option
    [SerializeField] private PlayerInput PlayerInput;

    [SerializeField] private GameObject _startNewWaveButton;

    private bool _toggle = false;

    private InputAction _buildAction;
    private InputActionMap _currentMap;
}
