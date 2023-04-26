using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
//using TMPro.EditorUtilities;
using TowerDefense.Data.Core;
using TowerDefense.Data.Enemies;
using TowerDefense.Gameplay.Enemies;
using TowerDefense.Gameplay.Helpers;
using UnityEngine;

namespace TowerDefense.Gameplay.Core
{
    public class GameController : MonoBehaviour
    {
        public void SetupNewGame()
        {
            InitStatistics();
            InitUI();
            OnGameBegin?.Invoke();
        }

        public void StartNewWave()
        {
            UpdateWaveText();
            _waveController.StartNextWave();
        }

        public void IncrementMoney(int amount)
        {
            _currGameStatistics.Money += amount;
            OnMoneyChanged?.Invoke(_currGameStatistics.Money);
        }

        public void DecrementMoney(int amount)
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

        public bool GameOver { get => _gameOver; }
        public bool IsWon { get => _isWon; }
        public int Money { get => _currGameStatistics.Money; }

        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            SetupNewGame();
            Debug.Log("mapos startos");
        }

        private void OnEnable()
        {
            _enemySpawner = EnemySpawner.Instance;
            _enemySpawner.OnEnemySpawned += SubscribeToEnemyEvents;
            _waveController.OnWaveCompleted += WaveCompleted;
        }

        private void OnDisable()
        {
            _enemySpawner.OnEnemySpawned -= SubscribeToEnemyEvents;
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

        //private void EndOfWaveReward(int waveNumber)
        //{

        //}

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
