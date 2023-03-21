using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    private void Start()
    {
        // Set initial position to first waypoint
        _currWaypoint = _path.GetNextWaypoint(_currWaypoint);
        transform.position = _currWaypoint.position;

        // Set next waypoint target
        _currWaypoint = _path.GetNextWaypoint(_currWaypoint);
        RotateInstant();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _currWaypoint.position, _moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _currWaypoint.position) < _distanceThreshold)
        {
            _currWaypoint = _path.GetNextWaypoint(_currWaypoint);
            RotateInstant();
        }
    }

    //private void RotateSmooth()
    //{
    //    Vector3 direction = (_currWaypoint.position - transform.position).normalized;
    //    Quaternion targetRot = Quaternion.LookRotation(direction);

    //    float distToTarget = (_currWaypoint.position - transform.position).magnitude;
    //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 1.0f);
    //}

    private void RotateInstant()
    {
        transform.LookAt(_currWaypoint);
    }

    [SerializeField] private Path _path;
    [SerializeField] private float _moveSpeed = 7.0f;
    [SerializeField] private float _distanceThreshold = 0.1f;

    // The waypoint that the enemy is moving towards
    private Transform _currWaypoint;
}