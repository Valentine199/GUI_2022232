using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Start()
    {
        _prevBulletSpeed = _bulletSpeed;
        _prevFiringRate = _firingRate;
    }

    private void Update()
    {
        if (_prevBulletSpeed != _bulletSpeed)
        {
            ChangeParticleSysBulletSpeed();
        }
        if (_prevFiringRate != _firingRate)
        {
            ChangeParticleSysFiringRate();
        }
    }

    private void ChangeParticleSysBulletSpeed()
    {
        var main = particleSys.main;
        main.startSpeed = _bulletSpeed;

        _prevBulletSpeed = _bulletSpeed;
    }


    private void ChangeParticleSysFiringRate()
    {
        _prevFiringRate = _firingRate;

        var emissionModule = particleSys.emission;
        emissionModule.rateOverTime = _firingRate;
    }

    public void Shoot(bool isActive)
    {
        var emissionModule = particleSys.emission;
        emissionModule.enabled = isActive;
    }

    [SerializeField] private ParticleSystem particleSys;

    [SerializeField] private float _firingRate = 0.5f;  // Emmission.RateOverTime
    [SerializeField] private float _bulletSpeed = 60f;  // particleSystem.StartSpeed, StartLifeTime also needs to be addressed

    private float _prevFiringRate = 0.5f;  // Emmission.RateOverTime
    private float _prevBulletSpeed = 60f;  // particleSystem.StartSpeed, StartLifeTime also needs to be addressed

    public float damage = 1; // Arbitrary
}
