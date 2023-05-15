using System.Collections;
using TowerDefense.Data.Towers;
using TowerDefense.Gameplay.Enemies;
using Unity.Netcode;
using UnityEngine;

namespace TowerDefense.Towers.TowerAttackControllers
{
    public class TowerDamage : NetworkBehaviour
    {

        public int Damage { get;  private set; }

        public virtual void InitDamage(TowerProperties properties)
        {
            Damage = properties.TowerDamage;
        }

        public virtual void DoDamage(EnemyController enemy)
        {
            enemy.HitEnemy();

        }

        private void OnParticleCollision(GameObject other)
        {
            if(!IsServer) { return; }
            if (other.TryGetComponent<EnemyController>(out EnemyController enemy))
            {
                DoDamage(enemy);
            }
        }
    }
    
}
