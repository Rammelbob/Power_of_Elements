using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour , ICombat
{
    AnimatorManager animatorManager;
    public float maxHP;
    float currentHP;
    public EnemyStautsBar enemyStautsBar;
    public Collider weapon;

    [Header("Combat Stats")]
    public EnemyTyps enemyTyp;
    public float damage;
    public float def;
    public float combatRange;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        enemyStautsBar.UpdateHpMaxValue(maxHP);
        currentHP = maxHP;
        enemyStautsBar.UpdateHpBar(currentHP);
    }

    public void UpdateHP(float hpUpdate)
    {
        currentHP = currentHP + hpUpdate <= 0 ? 0 : currentHP + hpUpdate >= maxHP ? maxHP : currentHP + hpUpdate;
        enemyStautsBar.UpdateHpBar(currentHP);

        if (currentHP == 0)
        {
            Destroy(gameObject);
        }
    }

    public void DoAttack(List<GameObject> hitList)
    {
        foreach (var hit in hitList)
        {
            ICombat combat = hit.GetComponentInParent<ICombat>();
            if (combat != null)
            {
                combat.GetAttacked(-damage, Vector3.zero);
            }
        }
    }

    public void GetAttacked(float damage, Vector3 forceDir)
    {
        float realdamage = damage + def;
        UpdateHP(realdamage);
    }

    public void HandleAttack()
    {
        switch(enemyTyp)
        {
            case EnemyTyps.Axe:
                weapon.enabled = true;
                animatorManager.PlayTargetAnimation($"AxeFight{Random.Range(0, 2)}", true, 0.2f, true, true);
                break;
            case EnemyTyps.Sword:
                break;
            case EnemyTyps.Bow:
                break;
        }
       
    }
}
