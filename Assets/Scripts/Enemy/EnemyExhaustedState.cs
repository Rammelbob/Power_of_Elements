using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExhaustedState : EnemyBaseState
{
    public float staminaPercentToExit;
    public override void EnterState()
    {
        enemyStateManager.animatorManager.animator.SetFloat("movementSpeed", 0, 0, Time.deltaTime);
        enemyStateManager.rb.isKinematic = true;
    }

    public override void UpdateState()
    {
        if (enemyStateManager.enemyStats.GetStatByEnum(StatEnum.Stamina).GetCurrentAmountInPercent() >= staminaPercentToExit)
        {
            enemyStateManager.SwitchState(enemyStateManager.combatState);
        }
        enemyStateManager.idleState.CheckPlayerInFieldofView(enemyStateManager.idleState.maxFieldofViewDistance);
        if (enemyStateManager.idleState.distanceToTarget > enemyStateManager.combatState.attackRange)
        {
            enemyStateManager.movementState.EnableMovement();
            if (!enemyStateManager.movementState.HandleMovmentChecks())
                enemyStateManager.SwitchState(enemyStateManager.movementState);
        }
    }
}
