using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatState : EnemyBaseState
{
    public float attackRange;
    public Attack[] enemyAttacks;

    public override void EnterState(EnemyStateManager enemyStateManager)
    {
        enemyStateManager.animatorManager.SetEnemyAnimatorValues(0);
    }

    public override void UpdateState(EnemyStateManager enemyStateManager)
    {
        if (enemyStateManager.isInteracting)
            return;

        enemyStateManager.idleState.CheckPlayerInFieldofView(enemyStateManager, enemyStateManager.idleState.maxFieldofViewDistance);
        if (enemyStateManager.idleState.distanceToTarget < attackRange)
        {
            enemyStateManager.movementState.HandleEnemyRotation(enemyStateManager, enemyStateManager.idleState.targetPosition.position);
            enemyStateManager.animatorManager.PlayTargetAnimation(enemyAttacks[Random.Range(0, enemyAttacks.Length)].attackName, true, 0.1f, true, false);
        }
        else
        {
            enemyStateManager.SwitchState(enemyStateManager.movementState);
        }
    }
}
