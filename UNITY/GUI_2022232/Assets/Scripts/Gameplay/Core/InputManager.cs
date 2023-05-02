using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace TowerDefense.Gameplay.Core
{
    public class InputManager : NetworkBehaviour
    {
        [SerializeField] private PlayerInput PlayerInput;
        [SerializeField] private PlayerControlls _playerControlls;

        public Vector2 Move { get; private set; }

        public Vector2 Look { get; private set; }

        public bool Run { get; private set; }

        public bool Jump { get; private set; }

        public bool Crouch {get; private set;}


        private InputActionMap _currentMap;

        private InputAction _moveAction;

        private InputAction _lookAction;

        private InputAction _runAction;

        private InputAction _jumpAction;

        private InputAction _crouchAction;

        private void Awake()
        {
            //if (!IsOwner) return;
            HideCursor();
            _currentMap = PlayerInput.currentActionMap;
            _moveAction = _currentMap.FindAction("Move");
            _lookAction = _currentMap.FindAction("Look");
            _runAction = _currentMap.FindAction("Run");
            _jumpAction = _currentMap.FindAction("Jump");
            _crouchAction = _currentMap.FindAction("Crouch");

            _moveAction.performed += onMove;
            _lookAction.performed += onLook;
            _runAction.performed += onRun;
            _jumpAction.performed += onJump;
            _crouchAction.started += onCrouch;

            _moveAction.canceled += onMove;
            _lookAction.canceled += onLook;
            _runAction.canceled += onRun;
            _jumpAction.canceled += onJump;
            _crouchAction.canceled += onCrouch;

            //_playerControlls = new PlayerControlls();
        }

        private void HideCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void onMove(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }
        private void onLook(InputAction.CallbackContext context)
        {
            Look = context.ReadValue<Vector2>();
        }
        private void onRun(InputAction.CallbackContext context)
        {
            Run = context.ReadValueAsButton();
        }
        private void onJump(InputAction.CallbackContext context)
        {           
            Jump = context.ReadValueAsButton();
        }
        private void onCrouch(InputAction.CallbackContext context)
        {
            Crouch = context.ReadValueAsButton();
        }

        //public Vector2 GetMouseDelta()
        //{
        //    return _playerControlls.Player.Look.ReadValue<Vector2>();
        //}

        private void OnEnable()
        {
            _currentMap.Enable();
            //_playerControlls.Enable();
        }

        private void OnDisable()
        {
            _currentMap.Disable();
            //_playerControlls.Disable();
        }
    }
}
