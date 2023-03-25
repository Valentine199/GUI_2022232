using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Data.Enemies;
using UnityEngine;

namespace TowerDefense.Data.Core
{
    [Serializable]
    public class SpawnPack
    {
        public EnemyType EnemyType
        {
            get => _enemyType;
            set => _enemyType = value;
        }

        public int AmountInGroup
        {
            get => _amountInGroup;
            set => _amountInGroup = value;
        }

        public int WorthInLowestTier
        {
            get
            {
                var enemyProperties = EnemyPropertiesProcessor.GetEnemyPropertyFromType(_enemyType);
                var worth = enemyProperties.WorthInLowestTier * _amountInGroup;
                return worth;
            }
        }

        public int TotalEnemyCount
        {
            get
            {
                var enemyProperties = EnemyPropertiesProcessor.GetEnemyPropertyFromType(_enemyType);
                var total = enemyProperties.TotalEnemyCount * _amountInGroup;
                return total;
            }
        }

        public float InitialSpawnDelay
        {
            get => _initialSpawnDelay;
            set => _initialSpawnDelay = value;
        }

        public float TimeBetweenEnemies
        {
            get => _timeBetweenEnemies;
            set => _timeBetweenEnemies = value;
        }

        [SerializeField] private EnemyType _enemyType;
        [SerializeField] private int _amountInGroup;
        [SerializeField] private float _timeBetweenEnemies;

        [Tooltip("Delay between the first enemy spawn of previous group and this one.")]
        [SerializeField] private float _initialSpawnDelay;
    }
}