using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TowerDefense.Gameplay.Core;
using Unity.Netcode;
using Cinemachine;

namespace TowerDefense.Gameplay.Core
{
    public class PlayerController : NetworkBehaviour
    {

        [SerializeField] private float AnimBlendSpeed = 8.9f;
        [SerializeField] private Transform CameraRoot;
        [SerializeField] private Transform Camera;
        [SerializeField] private GameObject cam;
        [SerializeField] private float UpperLimit = -40f;
        [SerializeField] private float BottomLimit = 70f;
        [SerializeField] private float MouseSensitivity = 21.9f;
        [SerializeField] private float JumpFactor = 260f;
        [SerializeField] private float DistanceToGround = 0.85f;
        [SerializeField] private float AirResistance = 0.8f;
        [SerializeField] private float jumpDelay = 1f;
        [SerializeField] private LayerMask GroundCheck;
        [SerializeField] private GameObject WeaponHolder;
        [SerializeField] private HandlePlaceCanvas _buildCanvas;
        [SerializeField] private TowerInteractUI _InteractCanvas;

        [SerializeField] private CinemachineExtender cinemachineExtender;

        private GameController _gameController;

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

        private float prevposx;
        private float prevposz;
        private bool checkMovement;

        //public bool CameraFreezed { get { return _cameraFreezed; } set { _cameraFreezed = value; } }
        public override void OnGainedOwnership()
        {
            //cam.isActiveAndEnabled = true;
            //cam.enabled = true;
            //cam.GetComponent<Camera>().enabled = true;
            //cam.SetActive(true);
        }
        private void OnEnable()
        {
            //if (!IsOwner) return;
            _buildCanvas.OnBuildingsCanvasToggled += ToggleFreezeCam;
            _InteractCanvas.OnUpgradeCanvasToggled += ToggleFreezeCam;
    
        }

        private void OnDisable()
        {
            //if (!IsOwner) return;
            _buildCanvas.OnBuildingsCanvasToggled -= ToggleFreezeCam;
            _InteractCanvas.OnUpgradeCanvasToggled -= ToggleFreezeCam;
        }

        private void Start()
        {
            _gameController = GameController.Instance;
            _gameController.OnGameOver += ToggleFreezeCam;
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
            
            //PlayerCamera.Instance.FollowPlayer(transform.Find("PlayerCameraRoot"));
            //CinemachineVirtualCamera virtualcam = GameObject.Find("Main Camera").GetComponent<CinemachineVirtualCamera>();
            //if (IsOwner) virtualcam.Follow = CameraRoot;
            Camera = GameObject.Find("Main Camera").transform;
            cam = GameObject.Find("Main Camera");
            //cinemachineExtender.Sensitivity = MouseSensitivity;
            //cinemachineExtender.inputManger = GetComponent<InputManager>();
            //virtualcam.AddExtension(cinemachineExtender);
            if (!IsOwner) return;
            cam.transform.SetParent(transform);
            WeaponHolder.transform.SetParent(cam.transform);
            WeaponHolder.transform.localPosition = Vector3.zero;
            WeaponHolder.transform.localRotation = Quaternion.identity;
            foreach (Transform child in WeaponHolder.transform)
            {
                child.transform.GetChild(0).TryGetComponent<GunSystem>(out var gunSystem);
                if (gunSystem != null)
                {
                gunSystem.fpsCam = cam.GetComponent<Camera>();
                }
            }
            checkMovement = false;
            _playerRigidbody.position = new Vector3(20.5f,0,-30);                        
            checkMovement = true;
            //WeaponHolderSync(); 
            if (!PlayerPrefs.HasKey("volume"))
            {
                PlayerPrefs.SetFloat("volume", 0.1f);
                Load();
            }
            else
            {
                Load();
            }
        }
       
        private void FixedUpdate()
        {           
            SampleGround();
            Move();
            HandleJump();
            HandleCrouch();
            Checkmovement();
            WeaponHolderSync();
        }

