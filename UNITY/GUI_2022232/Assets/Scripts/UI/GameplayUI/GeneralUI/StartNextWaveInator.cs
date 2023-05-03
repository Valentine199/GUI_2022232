using System.Collections;
using System.Collections.Generic;
using TowerDefense.Gameplay.Core;
using UnityEngine;

public class StartNextWaveInator : MonoBehaviour
{
    private GameController _gameController;

    private void Start()
    {
        _gameController = GameController.Instance;
    }

    public void StartNextWave()
    {
        _gameController.StartNewWave();
    }
}
