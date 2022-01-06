using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats : MonoBehaviour
{
    public float baseHP;
    public float currentHP;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        currentHP = baseHP;
    }

    public virtual void TakeDamage(float damage)
    {
        currentHP -= damage;

        anim.Play("Take Damage");
        if (currentHP <= 0)
        {
            currentHP = 0;
            anim.Play("Death");
        }
    }

    public virtual void GainHP(float amount)
    {
        currentHP += amount;
        if (currentHP >= baseHP)
            currentHP = baseHP;
    }
}
