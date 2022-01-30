using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour , ICombat
{
    public StatusBarManager barManager;
    InputManager inputManager;
    AnimatorManager animator;
    Rigidbody rb;
    PlayerLocomotion playerLocomotion;
    PlayerInventory playerInventory;
    Vector3 respawnPoint;
    int comboNR = 0;
    Dictionary<ElementsEnum, int> maxCombNrs = new Dictionary<ElementsEnum, int>()
    {
        {ElementsEnum.Air,4},
        {ElementsEnum.Electro,4},
        {ElementsEnum.Fire,5},
        {ElementsEnum.Ice,4},
        {ElementsEnum.Earth,4},
        {ElementsEnum.Water,4}
    };
    bool canCombo;
    float baseHP = 100, baseStamina = 100;
    int bonusHPLvL = 0, bonusStaminaLvL = 0, maxBonusLvL = 10;
    float bonusHPStaminaPerLvL = 10;
    float currentHP, currentStamina;
    bool staminaisFull, hpIsFull, isRecovering;
    float staminaLastUsed = 0, hpLastUsed = 0;
    public float staminaRecovertime, hpRecoverTime;
    public float staminaRecover, hpRecover;
    public GameObject ElectroWeapon;
    public List<ColliderData> ColliderForElements = new List<ColliderData>();
    bool iFrame;
    public bool useStamina;

    [Header("Combat Stats")]
    public float damage;
    public float def;

    private void Awake()
    {
        animator = GetComponent<AnimatorManager>();
        rb = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerInventory = GetComponent<PlayerInventory>();
        currentHP = GetMaxHP();
        currentStamina = GetMaxStamina();
        barManager.UpdateHpBar(currentHP, false);
        barManager.UpdateStaminaBar(currentStamina);
        InvokeRepeating("SetRespawnPoint", 0, 5);
        ResetCombo();
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
            HandleAttackCollider(playerInventory.currentElement, true, true);

            animator.PlayTargetAnimation($"{playerInventory.currentElement}Attack{comboNR}", true, 0.05f, true,true);
            comboNR = 1;
            return;
        }

        if (canCombo)
        {
            canCombo = false;
            comboNR++;
        }
    }

    public void ComboAttack()
    {
        if (maxCombNrs.TryGetValue(playerInventory.currentElement, out int value))
            if (comboNR < value)
            {
                HandleAttackCollider(playerInventory.currentElement, true, false);
                animator.PlayTargetAnimation($"{playerInventory.currentElement}Attack{comboNR - 1}", true, 0.15f, true, true);
            }
    }

    public void CanCombo()
    {
        canCombo = true;
    }

    public void ResetCombo()
    {
        HandleAttackCollider(playerInventory.currentElement, false, true);
        canCombo = false;
        comboNR = 0;
    }

    public void SetIFrame(int value)
    {
        iFrame = value % 2 == 0;
    }

    public void DoAttack(List<GameObject> hitList)
    {
        foreach (var hit in hitList)
        {
            hit.TryGetComponent<ICombat>(out ICombat combat);
            if (combat != null)
            {
                combat.GetAttacked(-damage, Vector3.zero);
            }
        }
    }

    private void HandleAttackCollider(ElementsEnum currentElement, bool isActive, bool isGameobjectActive)
    {
        foreach (ColliderData data in ColliderForElements)
            if (data.element == currentElement || !isActive)
                foreach (GameObject gameObject in data.collider)
                {
                    if (isGameobjectActive) 
                        gameObject.SetActive(isActive);
                    
                    gameObject.GetComponent<Collider>().enabled = isActive;
                }
    }
    #endregion

    #region GetAttacked
    public void GetAttacked(float damage, Vector3 forceDir)
    {
        float realDamage;
        if (iFrame)
            realDamage = 0;
        else realDamage = damage + def;

        UpdateHP(realDamage);
    }
    #endregion

    public void UpdateHP(float hpUpdate)
    {
        bool shouldLerp = false;
        if (hpUpdate <= 0)
        {
            hpLastUsed = Time.time;
            shouldLerp = true;
        }
            

        currentHP = currentHP + hpUpdate <= 0 ? 0 : currentHP + hpUpdate >= GetMaxHP() ? GetMaxHP() : currentHP + hpUpdate;
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

        //GetAttacked(-Time.deltaTime);

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
        if (!useStamina)
            return;

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

    [Serializable]
    public  struct ColliderData
    {
        public ElementsEnum element;
        public List<GameObject> collider;
    }
}
