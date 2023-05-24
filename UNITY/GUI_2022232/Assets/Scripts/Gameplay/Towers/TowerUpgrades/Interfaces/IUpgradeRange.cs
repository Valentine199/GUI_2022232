using UnityEngine;

namespace TowerDefense.Towers.TowerUpgrades.Interfaces
{
    public interface IUpgradeRange
    {
        void SetRangeServerRpc(float range);

        void SetRangeClientRpc(Vector3 newRange);
    }
}
