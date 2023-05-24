using System.Collections.Generic;
using TowerDefense.Towers.TowerUpgrades;
using Unity.Netcode;
using UnityEngine;

namespace TowerDefense.Towers.TowerAttackControllers
{
    public class TowerUpgradeController : NetworkBehaviour
    {
        private List<TowerUpgrade> _upgrades = new List<TowerUpgrade>();

        public List<TowerUpgrade> Upgrades
        {
            get { return _upgrades; }
            set { _upgrades = value; }
        }

        private int _upgradeIndex = -1;

        public void InitializeUpgrades(TowerController controller)
        {
            var towerProperties = controller.Properties;

            foreach (var item in towerProperties.TowerUpgradeOptions)
            {
                _upgrades.Add(TowerUpgrade.GetNewUpgrade(item));
            }
        }

        public TowerUpgrade GetUpgrade()
        {
            _upgradeIndex++;
            if (_upgradeIndex < _upgrades.Count)
            {
                return _upgrades[_upgradeIndex];
            }

            return null;
        }

    }
}
