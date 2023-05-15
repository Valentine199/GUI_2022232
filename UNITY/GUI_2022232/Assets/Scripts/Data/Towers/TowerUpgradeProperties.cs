using TowerDefense.Towers.TowerEnums;
using UnityEngine;

namespace TowerDefense.Data.Towers
{
    [CreateAssetMenu(fileName = "TowerUpgradePropertie", menuName = "Scriptable Objects/Tower Upgrade Properties", order = 2)]
    public class TowerUpgradeProperties : ScriptableObject
    {
        public string UpgradeName;
        public int UpgradeCost;
        public UpgradeType UpgradeType;
        public float _upgradeValue;
    }
}
