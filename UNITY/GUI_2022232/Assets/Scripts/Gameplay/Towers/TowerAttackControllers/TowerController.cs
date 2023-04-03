using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerDefense.Data.Towers;
using TowerDefense.Gameplay.Enemies;
using TowerDefense.Towers.TowerUpgrades;
using UnityEditor;
using UnityEngine;
using TowerDefense.Data.Towers;
using TowerDefense.Towers.TowerEnums;
using Unity.VisualScripting;

namespace TowerDefense.Towers.TowerAttackControllers
{
    [RequireComponent(typeof(TowerUpgradeController))]
    [RequireComponent(typeof(TowerManager))]
    public class TowerController : MonoBehaviour
    {
        private GameObject _particleSysGO;
        private int _towerCost;

        [SerializeField] private TowerProperties _properties;
        public TowerProperties Properties { get { return _properties; } private set { _properties = value; } }
        [SerializeField] private TowerEnemyDetector _enemyDetector;
        public TowerEnemyDetector EnemyDetector => _enemyDetector;
        [SerializeField] private TowerShooting _towerShooting;
        [SerializeField] private TowerParticleController _particleController;
        public TowerParticleController ParticleControll => _particleController;
        [SerializeField] private TowerUpgradeController _upgradeController;

        [SerializeField] private GameObject _bulletOrigin;
        public GameObject BulletOrigin { get { return _bulletOrigin; } private set { _bulletOrigin = value; } }
        public TargetingStyle TargetingStyle { get; private set; }
        [SerializeField] private TowerManager _towerManager;

        private void Awake()
        {
            _enemyDetector.TowerController = this;
            _towerShooting.TowerController = this;
            _particleController.TowerController = this;
            _towerManager.TowerController = this;
            _particleController.FiringRate = _properties.TowerFiringRate;

            _particleSysGO = _properties.BulletParticleSystem;
            _upgradeController.InitializeUpgrades(this);

            TargetingStyle = _properties.DefaultTowerTargetingStyle;
        }

        private void Start()
        {
            GameObject inst = Instantiate(_particleSysGO, _bulletOrigin.transform.position, Quaternion.identity);
            inst.transform.parent = _bulletOrigin.transform;
            var bulletScript =inst.AddComponent<BulletDescriptor>();
            bulletScript.Init(_properties.TowerDamage, _properties.BulletTypeEnum);

            _particleController.ChangeParticleSystem(inst.GetComponent<ParticleSystem>());
            _towerShooting.ChangeTargetingStyle(TargetingStyle);

            _towerCost = _properties.TowerCost;

            _enemyDetector.InitTowerDetection();

        }

        public void EnemyDetected(Collider other)
        {
            EnemyController target = other.gameObject.GetComponent<EnemyController>();
            if (target != null)
            {
                _towerShooting.AddTargetToInRange(target);
            }
        }

        public void OnEnemyExit(Collider other)
        {
            EnemyController target = other.gameObject.GetComponent<EnemyController>();
            if (target != null)
            {
                _towerShooting.RemoveTargetFromInRange(target);
            }
        }

        public void Shoot(bool canShoot)
        {
            _particleController.TowerShoot(canShoot);
        }


        public void IncreaseTotalTowerCost(int cost)
        {
            _towerCost += cost;
        }

        public void CycleTargetingMode()
        {
            int val = (int)TargetingStyle;
            val++;
            if (val >= Enum.GetNames(typeof(TargetingStyle)).Length)
            {
                val = 0;
            }

            TargetingStyle = (TargetingStyle)val;
            _towerShooting.ChangeTargetingStyle(TargetingStyle);
        }
        public void CycleTargetingModeBackwards()
        {
            int val = (int)TargetingStyle;
            val--;
            if (val < 0)
            {
                val = Enum.GetNames(typeof(TargetingStyle)).Length - 1;
            }

            TargetingStyle = (TargetingStyle)val;
            _towerShooting.ChangeTargetingStyle(TargetingStyle);
        }

        public TowerUpgrade FetchTowerUpgrade()
        {
            return _upgradeController.GetUpgrade();
        }


        //private void GetCurrentTarget()
        //{
        //    if (targetsInRange.Count <= 0)
        //    {
        //        currentTarget = null;
        //        OnTargetLost?.Invoke();
        //        return;
        //    }

        //    if (currentTarget != null)
        //    {
        //        currentTarget.OnDeath -= HandleTargetDeath;
        //    }

        //    currentTarget.OnDeath += HandleTargetDeath;

        //    OnTargetFound?.Invoke(currentTarget);
        //}



        //private void HandleTargetDeath()
        //{
        //    //RemoveTargetFromInRangeList(currentTarget);
        //    //currentTarget.OnDeath -= HandleTargetDeath;
        //    GetCurrentTarget();
        //}

    }
}
