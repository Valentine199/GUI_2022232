using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Burst.Intrinsics.X86;
using static UnityEditor.Progress;
using UnityEngine.Windows;

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
        Debug.Log("na");
        Ray r = new Ray(_transform.position, _transform.forward);
        if (Physics.Raycast(r,out RaycastHit hitInfo, _interactRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                interactObj.Interact();
            }
        }
    }

    public static event Action OnUpgradeCanvasToggled;

    [SerializeField] private float _interactRange;
    public Transform _transform;
    [SerializeField] private PlayerInput PlayerInput;

    private bool _toggle = false;

    private InputAction _interactAction;
    private InputActionMap _currentMap;
}

interface IInteractable
{
    public void Interact();
}