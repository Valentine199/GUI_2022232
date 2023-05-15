using System.Collections;
using TowerDefense.Data.Towers;
using TowerDefense.Gameplay.Enemies;
using TowerDefense.Towers.TowerAttackControllers;
using Unity.Netcode;
using UnityEngine;

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
        NetworkObject psNetworkObject = ps.GetComponent<NetworkObject>();
        psNetworkObject.Spawn();

        psNetworkObject.transform.parent = enemy.transform;
        enemy.AddActiveEffect(ps);
        //SpawnFireEffectServerRpc(enemy.GetEnemyNetworkObject());

        float startTime = Time.time;
        float currentTime = Time.time;


        while ((currentTime - startTime) < EffectProperties.Duration && enemy.HealthRemaining > 0)
        {
            enemy.HitEnemy();
            yield return new WaitForSeconds(EffectProperties.TickSpeed);
            currentTime = Time.time;
        }

        if (enemy.HealthRemaining > 0)
        {
            enemy.RemoveActiveEffect(ps);
            NetworkObject psNetwork = ps.GetComponent<NetworkObject>();
            psNetwork.Despawn();
            Destroy(ps);
        }
        if (ps != null)
        {
            NetworkObject psNetwork = ps.GetComponent<NetworkObject>();
            psNetwork.Despawn();
            Destroy(ps);
        }
        
    }
    public override void DoDamage(EnemyController enemy)
    {
        base.DoDamage(enemy);

        if (!_isOnFire)
        {
            //Debug.Log("FireDamage");
            StartCoroutine(ApplyEffect(enemy));
        }
    }
}
