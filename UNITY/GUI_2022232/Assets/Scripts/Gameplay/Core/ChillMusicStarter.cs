using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Gameplay.Core;
using Unity.Netcode;
using UnityEngine;

public class ChillMusicStarter : NetworkBehaviour, ISoundPlayer, IMusicStarter
{
    public event Action PlayInitSound;
    public event Action PlayAmbiance;
    public event Action StopAmbiance;
    public event Action PlayEndSound;

    //private void OnEnable()
    //{
    //    WaveController controller = WaveController.Instance;
    //    //controller.OnWaveStarted += StopMusic;
    //    //controller.OnWaveCompleted += StartMusic;
    //}

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        StartMusic();
    }

    private void Start()
    {
        
    }

    //private void OnDisable()
    //{
    //    WaveController controller = WaveController.Instance;
    //    controller.OnWaveStarted -= StopMusic;
    //    controller.OnWaveCompleted -= StartMusic;
    //}

    public void StartMusic()
    {
        PlayAmbiance?.Invoke();
    }

    public void StopMusic()
    {
        StopAmbiance?.Invoke();
    }
}
