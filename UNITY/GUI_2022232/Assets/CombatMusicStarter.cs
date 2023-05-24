using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Gameplay.Core;
using UnityEngine;

public class CombatMusicStarter : MonoBehaviour, ISoundPlayer, IMusicStarter
{
    public event Action PlayInitSound;
    public event Action PlayAmbiance;
    public event Action StopAmbiance;
    public event Action PlayEndSound;

    private void OnEnable()
    {
        WaveController controller = WaveController.Instance;
        controller.OnWaveStarted += StartMusic;
        controller.OnWaveCompleted += StopMusic;
    }

    private void OnDisable()
    {
        WaveController controller = WaveController.Instance;
        controller.OnWaveStarted -= StartMusic;
        controller.OnWaveCompleted -= StopMusic;
    }

    public void StartMusic()
    {
        PlayAmbiance?.Invoke();
    }

    public void StopMusic()
    {
        StopAmbiance?.Invoke();
    }
}
