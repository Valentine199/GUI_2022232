using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Data.Towers;
using TowerDefense.Gameplay.Core;
using TowerDefense.Towers.TowerAttackControllers;
using TowerDefense.Towers.TowerEnums;
using TowerDefense.Towers.TowerUpgrades.Interfaces;
using Unity.VisualScripting;
using UnityEngine;


namespace TowerDefense.Towers.TowerUpgrades
{
    public abstract class TowerUpgrade
    {
        protected TowerUpgradeProperties TowerUpgradeProperties { get; private set; }

        public string Name { get { return TowerUpgradeProperties.name; } }
        public int Cost { get { return TowerUpgradeProperties.UpgradeCost; } }
        public UpgradeType UpgradeType => TowerUpgradeProperties.UpgradeType;
        public bool IsPurchased { get; private set; }

        public virtual void PurchaseUpgrade(TowerController towerController)
        {
            IsPurchased = true;
            GameController.Instance.DecrementMoney(Cost);
            towerController.IncreaseTotalTowerCost(Cost);
        }

        public static TowerUpgrade GetNewUpgrade(TowerUpgradeProperties upgradeProperties)
        {
            if (upgradeProperties == null) { return null; }
            var upgradeType = upgradeProperties.UpgradeType;
            switch (upgradeType)
            {
                case UpgradeType.Range:
                    return new RangeUpgrade { TowerUpgradeProperties = upgradeProperties };
                case UpgradeType.Cooldown:
                    return new SpeedUpgrade() { TowerUpgradeProperties = upgradeProperties };
                //case UpgradeType.Damage:
                //return new DurationUpgrade() { TowerUpgradeProperties = upgradeProperties };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class RangeUpgrade : TowerUpgrade
    {
        private float rangeValue => TowerUpgradeProperties._upgradeValue;

        public override void PurchaseUpgrade(TowerController towerController)
        {
            if (towerController.EnemyDetector is IUpgradeRange rangeUpdate)
            {
                rangeUpdate.SetRangeServerRpc(rangeValue);
                base.PurchaseUpgrade(towerController);
            }

        }
    }

    public class SpeedUpgrade : TowerUpgrade
    {
        private float SpeedValue => TowerUpgradeProperties._upgradeValue;

        public override void PurchaseUpgrade(TowerController towerController)
        {
            if (towerController.ParticleControll is IUpgradeSpeed speedUpdate)
            {
                speedUpdate.SetFiringRateServerRpc(SpeedValue);
                base.PurchaseUpgrade(towerController);
            }

        }
    }
}