using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Data.Enemies;
using TowerDefense.Gameplay.Core;
using TowerDefense.Gameplay.Enemies;
using TowerDefense.Gameplay.Path;
using Unity.Netcode;
using UnityEngine;

namespace TowerDefense.Gameplay.Enemies
{
    /// <summary>
    /// Singleton class for spawning an enemy.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        public void SpawnEnemyOfType(EnemyType enemyType)
        {
            var enemyProperties = EnemyPropertiesProcessor.GetEnemyPropertyFromType(enemyType);
            SpawnEnemy(enemyProperties, _pathHeadPosition, 0);
        }

        public void SpawnEnemy(EnemyProperties enemyProperties, Vector3 spawnPosition, int waypointIndex)
        {
            EnemySpawnerMultiplayer.Instance.SpawnEnemy(enemyProperties, spawnPosition, waypointIndex);
        }

        public static EnemySpawner Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType(typeof(EnemySpawner)) as EnemySpawner;
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        private void Start()
        {
            _pathHeadPosition = _pathController[0];
        }

        [SerializeField] private PathController _pathController;

        private static EnemySpawner _instance;

        private Vector3 _pathHeadPosition;
    }
}
