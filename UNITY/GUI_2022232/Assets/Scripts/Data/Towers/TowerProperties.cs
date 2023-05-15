using TowerDefense.Towers.TowerEnums;
using UnityEngine;

namespace TowerDefense.Data.Towers
{
    [CreateAssetMenu(fileName = "TowerProperties", menuName = "Scriptable Objects/Tower Properties", order = 1)]
    public class TowerProperties : ScriptableObject
    {

        public string TowerName;
        public float TowerRange;
        public int TowerDamage;
        public float TowerFiringRate;
        public int TowerBulletSpeed;
        public int TowerCost;

        public GameObject BulletParticleSystem;
        public GameObject TowerRender;
        public TargetingStyle DefaultTowerTargetingStyle;
        public TowerEffectProperties Effect;

        public TowerUpgradeProperties[] TowerUpgradeOptions;
        public BulletTypeEnum BulletTypeEnum;
    }
}
