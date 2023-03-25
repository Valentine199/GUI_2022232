using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionRadius : MonoBehaviour
{
    //[SerializeField] private TowerTargeting towerTargeter;
    //[SerializeField] private Transform rangeCircle;
    //[SerializeField] private float towerRange = 30f;
    //private float _previousTowerRange;

    //private void Start()
    //{
    //    towerTargeter ??= GetComponent<TowerTargeting>();
    //    _previousTowerRange = towerRange;
    //}

    //private void Update()
    //{
    //    if (_previousTowerRange != towerRange) 
    //    {
    //        HandleRangeChange();
    //    }

    //}

    //private void HandleRangeChange()
    //{
    //    _previousTowerRange = towerRange;

    //    rangeCircle.localScale = new Vector3(towerRange*2, 1, towerRange*2);
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.TryGetComponent<Enemy>(out Enemy enemy))
    //    {
    //        towerTargeter.AddTargetToInRangeList(enemy);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.TryGetComponent<Enemy>(out Enemy enemy))
    //    {
    //        towerTargeter.RemoveTargetFromInRangeList(enemy);
    //    }
    //}
}
