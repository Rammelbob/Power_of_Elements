using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{

    Animator anim;

    public float healthLevel;
    public float maxHealth;
    public float currentHealth;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        maxHealth = SetMaxHealthFromLevel();
        currentHealth = maxHealth;
    }

    private float SetMaxHealthFromLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        anim.Play("Take Damage");
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            anim.Play("Death");
        }
    }
}
