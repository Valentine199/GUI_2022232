using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Towers.TowerUpgrades.Interfaces
{
    public interface IUpgradeSpeed
    {
        void SetFiringRate(float newCooldownTime);
    }
}
