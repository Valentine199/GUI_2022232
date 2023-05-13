using System;
using System.Collections;
using System.Collections.Generic;
//using TMPro.EditorUtilities;
using TowerDefense.Data.Core;
using TowerDefense.Data.Enemies;
using TowerDefense.Gameplay.Enemies;
using Unity.Netcode;
using UnityEngine;

namespace TowerDefense.Gameplay.Core
{
    public class WaveController : NetworkBehaviour
    {
        public void Initialize(GameStatistics gameStatistics)
        {
            _currGameStatistics = gameStatistics;
            for (int i = 0; i < _waves.Count; i++)
                _waves[i].WaveNumber = i + 1;
        }

        public void StartNextWave()
        {
            StartNextWaveServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        public void StartNextWaveServerRpc()
        {
            StartCoroutine(SpawnEnemiesInWave(_currWave));
        }

        public event Action<WaveProperties> OnWaveCompleted;
        public event Action<WaveProperties> OnPrepareNextRound;

        private void OnEnable()
        {
            _enemySpawner = EnemySpawner.Instance;
            EnemySpawnerMultiplayer.Instance.OnEnemySpawned += SubscribeToEnemyEvents;
            _gameController.OnGameBegin += PrepareNextRound;
        }

        private void OnDisable()
        {
            EnemySpawnerMultiplayer.Instance.OnEnemySpawned -= SubscribeToEnemyEvents;
            _gameController.OnGameBegin -= PrepareNextRound;
        }

        private IEnumerator SpawnEnemiesInWave(WaveProperties waveProperties)
        {
            foreach (var sp in waveProperties.SpawnPacks)
            {
                yield return new WaitForSeconds(sp.InitialSpawnDelay);
                StartCoroutine(SpawnEnemiesInPack(sp));
            }
        }

        private IEnumerator SpawnEnemiesInPack(SpawnPack spawnPack)
        {
            for (int i = 0; i < spawnPack.AmountInGroup; i++)
            {
                _enemySpawner.SpawnEnemyOfType(spawnPack.EnemyType);
                yield return new WaitForSeconds(spawnPack.TimeBetweenEnemies);
            }
        }

        private void SubscribeToEnemyEvents(EnemyController enemyController)
        {
            enemyController.OnEnemyKilled += EnemyKilled;
            enemyController.OnEnemyReachedEnd += EnemyReachedEnd;
        }

        private void EnemyKilled(EnemyProperties enemyProperties)
        {
            DecrementEnemiesLeft(1);
        }

        private void EnemyReachedEnd(EnemyProperties enemyProperties)
        {
            DecrementEnemiesLeft(enemyProperties.TotalEnemyCount);
        }

        private void DecrementEnemiesLeft(int amount)
        {
            if (_gameController.GameOver)
                return;

            _enemiesLeft -= amount;
            if (_enemiesLeft <= 0)
                PrepareNextRound();
        }

        private void PrepareNextRound()
        {
            ++_currGameStatistics.Waves;
            if (_currGameStatistics.Waves > _waves.Count)
            {
                _gameController.DoVictory();
                return;
            }
            if (_currGameStatistics.Waves != 1)
            {
                OnWaveCompleted?.Invoke(_currWave);
            }

            _currWave = _waves[CurrWaveIndex];
            _enemiesLeft = _currWave.TotalEnemyCount;

            OnPrepareNextRound?.Invoke(_currWave);
        }

        [SerializeField] private List<WaveProperties> _waves;
        [SerializeField] private GameController _gameController;

        private int _enemiesLeft;
        private WaveProperties _currWave;
        private GameStatistics _currGameStatistics;
        private EnemySpawner _enemySpawner;

        private int CurrWaveIndex
        {
            get => _currGameStatistics.Waves - 1;
        }
    }
}
