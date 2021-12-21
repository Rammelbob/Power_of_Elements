using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public StatusBarManager statusBarManager;
    AnimatorManager animatorManager;

    public float healthLevel;
    public float maxHealth;
    public float currentHealth;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
    }

    private void Start()
    {
        maxHealth = SetMaxHealthFromLevel();
        currentHealth = maxHealth;
        statusBarManager.SetHPBar(0, maxHealth);
    }

    private float SetMaxHealthFromLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        statusBarManager.UpdateHpBar(currentHealth, false);

        animatorManager.PlayTargetAnimation("Take Damage", true, 0.1f, true, true);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animatorManager.PlayTargetAnimation("Death", true, 0.1f, true, true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            TakeDamage(25);
    }
}
