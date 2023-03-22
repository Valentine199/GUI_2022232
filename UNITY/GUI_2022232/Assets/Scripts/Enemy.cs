using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyCollisionDetector))]
public class Enemy : MonoBehaviour
{
    private void Start()
    {
        _currentHealth = _baseHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            StartCoroutine(DeathRoutine());
        }
    }

    private IEnumerator DeathRoutine()
    {
        OnDeath?.Invoke();
        //Play animation
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    [SerializeField] private int _baseHealth = 100;
    [SerializeField] private int _currentHealth;
    public int BaseHealth { get { return _baseHealth; } set { _baseHealth = value; } }
    

    public event Action OnDeath;
}
