using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerDefense.Data.Core
{
    [CreateAssetMenu(fileName = "Wave-", menuName = "Scriptable Objects/Wave Spawn Statistics", order = 0)]
    public class WaveProperties : ScriptableObject
    {
        public List<SpawnPack> SpawnPacks
        {
            get => _spawnPacks;
            set => _spawnPacks = value;
        }

        public int WaveNumber
        {
            get => _waveNumber;
            set => _waveNumber = Mathf.Max(value, 1);
        }

        public float WaveTime
        {
            get => _spawnPacks.Max(sp => sp.InitialSpawnDelay + sp.AmountInGroup * sp.TimeBetweenEnemies);
        }

        public int WorthInLowestTier
        {
            get => _spawnPacks.Sum(sp => sp.WorthInLowestTier);
        }

        public int TotalEnemyCount
        {
            get => _spawnPacks.Sum(sp => sp.TotalEnemyCount);
        }

        [SerializeField] private List<SpawnPack> _spawnPacks;

        private int _waveNumber;
    }
}
