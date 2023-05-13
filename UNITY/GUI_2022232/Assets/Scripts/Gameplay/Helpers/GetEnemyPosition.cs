using System.Collections;
using System.Collections.Generic;
using TowerDefense.Gameplay.Enemies;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace TowerDefense.Gameplay.Helpers
{
    public static class GetEnemyPosition
    {
        public static NetworkObject First(List<EnemyController> enemies)
        {
            var farthest = enemies[0];
            for (int i = 1; i < enemies.Count; i++)
                farthest = EnemyController.CompareFirst(farthest, enemies[i]);

            return farthest.GetEnemyNetworkObject();
        }

        public static NetworkObject Last(List<EnemyController> enemies)
        {
            var last = enemies[0];
            for (int i = 1; i < enemies.Count; i++)
                last = EnemyController.CompareLast(last, enemies[i]);

            return last.GetEnemyNetworkObject();
        }

        public static NetworkObject Strongest(List<EnemyController> enemies)
        {
            var strongest = enemies[0];
            for (int i = 1; i < enemies.Count; i++)
                strongest = EnemyController.CompareStrongest(strongest, enemies[i]);

            return strongest.GetEnemyNetworkObject();
        }

        public static NetworkObject Weakest(List<EnemyController> enemies)
        {
            var weakest = enemies[0];
            for (int i = 1; i < enemies.Count; i++)
                weakest = EnemyController.CompareWeakest(weakest, enemies[i]);

            return weakest.GetEnemyNetworkObject();   
        }

        public static NetworkObject Closest(List<EnemyController> enemies, Vector3 towerPosition)
        {
            var closest = enemies[0];
            var closestDist = Vector3.Distance(towerPosition, closest.transform.position);
            for (var i = 1; i < enemies.Count; i++)
            {
                float currentDist = Vector3.Distance(towerPosition, enemies[i].transform.position);
                if (currentDist < closestDist)
                {
                    closest = enemies[i];
                    closestDist = currentDist;
                }
            }

            return closest.GetEnemyNetworkObject();
        }
    }
}
