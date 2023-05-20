using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Burst.Intrinsics.X86;
//using static UnityEditor.Progress;
using UnityEngine.Windows;
using TowerDefense.Towers.TowerUpgrades;
using TowerDefense.Towers.TowerEnums;

public class TowerUpgradeInteract : MonoBehaviour
{

    private void Awake()
    {
        _currentMap = PlayerInput.currentActionMap;
        _interactAction = _currentMap.FindAction("Interact");
        _interactAction.performed += onInteract;
    }       
    
    private void onInteract(InputAction.CallbackContext context)
    {
        ToggleInteractCanvas();
        OnUpgradeCanvasToggled?.Invoke();
    }

    private void ToggleInteractCanvas()
    {
        _toggle = !_toggle;
        
        if(!InteractCanvas.activeInHierarchy)
        {
            _transform = this.transform;
            Ray r = new Ray(_transform.position, _transform.forward);
            if (Physics.Raycast(r,out RaycastHit hitInfo, _interactRange))
            {
                Debug.Log(hitInfo.collider.gameObject.name);
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    //interactObj.InteractUpgrade();
                    InteractCanvas.SetActive(true);
                    InteractCanvas.GetComponent<TowerInteractUI>().InitSelf(interactObj);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        }
        else
        {
            InteractCanvas.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public event Action OnUpgradeCanvasToggled;

    [SerializeField] private float _interactRange;
    private Transform _transform;
    [SerializeField] private PlayerInput PlayerInput;
    [SerializeField] private GameObject InteractCanvas;

    private bool _toggle = false;

    private InputAction _interactAction;
    private InputActionMap _currentMap;
}

public interface IInteractable
{
    public event Action<TowerUpgrade> OnNewUpgrade;
    public event Action OnTargetingStyleChange;

    public void CycleTargetingStyleForward();
    public void CycleTargetingStyleBackwards();
    public TargetingStyle GetTargetingInfo();


    public TowerUpgrade GetUpgradeInfo();
    public void InteractUpgradeServerRpc();

    public int GetSellPrice();
    public void SellTower();
}