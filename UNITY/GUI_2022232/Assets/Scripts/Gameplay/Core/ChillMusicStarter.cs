using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Gameplay.Core;
using UnityEngine;

public class ChillMusicStarter : MonoBehaviour, ISoundPlayer, IMusicStarter
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

    private void Start()
    {
        StartMusic();
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
