using System.Collections;
using System.Collections.Generic;
using TowerDefense.Data.Towers;
using TowerDefense.Gameplay.Enemies;
using TowerDefense.Towers.TowerAttackControllers;
using Unity.Netcode;
using UnityEngine;

public class FreezeDamage : TowerDamage
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
        enemy.IsFrozen = true;
        //SpawnFireEffectServerRpc(enemy.GetEnemyNetworkObject());

        float startTime = Time.time;
        float currentTime = Time.time;

        enemy.EnemyProperties.MoveSpeed -= EffectProperties.SpeedReduction;

        while ((currentTime - startTime) < EffectProperties.Duration && enemy.HealthRemaining > 0)
        {
            yield return waitTime;
            currentTime = Time.time;
        }

        if (enemy.HealthRemaining > 0)
        {
            enemy.RemoveActiveEffect(ps);
            enemy.EnemyProperties.MoveSpeed += EffectProperties.SpeedReduction;
            enemy.IsFrozen = false;
            NetworkObject psNetwork = ps.GetComponent<NetworkObject>();
            psNetwork.Despawn();
            Destroy(ps);
        }
        else if (ps != null)
        {
            //enemy  doesn't exist in this block
            NetworkObject psNetwork = ps.GetComponent<NetworkObject>();
            psNetwork.Despawn();
            Destroy(ps);
        }

    }
    public override void DoDamage(EnemyController enemy)
    {
        base.DoDamage(enemy);

        if (!enemy.IsFrozen)
        {
            //Debug.Log("FireDamage");
            StartCoroutine(ApplyEffect(enemy));

        }
    }
}
