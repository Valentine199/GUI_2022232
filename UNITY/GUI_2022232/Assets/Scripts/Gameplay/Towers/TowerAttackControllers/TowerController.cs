using System;
using TowerDefense.Data.Towers;
using TowerDefense.Gameplay.Enemies;
using TowerDefense.Towers.TowerUpgrades;
using UnityEngine;
using TowerDefense.Towers.TowerEnums;
using static BulletTypeEnums;
using TowerDefense.Gameplay.Core;
using Unity.Netcode;

namespace TowerDefense.Towers.TowerAttackControllers
{
    [RequireComponent(typeof(TowerUpgradeController))]
    [RequireComponent(typeof(TowerManager))]
    public class TowerController : NetworkBehaviour
    {
        private GameObject _particleSysGO;
        private int _towerCost;

        [SerializeField] private TowerProperties _properties;
        [SerializeField] private TowerEnemyDetector _enemyDetector;
        [SerializeField] private TowerShooting _towerShooting;
        [SerializeField] private TowerParticleController _particleController;
        [SerializeField] private TowerUpgradeController _upgradeController;
        [SerializeField] private GameObject _bulletOrigin;
        [SerializeField] private TowerManager _towerManager;

        public TowerProperties Properties { get { return _properties; } private set { _properties = value; } }
        public TowerEnemyDetector EnemyDetector => _enemyDetector;
        public TowerParticleController ParticleControll => _particleController;
        public GameObject BulletOrigin { get { return _bulletOrigin; } private set { _bulletOrigin = value; } }
        public TargetingStyle TargetingStyle { get; private set; }

        private float SellTowerMultiplier => GameController.Instance.SellTowerMultiplier;
        public int SellTowerCost => Mathf.FloorToInt(_towerCost * SellTowerMultiplier);

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

            var bulletScript = GetDamageType(inst);
            bulletScript.InitDamage(_properties);

            _particleController.ChangeParticleSystem(inst.GetComponent<ParticleSystem>());
            _towerShooting.ChangeTargetingStyle(TargetingStyle);

            _towerCost = _properties.TowerCost;

            _enemyDetector.InitTowerDetection();

        }

        public void SellTower()
        {
            GameController.Instance.IncrementMoney(SellTowerCost);
            StopAllCoroutines();
            Destroy(gameObject);
        }

        public void EnemyDetected(Collider other)
        {
            EnemyController target = other.gameObject.GetComponent<EnemyController>();
            if (target != null)
            {
                _towerShooting.AddTargetToInRangeServerRpc(target.GetEnemyNetworkObject());
            }
        }
        public void OnEnemyExit(Collider other)
        {
            EnemyController target = other.gameObject.GetComponent<EnemyController>();
            if (target != null)
            {
                _towerShooting.RemoveTargetFromInRangeServerRpc(target.GetEnemyNetworkObject());
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

        private TowerDamage GetDamageType(GameObject inst)
        {
            if (_properties == null && inst==null) { return null; }
            BulletTypeEnum bulletType = _properties.BulletTypeEnum;

            switch (bulletType)
            {
                case BulletTypeEnum.None:
                    return inst.AddComponent<TowerDamage>();
                case BulletTypeEnum.Fire:
                    return inst.AddComponent<FireDamage>();
                //case BulletTypeEnum.Ice:
                //    break;
                //case BulletTypeEnum.poison:
                //    break;
                default:
                    return null;
            }
        }
    }
}
