using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TowerDefense.Gameplay.Core;
using System.Runtime.CompilerServices;
using UnityEngine.InputSystem;

public class CinemachineExtender : CinemachineExtension
{

    [SerializeField] private float clampAngle = 80f;
    public float Sensitivity = 10f;

    [SerializeField] public InputManager inputManger;
    private Vector3 startingRotation;

    protected override void Awake()
    {
        startingRotation = transform.localRotation.eulerAngles;
        base.Awake();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                Vector2 deltaInput = inputManger.GetMouseDelta();
                startingRotation.x += deltaInput.x * Sensitivity * Time.smoothDeltaTime;
                startingRotation.y += -deltaInput.y * Sensitivity * Time.smoothDeltaTime;
                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);

                state.RawOrientation = Quaternion.Euler(startingRotation.y, startingRotation.x, 0f);
            }
        }
    }
}
