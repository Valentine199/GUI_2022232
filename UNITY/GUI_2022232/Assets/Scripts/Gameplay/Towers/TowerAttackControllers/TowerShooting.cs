using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Gameplay.Enemies;
using TowerDefense.Gameplay.Helpers;
using UnityEngine;


namespace TowerDefense.Towers.TowerAttackControllers
{
    public class TowerShooting : MonoBehaviour
    {
        [SerializeField] private TowerEnums.TargetingStyle TargetingStyle;
        [SerializeField] private Transform head;
        [SerializeField] private List<EnemyController> targetsInRange = new List<EnemyController>();
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
            TargetingStyle = attack;
            GetTarget();
        }

        private void Start()
        {
            if (_towerController != null)
            {
                TargetingStyle = _towerController.TargetingStyle;
            }
        }

        public void AddTargetToInRange(EnemyController target)
        {
            targetsInRange.Add(target);
            GetTarget();
        }

        public void RemoveTargetFromInRange(EnemyController target)
        {
            targetsInRange.Remove(target);
            GetTarget();
        }

        private void GetTarget()
        {
            switch (TargetingStyle)
            {
                case TowerEnums.TargetingStyle.First:
                    _target = GetEnemyPosition.First(targetsInRange);
                    break;
                case TowerEnums.TargetingStyle.Last:
                    _target = GetEnemyPosition.Last(targetsInRange);
                    break;
                case TowerEnums.TargetingStyle.Strongest:
                    _target = GetEnemyPosition.Strongest(targetsInRange);
                    break;
                case TowerEnums.TargetingStyle.Weakest:
                    _target = GetEnemyPosition.Weakest(targetsInRange);
                    break;
                case TowerEnums.TargetingStyle.Closest:
                    _target = GetEnemyPosition.Closest(targetsInRange, transform.position);
                    break;
                default:
                    _target = GetEnemyPosition.First(targetsInRange);
                    break;
            }

            if (_target == null) { return; }
            TargetEnemy();

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
        }


        private void TargetEnemy()
        {
            head.LookAt(_target);
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
