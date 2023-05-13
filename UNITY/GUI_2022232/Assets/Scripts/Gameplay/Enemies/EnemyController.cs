using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Data.Enemies;
using TowerDefense.Gameplay.Path;
using TowerDefense.Towers.TowerAttackControllers;
using Unity.VisualScripting;
using UnityEngine;

namespace TowerDefense.Gameplay.Enemies
{
    public class EnemyController : MonoBehaviour
    {
        public void InitEnemy(EnemyProperties enemyProperties, int targetWaypointIndex)
        {
            _enemyProperties = enemyProperties;
            _healthRemaining = _enemyProperties.Health;
            _targetWaypointIndex = targetWaypointIndex;
            SetTargetWaypointPosition();
        }

        public bool HitEnemy()
        {
            --_healthRemaining;
            if (_healthRemaining < 0)
                return false;
            if (_healthRemaining <= 0)
                BurstEnemy();

            return true;
        }

        public bool HitEnemy(out EnemyController[] spawnedEnemies)
        {
            spawnedEnemies = null;
            --_healthRemaining;
            if (_healthRemaining < 0)
                return false;
            spawnedEnemies = _healthRemaining <= 0 ? BurstEnemy() : new EnemyController[1] { this };
            return true;
        }

        public static EnemyController CompareFirst(EnemyController e1, EnemyController e2)
        {
            if (e1._targetWaypointIndex > e2._targetWaypointIndex)
                return e1;
            if (e1._targetWaypointIndex < e2._targetWaypointIndex)
                return e2;
            if (e1.PercentToNextWaypoint > e2.PercentToNextWaypoint)
                return e1;
            if (e1.PercentToNextWaypoint < e2.PercentToNextWaypoint)
                return e2;

            return e1;
        }

        public static EnemyController CompareLast(EnemyController e1, EnemyController e2)
        {
            if (e1._targetWaypointIndex > e2._targetWaypointIndex)
                return e1;
            if (e1._targetWaypointIndex < e2._targetWaypointIndex)
                return e2;
            if (e1.PercentToNextWaypoint > e2.PercentToNextWaypoint)
                return e1;
            if (e1.PercentToNextWaypoint < e2.PercentToNextWaypoint)
                return e2;

            return e1;
        }

        public static EnemyController CompareStrongest(EnemyController e1, EnemyController e2)
        {
            if (e1.LTW > e2.LTW)
                return e1;
            if (e1.LTW < e2.LTW) 
                return e2;

            // Same strength -> target first
            return CompareFirst(e1, e2);
        }

        public static EnemyController CompareWeakest(EnemyController e1, EnemyController e2)
        {
            if (e1.LTW < e2.LTW)
                return e1;
            if (e1.LTW > e2.LTW)
                return e2;

            // Same strength -> target first
            return CompareFirst(e1, e2);
        }

        public event Action<EnemyProperties> OnEnemyReachedEnd;
        public event Action<EnemyProperties> OnEnemyKilled;
        public event Action<EnemyController> OnEnemyDie;

        public EnemyProperties EnemyProperties
        {
            get => _enemyProperties;
            set => _enemyProperties = value;
        }

        public PathController PathController
        {
            get => _path;
            set => _path = value;
        }

        public bool IsFrozen => _isFrozen;

        private void Update()
        {
            MoveEnemies();
        }

        private void BurstEnemy()
        {
            OnEnemyKilled?.Invoke(_enemyProperties);
            OnEnemyDie?.Invoke(this);

            Destroy(gameObject);
            if (HasEnemiesToSpawn)
                SpawnChildEnemies();
        }

        private void SpawnChildEnemies()
        {
            for (int i = 0; i < _enemyProperties.EnemiesToSpawnWhenKilled.Count; i++)
            {
                EnemyProperties enemyToSpawn = _enemyProperties.EnemiesToSpawnWhenKilled[i];
                Vector3 spawnOffset = new Vector3(
                    (float)Util.Random.NextDouble(),
                    (float)Util.Random.NextDouble(),
                    (float)Util.Random.NextDouble());
                EnemySpawner.Instance.SpawnEnemy(enemyToSpawn, transform.position + spawnOffset, _targetWaypointIndex);
            }
        }

        private void MoveEnemies()
        {
            float speed = _enemyProperties.MoveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetWaypointPosition, speed);

            if (PercentToNextWaypoint <= PERCENT_THRESHOLD || (100.0f - PercentToNextWaypoint) <= PERCENT_THRESHOLD)
                RotateSmooth();
            if (Vector3.Distance(transform.position, _targetWaypointPosition) <= DIST_THRESHOLD)
                SetNextTargetPosition();
        }

        /// <summary>
        /// Left in for testing purposes.
        /// </summary>
        private void RotateInstant()
        {
            var relativePos = _targetWaypointPosition - transform.position;
            transform.rotation = Quaternion.LookRotation(relativePos);
        }

        private void RotateSmooth()
        {
            if (_targetWaypointPosition - transform.position != Vector3.zero)
            {
                var targetRot = Quaternion.LookRotation(_targetWaypointPosition - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _enemyProperties.MoveSpeed * Time.deltaTime);
            }
        }

        private void SetNextTargetPosition()
        {
            ++_targetWaypointIndex;
            if (EnemyReachedEnd)
            {
                OnEnemyReachedEnd?.Invoke(_enemyProperties);
                Destroy(gameObject);
                return;
            }
            SetTargetWaypointPosition();
        }

        private void SetTargetWaypointPosition()
        {
            _targetWaypointPosition = _path[_targetWaypointIndex];
        }

        private const float DIST_THRESHOLD = 0.1f;
        private const float PERCENT_THRESHOLD = 5.0f;

        private EnemyProperties _enemyProperties;
        private PathController _path;

        private int _targetWaypointIndex;
        private int _healthRemaining;
        private bool _isFrozen;

        private Vector3 _targetWaypointPosition;

        private int LTW
        {
            get => _enemyProperties.WorthInLowestTier;
        }

        private Vector3 PreviousWaypointPosition
        {
            get => _targetWaypointIndex == 0.0f
                ? Vector3.zero
                : _path[_targetWaypointIndex - 1];
        }

        private float PathSegmentLength
        {
            get => _targetWaypointIndex == 0.0f
                ? 0.0f
                : Vector3.Distance(PreviousWaypointPosition, _targetWaypointPosition);
        }

        private float DistToNextWaypoint
        {
            get => Vector3.Distance(transform.position, _targetWaypointPosition);
        }

        private float PercentToNextWaypoint
        {
            get => _targetWaypointIndex == 0.0f
                ? 0.0f
                : (PathSegmentLength - DistToNextWaypoint) / PathSegmentLength;
        }

        private bool EnemyReachedEnd
        {
            get => _targetWaypointIndex >= _path.WaypointCount;
        }

        private bool HasEnemiesToSpawn
        {
            get => _enemyProperties.EnemiesToSpawnWhenKilled != null ||
                _enemyProperties.EnemiesToSpawnWhenKilled.Count > 0;
        }
    }
}