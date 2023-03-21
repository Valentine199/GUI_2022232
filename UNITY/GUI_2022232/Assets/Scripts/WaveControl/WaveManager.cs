using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Splines;

public class WaveManager : MonoBehaviour
{
    private void Awake()
    {
        ImportWaveJSON();
    }

    private void Start()
    {
        _enemiesGO = GameObject.Find(ENEMIES_GAMEOBJECT);
        if (_enemiesGO == null )
            _enemiesGO = new GameObject(ENEMIES_GAMEOBJECT);

        Instantiate(_path);
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
        for (int i = 0; i < pack.amount; i++)
        {
            Enemy enemy = Instantiate(_enemies[(int)pack.type], transform.position, transform.rotation);
            enemy.transform.parent = _enemiesGO.transform;
            yield return new WaitForSeconds(pack.spacing);
        }
    }

    private void ImportWaveJSON()
    {
        using (StreamReader sr = new StreamReader(_filepath))
        {
            string json = sr.ReadToEnd();
            _packs = JsonConvert.DeserializeObject<List<Pack>>(json);
        }
    }

    private enum EnemyType
    {
        rockhopper,
        chinstrap,
        adelie,
        macaroni,
        magellanic,
        gentoo,
        king,
        emperor
    }
    private struct Pack
    {
        public EnemyType type;
        public int amount;
        public float spacing;   // seconds between two enemies
    }

    private const string ENEMIES_GAMEOBJECT = "Enemies";

    private GameObject _enemiesGO;

    [SerializeField] private Path _path;
    [SerializeField] private List<Enemy> _enemies;
    [SerializeField] private string _filepath;

    private List<Pack> _packs;
}