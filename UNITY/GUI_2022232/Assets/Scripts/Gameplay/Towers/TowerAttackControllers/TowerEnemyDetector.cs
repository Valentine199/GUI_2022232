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

        public void SetRange(float range)
        {
            var rangeNow = transform.localScale;
            float newRangeValue = range * 2;
            Vector3 newRange = new Vector3(rangeNow.x + newRangeValue, 0.1f, rangeNow.z + newRangeValue);
            transform.localScale = newRange;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Got hit!");
            if (other.gameObject.layer == 7)
            {
                TowerController.OnEnemyEnter(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 7)
            {
                TowerController.OnEnemyExit(other);
            }
        }
    }
}
