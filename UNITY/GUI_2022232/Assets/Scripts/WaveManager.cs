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
        StartCoroutine(StartWave());
    }

    private IEnumerator StartWave()
    {
        for (int i = 0; i < _packs.Count; i++)
        {
            yield return StartCoroutine(SpawnPack(_packs[i]));
        }
    }

    private IEnumerator SpawnPack(Pack pack)
    {
        for (int i = 0; i < pack.count; i++)
        {
            Instantiate(_enemies[(int)pack.type], transform.position, transform.rotation);
            yield return new WaitForSeconds(pack.cooldown);
        }
        yield return null;
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

    // For testing purposes
    private readonly List<Pack> _packs = new List<Pack>()
    {
        new Pack() { type = EnemyType.green, count = 10, cooldown = 0.8f },
        new Pack() { type = EnemyType.red, count = 11, cooldown = 0.9f },
        new Pack() { type = EnemyType.blue, count = 5, cooldown = 0.5f },
        new Pack() { type = EnemyType.yellow, count = 3, cooldown = 0.9f }
    };
}
