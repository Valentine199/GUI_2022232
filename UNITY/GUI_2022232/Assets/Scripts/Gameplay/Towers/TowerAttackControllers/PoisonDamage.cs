using System.Collections;
using System.Collections.Generic;
using TowerDefense.Data.Towers;
using TowerDefense.Gameplay.Enemies;
using TowerDefense.Towers.TowerAttackControllers;
using Unity.Netcode;
using UnityEngine;

public class PoisonDamage : TowerDamage
{
    public TowerEffectProperties EffectProperties { get; private set; }

    public override void InitDamage(TowerProperties properties)
    {
        base.InitDamage(properties);
        EffectProperties = properties.Effect;
    }

    IEnumerator ApplyEffect(EnemyController enemy)
    {
        WaitForSeconds waitTime = new WaitForSeconds(EffectProperties.TickSpeed);
        GameObject ps = Instantiate(EffectProperties.ParticleEffect, enemy.transform);
        NetworkObject psNetworkObject = ps.GetComponent<NetworkObject>();
        psNetworkObject.Spawn();

        psNetworkObject.transform.parent = enemy.transform;
        enemy.AddActiveEffect(ps);
        enemy.IsPoisoned = true;
        //SpawnFireEffectServerRpc(enemy.GetEnemyNetworkObject());

        float startTime = Time.time;
        float currentTime = Time.time;

        enemy.CurrentMoveSpeed -= EffectProperties.SpeedReduction;

        while ((currentTime - startTime) < EffectProperties.Duration && enemy.HealthRemaining > 0)
        {
            enemy.HitEnemy();
            yield return waitTime;
            currentTime = Time.time;
        }

        if (enemy.HealthRemaining > 0)
        {
            enemy.RemoveActiveEffect(ps);
            enemy.CurrentMoveSpeed += EffectProperties.SpeedReduction;
            enemy.IsPoisoned = false;
            NetworkObject psNetwork = ps.GetComponent<NetworkObject>();
            psNetwork.Despawn();
            Destroy(ps);
        }
        else if (ps != null)
        {
            NetworkObject psNetwork = ps.GetComponent<NetworkObject>();
            psNetwork.Despawn();
            Destroy(ps);
        }

    }
    public override void DoDamage(EnemyController enemy)
    {
        base.DoDamage(enemy);

        if (!enemy.IsPoisoned)
        {
            //Debug.Log("FireDamage");
            StartCoroutine(ApplyEffect(enemy));

        }
    }
}
