using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class WaveManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Instantiate(_spline);
        InvokeRepeating(SPAWN_METHOD, _spawnTime, _spawnDelay);
    }

    private void SpawnEnemy()
    {
        Instantiate(_enemies[0], transform.position, transform.rotation);
        if (_stopSpawning)
            CancelInvoke(SPAWN_METHOD);
    }

    private enum EnemyType
    {
        red,
        blue,
        green,
        yellow
    }

    private struct Pack
    {
        public EnemyType type;
        public int count;
        public float cooldown;
    }

    [SerializeField] private List<Enemy> _enemies;
    [SerializeField] private SplineContainer _spline;


    [SerializeField] private bool _stopSpawning;
    [SerializeField] private float _spawnTime;
    [SerializeField] private float _spawnDelay;
}
