using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerTargeting : MonoBehaviour
{
    private void Start()
    {
        _previousTargetingStyle = currentTargetingStyle;    
    }

    private void Update()
    {

        if (_previousTargetingStyle != currentTargetingStyle)
        {
            //HandleTargetStyleChange();
        }
    }
    //public void AddTargetToInRangeList(Enemy enemy)
    //{
    //    targetsInRange.Add(enemy);
    //    GetCurrentTarget();
    //}

    //public void RemoveTargetFromInRangeList(Enemy enemy)
    //{
    //    targetsInRange.Remove(enemy);
    //    GetCurrentTarget();
    //}

    //private void GetCurrentTarget()
    //{
    //    if (targetsInRange.Count <= 0)
    //    {
    //        currentTarget = null;
    //        OnTargetLost?.Invoke();
    //        return;
    //    }

    //    if (currentTarget != null)
    //    {
    //        currentTarget.OnDeath -= HandleTargetDeath;
    //    }

    //    currentTarget = currentTargetingStyle switch
    //    {
    //        Enums.TargetingStyle.First => targetsInRange.First(),
    //        Enums.TargetingStyle.Last => targetsInRange.Last(),
    //        //Enums.TargetingStyle.Strong => targetsInRange.OrderBy(e => e.BaseHealth).Last(),
    //        //Enums.TargetingStyle.Weak => targetsInRange.OrderBy(e => e.BaseHealth).First(),
    //        _ => targetsInRange.First()
    //    };

    //    currentTarget.OnDeath += HandleTargetDeath;

    //    OnTargetFound?.Invoke(currentTarget);
    //}


    //private void HandleTargetStyleChange()
    //{
    //    _previousTargetingStyle = currentTargetingStyle;
    //    GetCurrentTarget();

    //    Debug.Log("Targeting style changed to: " + currentTargetingStyle);
    //}

    //private void HandleTargetDeath()
    //{
    //    //RemoveTargetFromInRangeList(currentTarget);
    //    //currentTarget.OnDeath -= HandleTargetDeath;
    //    GetCurrentTarget();
    //}

    //private List<Enemy> targetsInRange = new List<Enemy>();
    //[SerializeField] private Enemy currentTarget;
    [SerializeField] private Enums.TargetingStyle currentTargetingStyle = Enums.TargetingStyle.First;
    private Enums.TargetingStyle _previousTargetingStyle;

    //public event Action<Enemy> OnTargetFound;
    public event Action OnTargetLost;
}
