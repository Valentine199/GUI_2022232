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
using System.Linq;
using Unity.Netcode;

public class TowerUpgradeInteract : NetworkBehaviour, ConflictDetectorInterface
{

    private void Awake()
    {
        _currentMap = PlayerInput.currentActionMap;
        _interactAction = _currentMap.FindAction("Interact");
        _interactAction.performed += onInteract;
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    private void onInteract(InputAction.CallbackContext context)
    {
        if(OtherIsOpen() || !IsOwner) { return; }
        ToggleInteractCanvas();
        
    }

    private void ToggleInteractCanvas()
    {
        
        if(!InteractCanvas.activeInHierarchy)
        {
            Ray r = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(r,out RaycastHit hitInfo, _interactRange))
            {
                Debug.Log(hitInfo.collider.gameObject.name);
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    //interactObj.InteractUpgrade();
                    InteractCanvas.SetActive(true);
                    InteractCanvas.GetComponent<TowerInteractUI>().InitSelf(interactObj);
                    
                }
            }
        }
        else
        {
            InteractCanvas.GetComponent<TowerInteractUI>().CloseSelf(false);
            
        }
    }

    public bool OtherIsOpen()
    {
        return _conflictCanvas.Any(x => x.activeInHierarchy);
    }



    private Camera _camera;
    [SerializeField] private float _interactRange;
    private Transform _transform;
    [SerializeField] private PlayerInput PlayerInput;
    [SerializeField] private GameObject InteractCanvas;
    [SerializeField] private GameObject[] _conflictCanvas;
    

    private InputAction _interactAction;
    private InputActionMap _currentMap;
}

public interface IInteractable
{
    public event Action<TowerUpgrade> OnNewUpgrade;
    public event Action OnTargetingStyleChange;

    public void ShowTowerRange();
    public string ShowName();
    public void HideTowerRange();

    public void CycleTargetingStyleForward();
    public void CycleTargetingStyleBackwards();
    public TargetingStyle GetTargetingInfo();


    public TowerUpgrade GetUpgradeInfo();
    public void BuyUpgrade();
    public Camera GetSnapshotCam();

    public int GetSellPrice();
    public void SellTower();
}