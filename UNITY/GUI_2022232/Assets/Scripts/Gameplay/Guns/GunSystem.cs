using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TowerDefense.Gameplay.Enemies;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GunSystem : NetworkBehaviour
{
    //Gun stats
    [SerializeField] int damage;

    [SerializeField] float timeBetweenShooting;
    [SerializeField] float spread;
    [SerializeField] float range;
    [SerializeField] float reloadTime;
    [SerializeField] float timeBetweenShots;

    [SerializeField] int magazineSize;
    [SerializeField] int bulletsPerTap;

    [SerializeField] bool allowButtonHold;

    int bulletsLeft;
    int bulletsShot;

    bool shooting;
    bool readyToShoot;
    bool reloading;

    public Camera fpsCam;
    [SerializeField] Transform attackPoint;
    [SerializeField] RaycastHit rayHit;
    [SerializeField] LayerMask whatIsEnemy;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] GameObject bulletHoleGraphic;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    private void Update()
    {
        if (!IsOwner) return;
        
        MyInput();

        text.SetText(bulletsLeft + " / " + magazineSize);
    }

    private void MyInput()
    {
        if (allowButtonHold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }

        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);


        //RayCast
        if (Physics.Raycast(fpsCam.transform.position,direction,out rayHit,range)) 
        {
            //Debug.Log(rayHit.collider.name);

            if ((whatIsEnemy.value & (1 << rayHit.collider.gameObject.layer)) > 0)
            {
                if (rayHit.collider.gameObject.TryGetComponent<EnemyController>(out EnemyController enemy))
                {
                    enemy.HitEnemy();
                }
                //rayHit.collider.GetComponent<Enemys>().TakeDamage(damage);
            }
        }

        Destroy(Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0)),0.1f);
        //Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));

        Destroy(Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity),0.01f);
        //Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished() 
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
