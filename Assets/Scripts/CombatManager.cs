using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public StatusBarManager barManager;
    InputManager inputManager;
    AnimatorManager animator;
    Rigidbody rb;
    PlayerLocomotion playerLocomotion;
    Vector3 respawnPoint;
    int comboNR = 0;
    int maxComboNr = 4;
    bool canCombo;
    float baseHP = 100, baseStamina = 100;
    int bonusHPLvL = 0, bonusStaminaLvL = 0, maxBonusLvL = 10;
    float bonusHPStaminaPerLvL = 10;
    float currentHP, currentStamina;
    bool staminaisFull, hpIsFull, isRecovering;
    float staminaLastUsed = 0, hpLastUsed = 0;
    public float staminaRecovertime, hpRecoverTime;
    public float staminaRecover, hpRecover;

    private void Awake()
    {
        animator = GetComponent<AnimatorManager>();
        rb = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        currentHP = GetMaxHP();
        currentStamina = GetMaxStamina();
        barManager.UpdateHpBar(currentHP, false);
        barManager.UpdateStaminaBar(currentStamina);
        InvokeRepeating("SetRespawnPoint", 0, 2);
    }

    #region DoAttack
    public void Attack1()
    {
        // do Attack 1
    }

    public void Attack2()
    {
        if (comboNR == 0)
        {
            rb.velocity = Vector3.zero;
            animator.PlayTargetAnimation($"{inputManager.currentElement}Attack{comboNR}", true, 0.05f, true);
            comboNR = 1;
            return;
        }

        if (canCombo)
        {
            canCombo = false;
            comboNR++;
        }
    }

    public void ComboAttack2()
    {
        if (comboNR < maxComboNr)
        {
            animator.PlayTargetAnimation($"{inputManager.currentElement}Attack{comboNR - 1}", true, 0.01f, true);
        }
    }

    public void CanCombo()
    {
        canCombo = true;
    }

    public void ResetCombo()
    {
        canCombo = false;
        comboNR = 0;
    }
    #endregion

    #region GetAttacked
    public void UpdateHP(float hpUpdate)
    {
        bool shouldLerp = false;
        if (hpUpdate <= 0)
        {
            hpLastUsed = Time.time;
            shouldLerp = true;
        }
            

        currentHP = currentHP + hpUpdate <= 0 ? 0 : currentHP + hpUpdate;
        barManager.UpdateHpBar(currentHP, shouldLerp);
        if (currentHP == 0)
            Respawn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            UpdateHP(-10);

        if (Input.GetKeyDown(KeyCode.Z))
            IncreaseHP();

        if (Input.GetKeyDown(KeyCode.U))
            IncreaseStamina();

        staminaisFull = currentStamina >= GetMaxStamina();
        hpIsFull = currentHP >= GetMaxHP();

        if (staminaLastUsed + staminaRecovertime < Time.time && !staminaisFull)
        {
            CanUseStamina();
            UpdateStamina(staminaRecover * Time.deltaTime);
        }


        if (hpLastUsed + hpRecoverTime < Time.time && !hpIsFull)
            UpdateHP(staminaRecover * Time.deltaTime);
    }
    #endregion

    public void IncreaseHP()
    {
        if (bonusHPLvL < maxBonusLvL)
        {
            bonusHPLvL++;
            currentHP = GetMaxHP();
            barManager.SetHPBar(bonusHPLvL, GetMaxHP());
            barManager.UpdateHpBar(currentHP, false);
        }
    }

    public float GetMaxHP()
    {
        return baseHP + bonusHPLvL * bonusHPStaminaPerLvL;
    }

    #region Stamina
    public void UpdateStamina(float staminaUpdate)
    {
        if (staminaUpdate < 0)
            staminaLastUsed = Time.time;

        currentStamina = currentStamina + staminaUpdate <= 0 ? 0 : currentStamina + staminaUpdate;
        barManager.UpdateStaminaBar(currentStamina);

        if (currentStamina == 0)
        {
            isRecovering = true;
        }
    }

    public bool CanUseStamina()
    {
        if (isRecovering)
        {
            if (currentStamina >= 10)
                isRecovering = false;

            else
                return false;
        }
        return true;
    }

    public void IncreaseStamina()
    {
        if (bonusStaminaLvL < maxBonusLvL)
        {
            bonusStaminaLvL++;
            currentStamina = GetMaxStamina();
            barManager.SetStaminaBar(bonusStaminaLvL, GetMaxStamina());
            barManager.UpdateStaminaBar(currentStamina);
        }
    }

    public float GetMaxStamina()
    {
        return baseStamina + bonusStaminaLvL * bonusHPStaminaPerLvL;
    }
    #endregion

    #region Respawn
    public void Respawn()
    {
        transform.position = respawnPoint;
    }

    private void SetRespawnPoint()
    {
        if (playerLocomotion.isGrounded)
        {
            respawnPoint = transform.position;
        }
    }
    #endregion
}
