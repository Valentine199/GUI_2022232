using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TowerDefense.Gameplay.Core;
using TowerDefense.Gameplay.Enemies;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Random = UnityEngine.Random;

public class GunSystem : NetworkBehaviour,ISoundPlayer
{

    [SerializeField] GameObject character;
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
    List<GameObject> weaponGameObject = new List<GameObject>();

    public Camera fpsCam;
    [SerializeField] Transform attackPoint;
    [SerializeField] RaycastHit rayHit;
    [SerializeField] LayerMask whatIsEnemy;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] GameObject bulletHoleGraphic;

    public event Action PlayInitSound;
    public event Action PlayAmbiance;
    public event Action StopAmbiance;
    public event Action PlayEndSound;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        character.GetComponent<PlayerController>().CamFreeze += CanShoot;
    }
    private void Start()
    {
        weaponGameObject.Add(muzzleFlash);
        weaponGameObject.Add(bulletHoleGraphic);
    }
    private void CanShoot()
    {
        readyToShoot = !readyToShoot;
    }
    private void Update()
    {
        if (!IsOwner) return;

        MyInput();

        text.SetText(bulletsLeft + " / " + magazineSize);
    }
    private GameObject GetweaponGameObject(int index)
    { 
        return weaponGameObject[index];
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
            PlayEndSound?.Invoke();
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
        PlayInitSound?.Invoke();

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //RayCast
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);
        Vector3 cameraPosition = fpsCam.transform.position;
        ShootServerRpc(cameraPosition, direction);
        
        Destroy(Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0)), 0.1f);
        BulletHoleServerRPC(rayHit.point);
        //Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));

        Destroy(Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity), 0.01f);
        MuzzleFlashServerRPC(attackPoint.position);
        //Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShootServerRpc(Vector3 cameraPosition, Vector3 direction)
    {
        ShootClientRpc(cameraPosition, direction);
    }

    [ClientRpc]
    private void ShootClientRpc(Vector3 cameraPosition, Vector3 direction)
    {
        if (Physics.Raycast(cameraPosition, direction, out rayHit, range))
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
    }

    [ServerRpc(RequireOwnership = false)]
    public void BulletHoleServerRPC(Vector3 hit)
    {
        BulletHoleClientRPC(hit);
    }
    [ClientRpc]
    public void BulletHoleClientRPC(Vector3 hit)
    {
        if (IsOwner) return;
        Destroy(Instantiate(GetweaponGameObject(1), hit, Quaternion.Euler(0, 180, 0)),0.1f);
    }

    [ServerRpc(RequireOwnership = false)]
    public void MuzzleFlashServerRPC(Vector3 hit)
    {
        MuzzleFlashClientRPC(hit);
    }
    [ClientRpc]
    public void MuzzleFlashClientRPC(Vector3 hit)
    {
        if (IsOwner) return;
        var GG = Instantiate(GetweaponGameObject(0), hit, Quaternion.Euler(0, 180, 0));
        //GG.transform.parent = gameObject.transform.GetChild(0).transform;
        //GG.transform.position = Vector3.zero;
        Destroy(GG,0.05f);
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
