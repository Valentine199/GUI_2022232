using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Data.Enemies;
using TowerDefense.Gameplay.Core;
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
        public void SpawnEnemyOfType(EnemyType enemyType)
        {
            var enemyProperties = EnemyPropertiesProcessor.GetEnemyPropertyFromType(enemyType);
            SpawnEnemy(enemyProperties, _pathHeadPosition, 0);
        }

        public EnemyController SpawnEnemy(EnemyProperties enemyProperties, Vector3 spawnPosition, int waypointIndex)
        {
            var newEnemy = Instantiate(_enemyPrefabs[(int)enemyProperties.EnemyType], spawnPosition, Quaternion.identity);
            var newEnemyController = newEnemy.GetComponent<EnemyController>();

            newEnemyController.PathController = _pathController;
            newEnemyController.InitEnemy(enemyProperties, waypointIndex);
            OnEnemySpawned?.Invoke(newEnemyController);

            if (_spawnedEnemiesGO == null)
                _spawnedEnemiesGO = new GameObject("Enemies");
            newEnemy.transform.parent = _spawnedEnemiesGO.transform;

            return newEnemyController;
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

        public event Action<EnemyController> OnEnemySpawned;

        private void Start()
        {
            _pathHeadPosition = _pathController[0];
        }

        [SerializeField] private List<GameObject> _enemyPrefabs;
        [SerializeField] private PathController _pathController;

        private static EnemySpawner _instance;

        private GameObject _spawnedEnemiesGO;

        private Vector3 _pathHeadPosition;
    }
}
