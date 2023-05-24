using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance { get; private set; }

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    
    private void Awake()
    {
        
        Instance = this;
        
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        Debug.Log("Cam found");
    }

    public void FollowPlayer(Transform transform)
    {
        cinemachineVirtualCamera.Follow = transform;
    }
}
