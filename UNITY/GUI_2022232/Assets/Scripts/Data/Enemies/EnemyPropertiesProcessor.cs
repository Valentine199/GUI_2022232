using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Data.Enemies
{
    public static class EnemyPropertiesProcessor
    {
        public static EnemyProperties GetEnemyPropertyFromType(EnemyType enemyType)
        {
            if (!_initialized)
                Initialize();

            return _enemies[enemyType];
        }

        /// <summary>
        /// Finds all instantiated SOs that are of type EnemyProperties, then 
        /// fills the dictionary with the type associated with its properties.
        /// </summary>
        private static void Initialize()
        {
            _enemies.Clear();
            Resources.LoadAll("ScriptableObjects");
            if (Resources.FindObjectsOfTypeAll(typeof(EnemyProperties)) is EnemyProperties[] allEnemyProperties)
                foreach (var enemyProperties in allEnemyProperties)
                    _enemies.Add(enemyProperties.EnemyType, enemyProperties);

            _initialized = true;
        }

        private static Dictionary<EnemyType, EnemyProperties> _enemies = new();
        private static bool _initialized;
    }
}
