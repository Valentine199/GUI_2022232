using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent<Bullet>(out Bullet bullet))
        {
            Debug.Log("I got hit for " + bullet.damage + " damage.");
        }
    }

    private IEnumerator DeathRoutine()
    {
        OnDeath?.Invoke();
        //Play animation
        yield return new WaitForSeconds(1f);
    }

    [SerializeField] private int _baseHealth;
    public int BaseHealth { get { return _baseHealth; } set { _baseHealth = value; } }

    public event Action OnDeath;
}
