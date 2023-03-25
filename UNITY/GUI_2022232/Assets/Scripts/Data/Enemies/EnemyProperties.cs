using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerDefense.Data.Enemies
{
    [CreateAssetMenu(fileName = "EnemyProperties", menuName = "Scriptable Objects/Enemy Properties", order = 0)]
    public class EnemyProperties : ScriptableObject
    { 
        public int Health
        {
            get => _health;
            set => _health = value;
        }

        public int MoneyWhenKilled
        {
            get => _moneyWhenKilled;
            set => _moneyWhenKilled = value;
        }

        public float MoveSpeed
        {
            get => _moveSpeed;
            set => _moveSpeed = value;
        }

        public List<EnemyProperties> EnemiesToSpawnWhenKilled
        {
            get => _enemiesToSpawnWhenKilled;
            set => _enemiesToSpawnWhenKilled = value;
        }

        public EnemyType EnemyType
        {
            get => _enemyType;
            set => _enemyType = value;
        }

        // Uses recursion to find how much this enemy is worth in lowest tier enemies
        public int WorthInLowestTier
        {
            get
            {
                int equiv = _health;
                if (_enemiesToSpawnWhenKilled != null || _enemiesToSpawnWhenKilled.Count <= 0)
                    return equiv;

                return _enemiesToSpawnWhenKilled.Sum(e => e.WorthInLowestTier);
            }
        }

        // Uses recursion to find total number of enemies when all tiers are killed
        public int TotalEnemyCount
        {
            get
            {
                int total = 1;
                if (_enemiesToSpawnWhenKilled != null || _enemiesToSpawnWhenKilled.Count <= 0)
                    return total;

                return _enemiesToSpawnWhenKilled.Sum(e => e.TotalEnemyCount);
            }
        }

        [SerializeField] private int _health;
        [SerializeField] private int _moneyWhenKilled;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private List<EnemyProperties> _enemiesToSpawnWhenKilled;
        [SerializeField] private EnemyType _enemyType;
    }
}