        private void LateUpdate()
        {
            
            CamMovements();
            WeaponHolderSync();
        }
        void Checkmovement()
        {
            if (checkMovement) return; 
            //Debug.Log("GG");
            //Debug.Log(Math.Abs((prevposx + prevposz) - (transform.position.x + transform.position.z)));
            if (Math.Abs(prevposx - transform.position.x) > 0.5 || Math.Abs(prevposz - transform.position.z) > 0.5)
            {
                Debug.Log("Teleportation prevented");
                transform.position = new Vector3(prevposx, transform.position.y, prevposz);
            }
            prevposx = transform.position.x;
            prevposz = transform.position.z;
        }
        public event Action CamFreeze;
        public void ToggleFreezeCam()
        {         
            if (!IsOwner) return;
            _cameraFreezed = !_cameraFreezed;
            CamFreeze?.Invoke();

        }
        public void UnStuck()
        {
            checkMovement = false;
            _playerRigidbody.position = new Vector3(15, 0, -25);
            checkMovement = true;
        }
        private void Move()
        {
            if (!IsOwner) return;
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
        private void WeaponHolderSync()
        {
            WeaponHolderSyncServerRPC();
        }
        [ServerRpc(RequireOwnership = false)]
        private void WeaponHolderSyncServerRPC()
        {
            WeaponHolderSyncClientRPC();
        }
        [ClientRpc]
        private void WeaponHolderSyncClientRPC()
        {
            if (IsOwner) return;
            WeaponHolder.transform.position = CameraRoot.position;
            //WeaponHolder.transform.parent = CameraRoot;
        }
        private void CamMovements()
        {
            if (!IsOwner) return;
            if (!_hasAnimator) return;
            if (_cameraFreezed) return;

            var Mouse_X = _inputManager.Look.x;
            var Mouse_Y = _inputManager.Look.y;
            Camera.position = CameraRoot.position;

            _xRotation -= Mouse_Y * MouseSensitivity * Time.deltaTime;
            _xRotation = Mathf.Clamp(_xRotation, UpperLimit, BottomLimit);

            Camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);
            RotateWeaponServerRpc(_xRotation);
            _playerRigidbody.MoveRotation(_playerRigidbody.rotation * Quaternion.Euler(0, Mouse_X * MouseSensitivity * Time.smoothDeltaTime, 0));
        }
        [ServerRpc(RequireOwnership = false)]
        void RotateWeaponServerRpc(float xd)
        {           
            RotateWeaponClientRpc(xd);
        }
        [ClientRpc]
        void RotateWeaponClientRpc(float xd)
        {
            if (IsOwner) return;
            WeaponHolder.transform.localRotation = Quaternion.Euler(xd, 0, 0);
        }
        private void HandleCrouch()
        {
            if (!IsOwner) return;
            
            _animator.SetBool(_crouchVelHash, _inputManager.Crouch);
        }

        private void HandleJump()
        {
            jumpDelay -= Time.deltaTime;
            if (jumpDelay <= 0.0f)
            {
                if (!IsOwner) return;
                if (!_hasAnimator) return;
                if (!_inputManager.Jump) return;
                if (!_grounded) return;

                _animator.SetTrigger(_jumpHash);
                jumpDelay = 1f;
            }
        }
            
        public void JumpAddForce()
        {
            if (!IsOwner) return;
            _playerRigidbody.AddForce(-_playerRigidbody.velocity.y*Vector3.up,ForceMode.VelocityChange);
            _playerRigidbody.AddForce(Vector3.up * JumpFactor, ForceMode.Impulse);
            
            _animator.ResetTrigger(_jumpHash);
        }

        private void SampleGround()
        {
            if (!IsOwner) return;
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
            if (!IsOwner) return;
            
            _animator.SetBool(_fallingHash, !_grounded);
            _animator.SetBool(_groundHash, _grounded);
        }
        void Load()
        {
            AudioListener.volume = PlayerPrefs.GetFloat("volume");
        }
    }
}
