using System;
using System.Collections.Generic;
using TowerDefense.Data.Enemies;
using TowerDefense.Gameplay.Enemies;
using TowerDefense.Gameplay.Helpers;
using UnityEngine;


namespace TowerDefense.Towers.TowerAttackControllers
{
    public class TowerShooting : MonoBehaviour
    {
        private TowerEnums.TargetingStyle _targetingStyle;
        [SerializeField]
        [Tooltip("Logical, only neccessery for the model")]
        private Transform _head;

        [SerializeField]
        [Tooltip("Ensures accurate shooting")]
        private Transform _bulletOrigin;

        [SerializeField] private List<EnemyController> _targetsInRange = new List<EnemyController>();
        [SerializeField] private Queue<EnemyController> _targetsToRemove = new Queue<EnemyController>();
        [SerializeField] private LayerMask _hitLayer;
        [SerializeField] private LayerMask _enemyLayer;
        private Vector3 _origin;
        [SerializeField] private Transform _target = null;
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
                _origin = _towerController.BulletOrigin.transform.position;
            }

        }

        public void AddTargetToInRange(EnemyController target)
        {
            _targetsInRange.Add(target);
            target.OnEnemyDie += HandleOnEnemyKilled;
            //EnsureSight();
            GetTarget();
        }

        public void RemoveTargetFromInRange(EnemyController target)
        {
            _targetsInRange.Remove(target);
            //if (_targetsInSight.Contains(target))
            //{
            //    _targetsInSight.Remove(target);
            //}

            target.OnEnemyDie -= HandleOnEnemyKilled;

            //EnsureSight();
            GetTarget();
        }

        private bool EnsureSight(EnemyController target)
        {
            if (target == null)
            {
                return false;
            }
            var direction = target.transform.position - _origin;
            Debug.DrawRay(_origin, direction);
            Ray ray = new Ray(_origin, direction);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _hitLayer))
            {
                if (((_enemyLayer.value & (1 << hit.collider.gameObject.layer)) > 0))
                {
                    return true;
                }
                else if ((_enemyLayer.value & (1 << hit.collider.gameObject.layer)) <= 0)
                {
                    return false;
                }
            }

            return false;


        }

        private void GetTarget()
        {
            List<EnemyController> targetsInSight = new List<EnemyController>();

            foreach (EnemyController target in _targetsInRange)
            {
                if (EnsureSight(target))
                {
                    targetsInSight.Add(target);
                }
            }

            if (targetsInSight.Count > 0)
            {
                switch (_targetingStyle)
                {
                    case TowerEnums.TargetingStyle.First:
                        _target = GetEnemyPosition.First(targetsInSight);
                        break;
                    case TowerEnums.TargetingStyle.Last:
                        _target = GetEnemyPosition.Last(targetsInSight);
                        break;
                    case TowerEnums.TargetingStyle.Strongest:
                        _target = GetEnemyPosition.Strongest(targetsInSight);
                        break;
                    case TowerEnums.TargetingStyle.Weakest:
                        _target = GetEnemyPosition.Weakest(targetsInSight);
                        break;
                    case TowerEnums.TargetingStyle.Closest:
                        _target = GetEnemyPosition.Closest(targetsInSight, transform.position);
                        break;
                    default:
                        _target = GetEnemyPosition.First(targetsInSight);
                        break;
                }

                //if (_target != null)
                //{

                //        _target.GetComponent<EnemyController>().OnEnemyKilled += HandleOnEnemyKilled;
                //}
            }
        }

        private void HandleOnEnemyKilled(EnemyController enemy)
        {
            //EnemyController enemyScript = _target.GetComponent<EnemyController>();


            _targetsToRemove.Enqueue(enemy);
            //RemoveTargetFromInRange(enemy);
            //_target = null;


            //_targetsInRange.Remove(enemyScript);
            //Debug.Log("enemyRemoved");

            //if (_targetsInSight.Contains(enemyScript))
            //{
            //    _targetsInSight.Remove(enemyScript);
            //    Debug.Log("enemyRemoved from sight");
            //}
            //_target = null;
            ////enemyScript.OnEnemyKilled -= HandleOnEnemyKilled;
            //GetTarget();

        }

        private void Update()
        {
            if(_targetsToRemove.Count>0)
            {
                RemoveDeadEnemies();
            }

            if (_target != null && EnsureSight(_target.GetComponent<EnemyController>()))
            {
                _canShoot = true;
                TargetEnemy();   
            }
            else
            {
                _canShoot = false;
                GetTarget();

            }
            _towerController.Shoot(_canShoot);
        }

        private void RemoveDeadEnemies()
        {
            foreach (EnemyController target in _targetsToRemove)
            {
                _targetsInRange.Remove(target);
                _target = null;
            }

            _targetsToRemove.Clear();

        }

        private void TargetEnemy()
        {
            _head.LookAt(_target);
            _bulletOrigin.LookAt(_target);
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
