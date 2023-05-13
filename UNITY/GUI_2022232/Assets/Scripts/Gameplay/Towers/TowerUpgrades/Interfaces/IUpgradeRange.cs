using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace TowerDefense.Towers.TowerUpgrades.Interfaces
{
    public interface IUpgradeRange
    {
        [ServerRpc(RequireOwnership =false)]
        void SetRangeServerRpc(float range);

        [ClientRpc]
        void SetRangeClientRpc(Vector3 newRange);
    }
}
