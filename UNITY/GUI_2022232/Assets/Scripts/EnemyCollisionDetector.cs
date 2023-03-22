using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionDetector : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent<Bullet>(out Bullet bullet))
        {
            int damage = Mathf.RoundToInt(bullet.damage);
            _enemy.TakeDamage(damage);
        }
    }

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    [SerializeField] private Enemy _enemy;
}
