namespace TowerDefense.Towers.TowerUpgrades.Interfaces
{
    public interface IUpgradeSpeed
    {
        void SetFiringRateServerRpc(float newCooldownTime);

        void SetFiringRateClientRpc(float newCooldownTime);
    }
}
