using System.Collections;
using System.Collections.Generic;
using TowerDefense.Gameplay.Path;
using UnityEngine;

namespace TowerDefense.Data.Debug
{
    public class PathVisualization : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            // Display a wireframe sphere around each waypoint in scene view
            Gizmos.color = Color.blue;
            for (int i = 0; i < _path.WaypointCount; i++)
                Gizmos.DrawWireSphere(_path[i], _path.WaypointSize);

            // Draw line between consecutive waypoints
            Gizmos.color = Color.red;
            for (int i = 0; i < _path.WaypointCount - 1; i++)
                Gizmos.DrawLine(_path[i], _path[i + 1]);
        }

        [SerializeField] private PathController _path;
    }
}
