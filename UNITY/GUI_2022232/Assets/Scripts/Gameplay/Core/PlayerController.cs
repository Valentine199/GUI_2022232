using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TowerDefense.Gameplay.Core;

namespace TowerDefense.Gameplay.Core
{
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private float AnimBlendSpeed = 8.9f;
        [SerializeField] private Transform CameraRoot;
        [SerializeField] private Transform Camera;
        [SerializeField] private float UpperLimit = -40f;
        [SerializeField] private float BottomLimit = 70f;
        [SerializeField] private float MouseSensitivity = 21.9f;
        [SerializeField] private float JumpFactor = 260f;
        [SerializeField] private float DistanceToGround = 0.85f;
        [SerializeField] private float AirResistance = 0.8f;
        [SerializeField] private LayerMask GroundCheck;        

        private Rigidbody _playerRigidbody;

        private InputManager _inputManager;

        private Animator _animator;

        private bool _hasAnimator;

        private bool _cameraFreezed = false;

        private int _xVelHash;

        private int _yVelHash;

        private int _zVelHash;

        private int _crouchVelHash;

        private int _jumpHash;

        private bool _grounded = false;

        private int _groundHash;

        private int _fallingHash;

        private float _xRotation;


        private const float _walkSpeed = 3f;

        private const float _runSpeed = 9f;

        private Vector2 _currentVelocity;

        //public bool CameraFreezed { get { return _cameraFreezed; } set { _cameraFreezed = value; } }

        private void OnEnable()
        {
            HandlePlaceCanvas.OnBuildingsCanvasToggled += ToggleFreezeCam;
        }

        private void OnDisable()
        {
            HandlePlaceCanvas.OnBuildingsCanvasToggled -= ToggleFreezeCam;
        }

        private void Start()
        {
            _hasAnimator = TryGetComponent<Animator>(out _animator);
            _playerRigidbody = GetComponent<Rigidbody>();
            _inputManager = GetComponent<InputManager>();

            _xVelHash = Animator.StringToHash("X_Velocity");
            _yVelHash = Animator.StringToHash("Y_Velocity");
            _jumpHash = Animator.StringToHash("Jump");
            _groundHash = Animator.StringToHash("Grounded");
            _fallingHash = Animator.StringToHash("Falling");
            _zVelHash = Animator.StringToHash("Z_Velocity");
            _crouchVelHash = Animator.StringToHash("Crouch");
        }

        private void FixedUpdate()
        {
            SampleGround();
            Move();
            HandleJump();
            HandleCrouch();
        }

        private void LateUpdate()
        {
            CamMovements();
        }

        private void ToggleFreezeCam()
        {
            _cameraFreezed = !_cameraFreezed;
        }

        private void Move()
        {
            if (!_hasAnimator) return;

            float tartgetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
            if (_inputManager.Crouch) tartgetSpeed = 1.5f;
            
            if (_inputManager.Move == Vector2.zero) tartgetSpeed = 0;

            if (_grounded)
            {
                _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _inputManager.Move.x * tartgetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);
                _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, _inputManager.Move.y * tartgetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);

                var xVelDifference = _currentVelocity.x - _playerRigidbody.velocity.x;
                var zVelDifference = _currentVelocity.y - _playerRigidbody.velocity.z;

                _playerRigidbody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, zVelDifference)), ForceMode.VelocityChange);

            }
            else
            {
                _playerRigidbody.AddForce(transform.TransformVector(new Vector3(_currentVelocity.x* AirResistance,0,_currentVelocity.y * AirResistance)),ForceMode.VelocityChange);
            }
            
            _animator.SetFloat(_xVelHash, _currentVelocity.x);
            _animator.SetFloat(_yVelHash, _currentVelocity.y);

        }

        private void CamMovements()
        {
            if (!_hasAnimator) return;
            if (_cameraFreezed)  return; 

            var Mouse_X = _inputManager.Look.x;
            var Mouse_Y = _inputManager.Look.y;
            Camera.position = CameraRoot.position;

            _xRotation -= Mouse_Y * MouseSensitivity * Time.deltaTime;
            _xRotation = Mathf.Clamp(_xRotation, UpperLimit, BottomLimit);

            Camera.localRotation = Quaternion.Euler(_xRotation,0,0);
            _playerRigidbody.MoveRotation(_playerRigidbody.rotation * Quaternion.Euler(0, Mouse_X * MouseSensitivity * Time.smoothDeltaTime, 0));
        }

        private void HandleCrouch()
        {
            _animator.SetBool(_crouchVelHash, _inputManager.Crouch);
        }

        private void HandleJump()
        {
            if (!_hasAnimator) return;
            if (!_inputManager.Jump) return;
            if (!_grounded) return;            
            _animator.SetTrigger(_jumpHash);       
        }
            
        public void JumpAddForce()
        {
            _playerRigidbody.AddForce(-_playerRigidbody.velocity.y*Vector3.up,ForceMode.VelocityChange);
            _playerRigidbody.AddForce(Vector3.up * JumpFactor, ForceMode.Impulse);
            _animator.ResetTrigger(_jumpHash);
        }

        private void SampleGround()
        {
            if (!_hasAnimator) return;

            RaycastHit hitInfo;
            if (Physics.Raycast(_playerRigidbody.worldCenterOfMass, Vector3.down, out hitInfo, DistanceToGround + 0.1f, GroundCheck))
            {
                //Grounded
                _grounded = true;
                SetAnimationGrounding();
                return;
            }
            //Falling
            _grounded = false;
            _animator.SetFloat(_zVelHash, _playerRigidbody.velocity.y);
            SetAnimationGrounding();
            return;
        }

        private void SetAnimationGrounding()
        {
            _animator.SetBool(_fallingHash, !_grounded);
            _animator.SetBool(_groundHash, _grounded);
        }
    }
}
