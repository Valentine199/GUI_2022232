using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTargeting : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private ParticleSystem particleSys;
    [SerializeField] private float range = 30f;

    private Transform target = null;

    private void Awake()
    {
       //particleSys.Play();
    }

    private void Update()
    {
        GetTarget();
        TargetEnemy();
    }

    private void GetTarget()
    {
        // Min value search
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        target = null;
        float maxDistance = Mathf.Infinity;

        for (int i = 0; i < enemies.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, enemies[0].transform.position);
            if (distance < range && distance < maxDistance)
            {
                target = enemies[0].transform;
            }
        }

    }
    private void TargetEnemy()
    {
        if (target == null)
        {
            Shoot(false);
        }
        else
        {
            head.LookAt(target);
            Shoot(true);
        }
    }

    private void Shoot(bool isActive)
    {
        var emissionModule = particleSys.emission;
        emissionModule.enabled = isActive;
    }
}
