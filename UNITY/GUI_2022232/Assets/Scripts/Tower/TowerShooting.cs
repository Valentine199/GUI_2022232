using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TowerTargeting))]
public class TowerShooting : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private TowerTargeting targeting;
    [SerializeField] private Bullet bullet;

    private Transform _target = null;

    private void Awake()
    {
        targeting ??= GetComponent<TowerTargeting>();

        if (targeting != null)
        {
            targeting.OnTargetFound += SetTarget;
            targeting.OnTargetLost += SetTargetToNull;
        }
    }

    private void OnDisable()
    {
        if (targeting != null)
        {
            targeting.OnTargetFound -= SetTarget;
            targeting.OnTargetLost -= SetTargetToNull;
        }
    }

    public void SetTarget(Enemy enemy)
    {
        _target = enemy.transform;
    }
    public void SetTargetToNull()
    {
        _target = null;
    }

    private void Update()
    {
        TargetEnemy();
    }

    private void TargetEnemy()
    {
        if (_target == null)
        {
            bullet.Shoot(false);
        }
        else
        {
            head.LookAt(_target);
            bullet.Shoot(true);
        }
    }

}
