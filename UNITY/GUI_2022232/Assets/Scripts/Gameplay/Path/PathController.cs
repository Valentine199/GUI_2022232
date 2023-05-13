using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Gameplay.Path
{
    public class PathController : MonoBehaviour
    {
        public Vector3 this[int i] => _waypoints[i].transform.position;

        public int WaypointCount => _waypoints?.Count ?? 0;

        public float WaypointSize => _waypointSize;

        [SerializeField] private List<GameObject> _waypoints;
        [SerializeField] private float _waypointSize;
    }
}
