using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Data.Enemies;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Enemies
{
    [CreateAssetMenu()]
    public class EnemyPropertiesList : ScriptableObject
    {
        [SerializeField] public List<EnemyProperties> enemiesPropertiesList;
    }
}
