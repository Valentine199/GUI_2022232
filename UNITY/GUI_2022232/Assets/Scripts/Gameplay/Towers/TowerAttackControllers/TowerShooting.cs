using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Data.Enemies;
using TowerDefense.Gameplay.Enemies;
using TowerDefense.Gameplay.Helpers;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


namespace TowerDefense.Towers.TowerAttackControllers
{
    public class TowerShooting : MonoBehaviour
    {
        private TowerEnums.TargetingStyle _targetingStyle;
        [SerializeField] private Transform _head;
        private List<EnemyController> _targetsInRange = new List<EnemyController>();
        private List<EnemyController> _targetsInSight = new List<EnemyController>();
        [SerializeField] private LayerMask _hitLayer;
        [SerializeField] private LayerMask _enemyLayer;
        private Vector3 _origin;
        private Transform _target = null;
        private bool _canShoot = false;


        private TowerController _towerController;
        public TowerController TowerController
        {
            get
            {
                return _towerController;
            }
            set
            {
                _towerController = value;
            }
        }

        public void ChangeTargetingStyle(TowerEnums.TargetingStyle attack)
        {
            _targetingStyle = attack;
            GetTarget();
        }

        private void Start()
        {
            if (_towerController != null)
            {
                _origin =  _towerController.BulletOrigin.transform.position;
            }

        }

        public void AddTargetToInRange(EnemyController target)
        {
            _targetsInRange.Add(target);
            EnsureSight();
        }

        public void RemoveTargetFromInRange(EnemyController target)
        {
            _targetsInRange.Remove(target);
            if (_targetsInSight.Contains(target))
            {
                _targetsInSight.Remove(target);
            }
            
            _target = null;

            EnsureSight();
        }

        private void EnsureSight()
        {
            if (_targetsInRange.Count > 0)
            {
                foreach (EnemyController target in _targetsInRange)
                {

                    var direction =  target.transform.position - _origin;
                    Debug.DrawRay(_origin, direction);
                    Ray ray = new Ray(_origin, direction);

                    if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _hitLayer))
                    {                                               
                        if (((_enemyLayer.value & (1 << hit.collider.gameObject.layer)) > 0) && (!_targetsInSight.Contains(target)))
                        {
                            _targetsInSight.Add(target);
                        }
                        else if ((_enemyLayer.value & (1 << hit.collider.gameObject.layer)) <= 0 && _targetsInSight.Contains(target))
                        {
                            _targetsInSight.Remove(target);
                        }
                    }
                }

                GetTarget();
            }
            else
            {
                _target = null;
            }
        }

        private void GetTarget()
        {
            if (_targetsInSight.Count > 0)
            {
                switch (_targetingStyle)
                {
                    case TowerEnums.TargetingStyle.First:
                        _target = GetEnemyPosition.First(_targetsInSight);
                        break;
                    case TowerEnums.TargetingStyle.Last:
                        _target = GetEnemyPosition.Last(_targetsInSight);
                        break;
                    case TowerEnums.TargetingStyle.Strongest:
                        _target = GetEnemyPosition.Strongest(_targetsInSight);
                        break;
                    case TowerEnums.TargetingStyle.Weakest:
                        _target = GetEnemyPosition.Weakest(_targetsInSight);
                        break;
                    case TowerEnums.TargetingStyle.Closest:
                        _target = GetEnemyPosition.Closest(_targetsInSight, transform.position);
                        break;
                    default:
                        _target = GetEnemyPosition.First(_targetsInSight);
                        break;
                }

                if (_target != null)
                {
                    _target.GetComponent<EnemyController>().OnEnemyKilled += HandleOnEnemyKilled;
                }
            }
        }

        private void HandleOnEnemyKilled(EnemyProperties enemy)
        {
            EnemyController enemyScript = _target.GetComponent<EnemyController>();
            enemyScript.OnEnemyKilled -= HandleOnEnemyKilled;
            _targetsInRange.Remove(enemyScript);
            if (_targetsInSight.Contains(enemyScript))
            {
                _targetsInRange.Remove(enemyScript);
            }
            _target = null;
            GetTarget();

        }

        private void Update()
        {
            if (_target != null)
            {
                _canShoot = true;
            }
            else
            {
                _canShoot = false;
            }
            TargetEnemy();
            _towerController.Shoot(_canShoot);

            EnsureSight();

        }


        private void TargetEnemy()
        {
            _head.LookAt(_target);
        }



        ////public void SetTarget(Enemy enemy)
        ////{
        ////    _target = enemy.transform;
        ////}
        //public void SetTargetToNull()
        //{
        //    _target = null;
        //}




    }
}
