using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Towers.TowerUpgrades.Interfaces;
using Unity.Netcode;
using UnityEngine;

namespace TowerDefense.Towers.TowerAttackControllers
{
    public class TowerParticleController : NetworkBehaviour, IUpgradeSpeed
    {
        [SerializeField] private ParticleSystem ps;
        private bool isShooting = false;

        private float firingRate;

        public float FiringRate
        {
            get { return firingRate; }
            set { firingRate = value; }
        }


        private TowerController _towerController;
        public TowerController TowerController
        {
            get
            {
                return _towerController;
            }
            set
            {
                _towerController = value;

            }
        }

        public void ChangeParticleSystem(ParticleSystem particleSys)
        {
            ps = particleSys;
        }

        public void ChangeBulletSpeed(int speed)
        {
            var main = ps.main;
            main.startSpeed = speed;
        }

        [ClientRpc(Delivery = RpcDelivery.Unreliable)]
        public void TowerShootClientRpc(bool canShoot)
        {

            if (isShooting && canShoot) { return; }
            if (!isShooting && !canShoot) { return; }

            if (!isShooting && canShoot)
            {
                StartCoroutine(ShootCoroutine());
            }
            else if (isShooting && !canShoot)
            {
                StopCoroutine(ShootCoroutine());
            }
        }

        IEnumerator ShootCoroutine()
        {
            isShooting = true;
            ps.Emit(1);
            yield return new WaitForSeconds(firingRate);
            isShooting = false;
        }

        [ServerRpc(RequireOwnership =false)]
        public void SetFiringRateServerRpc(float newCooldownTime)
        {
            SetFiringRateClientRpc(newCooldownTime);
        }

        [ClientRpc]
        public void SetFiringRateClientRpc(float newCooldownTime)
        {
            firingRate -= newCooldownTime;
        }
    }
}
