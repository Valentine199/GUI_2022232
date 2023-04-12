using System.Collections;
using TowerDefense.Data.Towers;
using TowerDefense.Gameplay.Enemies;
using UnityEngine;

namespace TowerDefense.Towers.TowerAttackControllers
{
    public class TowerDamage : MonoBehaviour
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
            if (other.TryGetComponent<EnemyController>(out EnemyController enemy))
            {
                DoDamage(enemy);
            }
        }
    }
    public class FireDamage : TowerDamage
    {
        private bool _isOnFire = false;
        public TowerEffectProperties EffectProperties { get; private set; }

        public override void InitDamage(TowerProperties properties)
        {
            base.InitDamage(properties);
            EffectProperties = properties.Effect;
        }

        IEnumerator ApplyEffect(EnemyController enemy)
        {
            GameObject ps = Instantiate(EffectProperties.ParticleEffect, enemy.transform);

            float startTime = Time.time;
            float currentTime = Time.time;  


            while ((currentTime - startTime) < EffectProperties.Duration)
            {
                enemy.HitEnemy();
                //Debug.Log("Tick");
                yield return new WaitForSeconds(EffectProperties.TickSpeed);
                //Debug.Log("Tack");
                currentTime = Time.time;
            }

            Destroy(ps);
           // Debug.Log("effect Ended");
        }

        public override void DoDamage(EnemyController enemy)
        {
            base.DoDamage(enemy);

            if(!_isOnFire)
            {
                //Debug.Log("FireDamage");
                StartCoroutine(ApplyEffect(enemy));
            }
        }
    }
}
