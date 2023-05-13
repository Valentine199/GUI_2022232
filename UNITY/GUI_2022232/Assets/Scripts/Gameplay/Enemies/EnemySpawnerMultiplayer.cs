using Assets.Scripts.Gameplay.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Data.Enemies;
using TowerDefense.Gameplay.Enemies;
using TowerDefense.Gameplay.Path;
using Unity.Netcode;
using UnityEngine;

public class EnemySpawnerMultiplayer : NetworkBehaviour
{
    public void SpawnEnemy(EnemyProperties enemyProperties, Vector3 spawnPosition, int waypointIndex)
    {
        SpawnEnemyServerRpc((int)enemyProperties.EnemyType, spawnPosition, waypointIndex);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnEnemyServerRpc(int enemyPropertyIndex, Vector3 spawnPosition, int waypointIndex)
    {
        EnemyProperties enemyProperties = GetEnemyPropertyFromIndex(enemyPropertyIndex);
        var newEnemy = Instantiate(enemyProperties.prefab, spawnPosition, Quaternion.identity);
        var newEnemyController = newEnemy.GetComponent<EnemyController>();

        NetworkObject enemyNO = newEnemy.GetComponent<NetworkObject>();
        enemyNO.Spawn(true);

        newEnemyController.PathController = _pathController;
        newEnemyController.InitEnemy(enemyProperties, waypointIndex);

        OnEnemySpawned?.Invoke(newEnemyController);

        newEnemy.transform.parent = _enemyParent.transform;
    }

    private void Awake()
    {
        Instance = this;

        _enemyParent = new GameObject("Enemies");
        _enemyParent.AddComponent<NetworkObject>();
    }

    private EnemyProperties GetEnemyPropertyFromIndex(int enemyPropertyIndex)
    {
        return _enemies.enemiesPropertiesList[enemyPropertyIndex];
    }

    public event Action<EnemyController> OnEnemySpawned;

    public static EnemySpawnerMultiplayer Instance { get; private set; }

    [SerializeField] private PathController _pathController;
    [SerializeField] private EnemyPropertiesList _enemies;

    private GameObject _enemyParent;
}
