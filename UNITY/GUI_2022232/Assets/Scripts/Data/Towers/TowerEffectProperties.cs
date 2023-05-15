using UnityEngine;

namespace TowerDefense.Data.Towers
{
    [CreateAssetMenu(fileName = "TowerEffects", menuName = "Scriptable Objects/Tower Effects", order = 3)]
    public class TowerEffectProperties : ScriptableObject
    {
        public string Name { get; }

        public int DamageOverTime;
        public int SpeedReduction;

        public float TickSpeed;
        public float Duration;

        public GameObject ParticleEffect { get; }

    }
}
