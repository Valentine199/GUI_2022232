using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Towers.TowerUpgrades.Interfaces;
using UnityEngine;

namespace TowerDefense.Towers.TowerAttackControllers
{
    public class TowerEnemyDetector : MonoBehaviour, IUpgradeRange
    {
        private TowerController _towerController;
        [SerializeField] private LayerMask _layerMask;

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

        public void InitTowerDetection()
        {
            var towerCollider = GetComponent<SphereCollider>();
            Collider[] colliders = Physics.OverlapSphere(transform.position, towerCollider.radius, _layerMask);
            foreach (Collider collider in colliders)
            {
                TowerController.EnemyDetected(collider);
            }
        }

        public void SetRange(float range)
        {
            var rangeNow = transform.localScale;
            float newRangeValue = range * 2;
            Vector3 newRange = new Vector3(rangeNow.x + newRangeValue, 0.1f, rangeNow.z + newRangeValue);
            transform.localScale = newRange;
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((_layerMask.value & (1 << other.gameObject.layer)) > 0)
            {
                TowerController.EnemyDetected(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if ((_layerMask.value & (1 << other.gameObject.layer)) > 0)
            {
                TowerController.OnEnemyExit(other);
            }
        }
    }
}
