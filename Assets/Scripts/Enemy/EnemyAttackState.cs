using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public Attack enemyAttack;
    public float attackRange;
    Attack currentAttack;

    public override void EnterState()
    {
        ChaseOrAttack();
    }

    public override void UpdateState()
    {
        //enemyStateManager.combatState.HandleEnemyRotation(enemyStateManager.idleState.targetPosition.position);

        if (enemyStateManager.isInteracting)
            return;

        ChaseOrAttack();

        //else if (enemyStateManager.canDoCombo)
        //{
        //    enemyStateManager.idleState.CheckPlayerInFieldofView(enemyStateManager.idleState.maxFieldofViewDistance);
        //    if (enemyStateManager.idleState.distanceToTarget < combatRange)
        //    {
        //        enemyStateManager.animatorManager.PlayTargetAnimation(currentAttack.nextLightAttack.attackName, true, 0.1f, true, false);
        //        enemyStateManager.animator.SetBool("canDoCombo", false);
        //        currentAttack = currentAttack.nextLightAttack;
        //    }
        //    else
        //    {
        //        enemyStateManager.SwitchState(enemyStateManager.movementState);
        //    }
        //}
    }

    public void ChaseOrAttack()
    {
        enemyStateManager.idleState.CheckPlayerInFieldofView(enemyStateManager.idleState.maxFieldofViewDistance);
        if (enemyStateManager.idleState.distanceToTarget < attackRange)
        {
            enemyStateManager.movementState.DisableMovement();
            enemyStateManager.combatState.HandleEnemyRotation(enemyStateManager.idleState.targetPosition.position);
            enemyStateManager.animatorManager.PlayTargetAnimation(enemyAttack.attackName, true, 0.1f, true, false);
            currentAttack = enemyAttack;
        }
        else
        {
            enemyStateManager.movementState.EnableMovement();
            if (!enemyStateManager.movementState.HandleMovmentChecks())
                enemyStateManager.SwitchState(enemyStateManager.movementState);
        }
    }
}
