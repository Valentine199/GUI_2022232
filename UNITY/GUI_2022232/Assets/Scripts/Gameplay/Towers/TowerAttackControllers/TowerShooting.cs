using System.Collections.Generic;
using TowerDefense.Gameplay.Enemies;
using TowerDefense.Gameplay.Helpers;
using Unity.Netcode;
using UnityEngine;


namespace TowerDefense.Towers.TowerAttackControllers
{
    public class TowerShooting : NetworkBehaviour
    {
        private TowerEnums.TargetingStyle _targetingStyle;
        [SerializeField]
        [Tooltip("Logical, only neccessery for the model")]
        private Transform _head;

        [SerializeField] private LayerMask _hitLayer;
        [SerializeField] private LayerMask _enemyLayer;

        private List<EnemyController> _targetsInRange = new List<EnemyController>();
        private Queue<EnemyController> _targetsToRemove = new Queue<EnemyController>();
        private Transform _origin;
        private NetworkObject _target = null;
        private Transform _targetTransform = null;
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

        public void TargetingStyleChanged()
        {
            _targetingStyle = _towerController.TargetingStyle;
            GetTarget();
        }

        private void Start()
        {
            if (_towerController != null)
            {
                _origin = _towerController.BulletOrigin.transform;
                _towerController.OnTargetingStyleChanged += TargetingStyleChanged;
            }

        }

        [ServerRpc(RequireOwnership = false)]
        public void AddTargetToInRangeServerRpc(NetworkObjectReference target)
        {
            target.TryGet(out NetworkObject networkTarget);

            EnemyController targetEnemyController = networkTarget.GetComponent<EnemyController>();

            _targetsInRange.Add(targetEnemyController);
            targetEnemyController.OnEnemyDie += HandleOnEnemyKilled;
            GetTarget();
        }

        [ServerRpc(RequireOwnership = false)]
        public void RemoveTargetFromInRangeServerRpc(NetworkObjectReference target)
        {
            target.TryGet(out NetworkObject networkTarget);

            EnemyController targetEnemyController = networkTarget.GetComponent<EnemyController>();

            _targetsInRange.Remove(targetEnemyController);
            targetEnemyController.OnEnemyDie -= HandleOnEnemyKilled;
            GetTarget();
        }

        private bool EnsureSight(EnemyController target)
        {
            if (!IsServer) { return false; }

            if (target == null)
            {
                return false;
            }
            var direction = target.transform.position - _origin.position;
            Debug.DrawRay(_origin.position, direction);
            Ray ray = new Ray(_origin.position, direction);

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
            if (!IsServer) { return; }

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

                SetTargetClientRpc(_target);
            }
        }

        [ClientRpc]
        private void SetTargetClientRpc(NetworkObjectReference target)
        {
            target.TryGet(out NetworkObject networkTarget);

            _targetTransform = networkTarget.GetComponent<Transform>();
        }

        private void HandleOnEnemyKilled(EnemyController enemy)
        {
            if (!IsServer) { return; }
            _targetsToRemove.Enqueue(enemy);
        }

        private void Update()
        {

            RemoveDeadEnemies();

            if (_targetTransform != null && EnsureSight(_targetTransform.GetComponent<EnemyController>()))
            {
                _canShoot = true;
                TargetEnemy();
            }
            else
            {
                _canShoot = false;
                GetTarget();

            }
                _towerController.ShootServerRpc(_canShoot);

            
        }

        private void RemoveDeadEnemies()
        {
            if (!IsServer || _targetsToRemove.Count <= 0) { return; }
            foreach (EnemyController target in _targetsToRemove)
            {
                _targetsInRange.Remove(target);
                _target = null;
            }

            _targetsToRemove.Clear();

        }

        private void TargetEnemy()
        {
            if (!IsClient) { return; }

            _head.LookAt(_targetTransform);
            _origin.LookAt(_targetTransform);
        }
    }
}
