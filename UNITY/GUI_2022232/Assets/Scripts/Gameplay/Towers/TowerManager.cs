using System.Collections;
using System.Collections.Generic;
using TowerDefense.Towers.TowerAttackControllers;
using TowerDefense.Towers.TowerUpgrades;
using UnityEngine;

namespace TowerDefense.Towers
{
    public class TowerManager : MonoBehaviour
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

        public void CycleTargetingStyleForward()
        {
            _towerController.CycleTargetingMode();
        }

        public void CycleTargetingStyleBackwards()
        {
            _towerController.CycleTargetingModeBackwards();
        }

        public TowerUpgrade GetUpgrade()
        {
            return _towerController.FetchTowerUpgrade();
        }
    }
}
