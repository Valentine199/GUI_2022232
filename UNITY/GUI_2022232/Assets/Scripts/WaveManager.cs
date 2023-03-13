using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class WaveManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Instantiate(_enemies[0]);
        Instantiate(_spline);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void SpawnEnemy()
    {

    }

    [SerializeField] private List<Enemy> _enemies;
    [SerializeField] private SplineContainer _spline;
}
