using System;
using TowerDefense.Data.Towers;
using TowerDefense.Gameplay.Enemies;
using TowerDefense.Towers.TowerUpgrades;
using UnityEngine;
using TowerDefense.Towers.TowerEnums;
using TowerDefense.Gameplay.Core;
using Unity.Netcode;

namespace TowerDefense.Towers.TowerAttackControllers
{
    [RequireComponent(typeof(TowerUpgradeController))]
    public class TowerController : NetworkBehaviour
    {
        //private GameObject _particleSysGO;
        private int _towerCost;

        [SerializeField] private TowerProperties _properties;
        [SerializeField] private TowerEnemyDetector _enemyDetector;
        [SerializeField] private TowerShooting _towerShooting;
        [SerializeField] private TowerParticleController _particleController;
        [SerializeField] private TowerUpgradeController _upgradeController;
        [SerializeField] private GameObject _bulletOrigin;
        [SerializeField] private TowerManager _towerManager;
        [SerializeField] private GameObject ParticleGO;
        [SerializeField] private Camera _snapshotCamera;

        public TowerProperties Properties { get { return _properties; } private set { _properties = value; } }
        public Camera SnapshotCam { get { return _snapshotCamera; } }
        public TowerEnemyDetector EnemyDetector => _enemyDetector;
        public TowerParticleController ParticleControll => _particleController;
        public GameObject BulletOrigin { get { return _bulletOrigin; } private set { _bulletOrigin = value; } }
        public TargetingStyle TargetingStyle { get; private set; }
        public event Action OnTargetingStyleChanged;

        private float SellTowerMultiplier => GameController.Instance.SellTowerMultiplier;
        public int SellTowerCost => Mathf.FloorToInt(_towerCost * SellTowerMultiplier);

        

        private void Awake()
        {
            _enemyDetector.TowerController = this;
            _towerShooting.TowerController = this;
            _particleController.TowerController = this;
            _towerManager.TowerController = this;
            _particleController.FiringRate = _properties.TowerFiringRate;

            //_particleSysGO = _properties.BulletParticleSystem;
            _upgradeController.InitializeUpgrades(this);

            TargetingStyle = _properties.DefaultTowerTargetingStyle;
        }

        private void Start()
        {
            //GameObject inst = Instantiate(_particleSysGO, _bulletOrigin.transform.position, Quaternion.identity);
            //inst.transform.parent = _bulletOrigin.transform;

            var bulletScript = ParticleGO.GetComponent<TowerDamage>();//GetDamageType(ParticleGO);
            bulletScript.InitDamage(_properties);


            _particleController.ChangeParticleSystem(ParticleGO.GetComponent<ParticleSystem>());
            OnTargetingStyleChanged?.Invoke();

            _towerCost = _properties.TowerCost;

            _enemyDetector.InitTowerDetection();

        }

        [ServerRpc(RequireOwnership =false)]
        public void SellTowerServerRpc()
        {
            GameController.Instance.IncrementMoney(SellTowerCost);
            StopAllCoroutines();

            NetworkObject networkTower = gameObject.GetComponent<NetworkObject>();
            networkTower.Despawn();
            Destroy(gameObject);
        }

        [ServerRpc(RequireOwnership = false)]
        public void ChangeRangeVisibilityServerRpc(bool enabled)
        {
            ChangeRangeVisibilityClientRpc(enabled);
        }

        [ClientRpc]
        private void ChangeRangeVisibilityClientRpc(bool isEnabled)
        {
            EnemyDetector.GetComponent<MeshRenderer>().enabled = isEnabled;
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

        [ServerRpc(RequireOwnership =false, Delivery = RpcDelivery.Unreliable)]
        public void ShootServerRpc(bool canShoot)
        {
            _particleController.TowerShootClientRpc(canShoot);
        }


        public void RequestIncreaseTotalTowerCost(int cost)
        {
            IncreaseTotalTowerCostClientRpc(cost);
        }

        [ClientRpc]
        private void IncreaseTotalTowerCostClientRpc(int cost)
        {
            _towerCost += cost;
        }

        [ServerRpc(RequireOwnership = false)]
        public void CycleTargetingModeServerRpc()
        {
            int val = (int)TargetingStyle;
            val++;
            if (val >= Enum.GetNames(typeof(TargetingStyle)).Length)
            {
                val = 0;
            }

            SetOwnTargetingStyleClientRpc(val);
        }

        [ClientRpc]
        public void SetOwnTargetingStyleClientRpc(int val)
        {
            TargetingStyle = (TargetingStyle)val;
            OnTargetingStyleChanged?.Invoke();

        }


        [ServerRpc(RequireOwnership = false)]
        public void CycleTargetingModeBackwardsServerRpc()
        {
            int val = (int)TargetingStyle;
            val--;
            if (val < 0)
            {
                val = Enum.GetNames(typeof(TargetingStyle)).Length - 1;
            }

            SetOwnTargetingStyleClientRpc(val);
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
