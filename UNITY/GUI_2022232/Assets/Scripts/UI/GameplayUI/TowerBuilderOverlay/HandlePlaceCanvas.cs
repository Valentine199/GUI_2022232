using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Gameplay.Core;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandlePlaceCanvas : MonoBehaviour
{
    private void Awake()
    {
        _currentMap = PlayerInput.currentActionMap;
        _buildAction = _currentMap.FindAction("Build");
        _buildAction.performed += onBuild;
        foreach (GameObject obj in _towers)
        {
            GameObject instance =  Instantiate(_buildMenuItem);
            HandleOnClick InstanceHandleClick = instance.GetComponent<HandleOnClick>();

            if (InstanceHandleClick != null)
            {
                InstanceHandleClick.MyTower = obj;
                InstanceHandleClick.OnClickSetCanvas += ToggleBuildingsCanvas;
                InstanceHandleClick.OnClickPlaceBuilding += gameObject.GetComponent<PlaceTower>().PlaceBuilding;
            }

            instance.transform.SetParent(_buildBg.transform, false);
        }
    }

    private void onBuild(InputAction.CallbackContext context)
    {
        ToggleBuildingsCanvas();
        OnBuildingsCanvasToggled?.Invoke();
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
    }

    public static event Action OnBuildingsCanvasToggled;

    [SerializeField] private Canvas _inGameCanvas;
    [SerializeField] private Canvas _selectionCanvas; // The canvas which handles the UI
    [SerializeField] private GameObject _buildBg;     // The direct parent to the build elements  
    [SerializeField] private GameObject[] _towers = new GameObject[0];  // A list of all possible buildings
    [SerializeField] private GameObject _buildMenuItem;  // What to instantiate as a building option
    [SerializeField] private PlayerInput PlayerInput;

    private bool _toggle = false;

    private InputAction _buildAction;
    private InputActionMap _currentMap;
}
