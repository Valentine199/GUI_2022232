using System.Collections;
using System.Collections.Generic;
using TowerDefense.Data.Core;
using UnityEngine;

public class DynamicEmptying : MonoBehaviour
{
    [SerializeField] private GameObject _emptyState;
    [SerializeField] private GameObject _25State;
    [SerializeField] private GameObject _50State;
    [SerializeField] private GameObject _75State;
    [SerializeField] private GameObject _100State;

    [SerializeField] private GameStatistics _currentStat;
    [SerializeField] private GameStatistics _maxState;

}
