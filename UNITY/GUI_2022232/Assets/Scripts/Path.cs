using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Path : MonoBehaviour
{
    public Transform GetNextWaypoint(Transform currWaypoint)
    {
        // First waypoint
        if (currWaypoint == null)
            return transform.GetChild(0);

        // The next waypoint in order
        int siblingIdx = currWaypoint.GetSiblingIndex();
        if (siblingIdx < transform.childCount - 1)
            return transform.GetChild(siblingIdx + 1);

        // Last waypoint (end of level)
        return null;
    }

    private void OnDrawGizmos()
    {
        // Display a wireframe sphere around each waypoint in scene view
        foreach (Transform t in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(t.position, _waypointSize);
        }

        // Draw line between consecutive waypoints
        Gizmos.color = Color.red;
        for (int i = 0; i < transform.childCount - 1; i++)
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
    }

    [Range(0.0f, 8.0f)]
    [SerializeField] private float _waypointSize = 6.0f;
}