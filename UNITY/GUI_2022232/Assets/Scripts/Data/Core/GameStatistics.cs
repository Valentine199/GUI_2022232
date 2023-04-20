using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Data.Core
{
    [CreateAssetMenu(fileName = "GameStatistics", menuName = "Scriptable Objects/Game Statistics", order = 0)]
    public class GameStatistics : ScriptableObject
    {
        public void SetGameStatistics(GameStatistics stats)
        {
            Waves = stats.Waves;
            Money = stats.Money;
            Lives = stats.Lives;
            EnemiesKilled = stats.EnemiesKilled;
            SellTowerMultiplier = stats.SellTowerMultiplier;
        }

        private void Awake()
        {
            _hasMaxStats = _maxGameStats != null;
        }

        public int Waves
        { 
            get => _waves; 
            set
            {
                _waves = _hasMaxStats && !_maxGameStats.Waves.Equals(0)
                    ? Mathf.Clamp(value, 0, _maxGameStats.Waves)
                    : Mathf.Max(value, 0);
            }
        }

        public int Money
        { 
            get => _money; 
            set
            {
                _money = _hasMaxStats && !_maxGameStats.Money.Equals(0)
                    ? Mathf.Clamp(value, 0, _maxGameStats.Money)
                    : Mathf.Max(value, 0);
            }
        }

        public int Lives
        { 
            get => _lives; 
            set
            {
                _lives = _hasMaxStats && !_maxGameStats.Lives.Equals(0)
                    ? Mathf.Clamp(value, 0, _maxGameStats.Lives)
                    : Mathf.Max(value, 0);
            }
        }

        public GameStatistics MaxGameStats
        { 
            get => _maxGameStats; 
            set => _maxGameStats = value; 
        }

        public int EnemiesKilled
        { 
            get => _enemiesKilled; 
            set => _enemiesKilled = value; 
        }

        public bool HasMaxStats
        { 
            get => _hasMaxStats; 
            set => _hasMaxStats = value; 
        }

        public float SellTowerMultiplier
        {
            get => _sellTowerMultiplier;
            set => _sellTowerMultiplier = value;
        }

        [SerializeField] private int _waves;
        [SerializeField] private int _money;
        [SerializeField] private int _lives;
        [SerializeField] private float _sellTowerMultiplier;
        [SerializeField] private GameStatistics _maxGameStats;

        private int _enemiesKilled;
        private bool _hasMaxStats;
    }
}
