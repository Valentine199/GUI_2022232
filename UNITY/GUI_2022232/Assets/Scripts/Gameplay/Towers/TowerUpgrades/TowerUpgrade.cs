using System;
using TowerDefense.Data.Towers;
using TowerDefense.Gameplay.Core;
using TowerDefense.Towers.TowerAttackControllers;
using TowerDefense.Towers.TowerEnums;
using TowerDefense.Towers.TowerUpgrades.Interfaces;
using Unity.Netcode;


namespace TowerDefense.Towers.TowerUpgrades
{
    public abstract class TowerUpgrade
    {
        protected TowerUpgradeProperties TowerUpgradeProperties { get; private set; }

        public string UpgradeName { get { return TowerUpgradeProperties.name; } }
        public int Cost { get { return TowerUpgradeProperties.UpgradeCost; } }
        public UpgradeType UpgradeType => TowerUpgradeProperties.UpgradeType;
        public bool IsPurchased { get; private set; }

        public virtual void PurchaseUpgrade(TowerController towerController)
        {
            if (GameController.Instance.Money >= Cost)
            {
                IsPurchased = true;
                GameController.Instance.DecrementMoney(Cost);
                towerController.RequestIncreaseTotalTowerCost(Cost);
            }
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
                base.PurchaseUpgrade(towerController);
                if (IsPurchased)
                {

                    rangeUpdate.SetRangeServerRpc(rangeValue);
                }
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
                base.PurchaseUpgrade(towerController);
                if (IsPurchased)
                {
                    speedUpdate.SetFiringRateServerRpc(SpeedValue);

                }
            }

        }
    }
}