using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GumballColorChanger : MonoBehaviour
{
    [SerializeField] private ParticleSystem _ps;
    [SerializeField] private Material[] _mats = new Material[4];
    private WaitForSeconds waitTime;

    private System.Random rng;
    private ParticleSystemRenderer _renderer;

    private void Awake()
    {
        waitTime = new WaitForSeconds(2f);
        rng = new System.Random();
        _renderer = _ps.GetComponent<ParticleSystemRenderer>();

        if (_renderer != null && _mats.Count() > 0)
        {
            StartCoroutine(ColorChange());
        }
    }

    private IEnumerator ColorChange()
    {
        while(true)
        {
            int nextColor = rng.Next(0, _mats.Length);

            _renderer.material = _mats[nextColor];

            yield return waitTime;

        }

    }

    private void OnDisable()
    {
        StopCoroutine(ColorChange());
    }




}
