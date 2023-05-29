using System;
using TowerDefense.Towers.TowerAttackControllers;
using TowerDefense.Towers.TowerEnums;
using TowerDefense.Towers.TowerUpgrades;
using Unity.Netcode;
using UnityEngine;

namespace TowerDefense.Towers
{
    public class TowerManager : NetworkBehaviour, IInteractable
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

        private TowerUpgrade _currentUpgrade;
        public event Action <TowerUpgrade> OnNewUpgrade;
        public event Action OnTargetingStyleChange;

        private int _openCount;

        private void Start()
        {
            GetUpgrade();
            _towerController.OnTargetingStyleChanged += TargetingChanged;
            _openCount = 0;
        }

        public Camera GetSnapshotCam()
        {
            return _towerController.SnapshotCam;
        }

        public TargetingStyle GetTargetingInfo()
        {
            return _towerController.TargetingStyle;
        }

        private void TargetingChanged()
        {
            OnTargetingStyleChange?.Invoke();
        }

        public void CycleTargetingStyleForward()
        {
            _towerController.CycleTargetingModeServerRpc();
        }

        public void CycleTargetingStyleBackwards()
        {
            _towerController.CycleTargetingModeBackwardsServerRpc();
        }

        [ClientRpc]
        private void GetUpgradeClientRpc()
        {
            _currentUpgrade = _towerController.FetchTowerUpgrade();
            OnNewUpgrade?.Invoke(_currentUpgrade);
        }

        private void GetUpgrade()
        {
            _currentUpgrade = _towerController.FetchTowerUpgrade();
            OnNewUpgrade?.Invoke(_currentUpgrade);
        }
        public TowerUpgrade GetUpgradeInfo()
        {
            return _currentUpgrade;
        }

        [ServerRpc(RequireOwnership = false)]
        public void InteractUpgradeServerRpc()
        {
            if (_currentUpgrade == null) { return;}

            _currentUpgrade.PurchaseUpgrade(_towerController);

            if(_currentUpgrade.IsPurchased)
            {
                GetUpgradeClientRpc();
            }

        }


        public int GetSellPrice()
        {
            return _towerController.SellTowerCost;
        }

        public void SellTower()
        {
            _towerController.SellTowerServerRpc();
        }

        public void ShowTowerRange()
        {
            _openCount++;
            _towerController.ChangeRangeVisibilityServerRpc(true);
        }

        public void HideTowerRange()
        {
            _openCount--;
            if(_openCount == 0 )
            {
                _towerController.ChangeRangeVisibilityServerRpc(false);
            }
        }

        public string ShowName()
        {
            return _towerController.Properties.TowerName;
        }
    }
}
