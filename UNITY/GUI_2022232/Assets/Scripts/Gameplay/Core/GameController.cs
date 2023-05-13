using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
//using TMPro.EditorUtilities;
using TowerDefense.Data.Core;
using TowerDefense.Data.Enemies;
using TowerDefense.Gameplay.Enemies;
using TowerDefense.Gameplay.Helpers;
using Unity.Netcode;
using UnityEngine;

namespace TowerDefense.Gameplay.Core
{
    public class GameController : NetworkBehaviour
    {
        public void SetupNewGame()
        {
            InitStatistics();
            InitUI();
            OnGameBegin?.Invoke();
        }

        public void StartNewWave()
        {
            StartNewWaveServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        public void StartNewWaveServerRpc()
        {
            StartNewWaveClientRpc();
            _waveController.StartNextWave();
        }

        [ClientRpc]
        public void StartNewWaveClientRpc()
        {
            UpdateWaveText();
        }

        public void IncrementMoney(int amount)
        {
            IncrementMoneyServerRpc(amount);
        }

        [ServerRpc]
        public void IncrementMoneyServerRpc(int amount)
        {
            IncrementMoneyClientRpc(amount);
        }

        [ClientRpc]
        public void IncrementMoneyClientRpc(int amount)
        {
            _currGameStatistics.Money += amount;
            OnMoneyChanged?.Invoke(_currGameStatistics.Money);
        }

        public void DecrementMoney(int amount)
        {
            DecrementMoneyServerRpc(amount);
        }

        [ServerRpc]
        public void DecrementMoneyServerRpc(int amount)
        {
            DecrementMoneyClientRpc(amount);
        }

        [ClientRpc]
        public void DecrementMoneyClientRpc(int amount)
        {
            _currGameStatistics.Money -= amount;
            OnMoneyChanged?.Invoke(_currGameStatistics.Money);
        }

        public event Action<int> OnWaveChanged;
        public event Action<int> OnMoneyChanged;
        public event Action<int> OnLivesChanged;

        public event Action OnGameBegin;
        public event Action OnGameOver;

        public static GameController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType(typeof(GameController)) as GameController;
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        public bool GameOver => _gameOver;
        public bool IsWon => _isWon;
        public int Money => _currGameStatistics.Money;
        public float SellTowerMultiplier => _currGameStatistics.SellTowerMultiplier;

        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            SetupNewGame();
        }

        private void OnEnable()
        {
            EnemySpawnerMultiplayer.Instance.OnEnemySpawned += SubscribeToEnemyEvents;
            _waveController.OnWaveCompleted += WaveCompleted;
        }

        private void OnDisable()
        {
            EnemySpawnerMultiplayer.Instance.OnEnemySpawned -= SubscribeToEnemyEvents;
            _waveController.OnWaveCompleted -= WaveCompleted;
        }

        private void SubscribeToEnemyEvents(EnemyController enemyController)
        {
            enemyController.OnEnemyReachedEnd += EnemyReachedEnd;
            enemyController.OnEnemyKilled += EnemyKilled;
        }

        private void InitStatistics()
        {
            _currGameStatistics.SetGameStatistics(_startGameStatistics);
            _waveController.Initialize(_currGameStatistics);
        }

        public void InitUI()
        {
            OnWaveChanged?.Invoke(_currGameStatistics.Waves);
            OnMoneyChanged?.Invoke(_currGameStatistics.Money);
            OnLivesChanged?.Invoke(_currGameStatistics.Lives);
        }

        private void WaveCompleted(WaveProperties waveProperties)
        {
            // Give money to player if needed
        }

        private void UpdateWaveText()
        {
            OnWaveChanged?.Invoke(_currGameStatistics.Waves);
        }

        private void DecrementLives(int amount)
        {
            DecrementLivesServerRpc(amount);
        }

        [ServerRpc]
        private void DecrementLivesServerRpc(int amount)
        {
            DecrementLivesClientRpc(amount);
        }

        [ClientRpc]
        private void DecrementLivesClientRpc(int amount)
        {
            if (_gameOver)
                return;

            _currGameStatistics.Lives -= amount;
            OnLivesChanged?.Invoke(_currGameStatistics.Lives);
            if (_currGameStatistics.Lives <= 0)
                DoGameOver();
        }

        private void EnemyReachedEnd(EnemyProperties enemyProperties)
        {
            DecrementLives(enemyProperties.TotalEnemyCount);
        }

        private void EnemyKilled(EnemyProperties enemyProperties)
        {
            IncrementMoney(enemyProperties.MoneyWhenKilled);
            ++_currGameStatistics.EnemiesKilled;
        }

        private void DoGameOver()
        {
            _gameOver = true;
            OnGameOver?.Invoke();
        }

        public void DoVictory()
        {
            _isWon = true;
            DoGameOver();
        }

        [SerializeField] private GameStatistics _startGameStatistics;
        [SerializeField] private GameStatistics _currGameStatistics;
        [SerializeField] private WaveController _waveController;
        [SerializeField] private EnemySpawner _enemySpawner;

        private static GameController _instance;

        private bool _gameOver;
        private bool _isWon;
    }
}
