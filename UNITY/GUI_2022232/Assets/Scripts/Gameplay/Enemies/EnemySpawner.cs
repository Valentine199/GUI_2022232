using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Data.Enemies;
using TowerDefense.Gameplay.Enemies;
using TowerDefense.Gameplay.Path;
using UnityEngine;

namespace TowerDefense.Gameplay.Enemies
{
    /// <summary>
    /// Singleton class for spawning an enemy.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        private EnemySpawner() { }

        public static EnemySpawner GetInstance()
        {
            return _instance;
        }

        public void SpawnEnemyOfType(EnemyType enemyType)
        {
            var enemyProperties = EnemyPropertiesProcessor.GetEnemyPropertyFromType(enemyType);
            SpawnEnemy(enemyProperties, _pathHeadPosition, 0);
        }

        public EnemyController SpawnEnemy(EnemyProperties enemyProperties, Vector3 spawnPosition, int waypointIndex)
        {
            var newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            var newEnemyController = newEnemy.GetComponent<EnemyController>();

            newEnemyController.PathController = _pathController;
            newEnemyController.InitEnemy(enemyProperties, waypointIndex);
            OnEnemySpawned?.Invoke(newEnemyController);

            return newEnemyController;
        }

        public event Action<EnemyController> OnEnemySpawned;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(this);
        }

        private void Start()
        {
            _pathHeadPosition = _pathController[0];
        }

        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private PathController _pathController;

        private Vector3 _pathHeadPosition;

        private static EnemySpawner _instance;
    }
}
