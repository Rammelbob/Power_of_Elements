using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExhaustedState : EnemyBaseState
{
    public float staminaPercentToExit;
    public override void EnterState()
    {
        enemyStateManager.movementState.DisableMovement();
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
            //enemyStateManager.movementState.EnableMovement();
            //if (!enemyStateManager.movementState.HandleMovmentChecks())
                enemyStateManager.SwitchState(enemyStateManager.movementState);
        }
    }
}
