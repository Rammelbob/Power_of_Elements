using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public float maxHP;
    float currentHP;
    public EnemyStautsBar enemyStautsBar;

    private void Awake()
    {
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

}
