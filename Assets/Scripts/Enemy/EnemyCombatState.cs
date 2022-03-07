using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatState : EnemyBaseState
{
    public float attackRange;
    // public Attack[] enemyAttacks; enemyAttacks[Random.Range(0, enemyAttacks.Length)]
    public Attack enemyAttack;
    public float rotationSpeed;
    Attack currentAttack;

    public override void EnterState(EnemyStateManager enemyStateManager)
    {
        enemyStateManager.animatorManager.SetEnemyAnimatorValues(0);
        enemyStateManager.agent.updateRotation = false;
        enemyStateManager.agent.updatePosition = false;
    }

    public override void UpdateState(EnemyStateManager enemyStateManager)
    {
        HandleEnemyRotation(enemyStateManager, enemyStateManager.idleState.targetPosition.position);
        if (!enemyStateManager.isInteracting)
        {
            enemyStateManager.idleState.CheckPlayerInFieldofView(enemyStateManager, enemyStateManager.idleState.maxFieldofViewDistance);
            if (enemyStateManager.idleState.distanceToTarget < attackRange)
            {
                enemyStateManager.animatorManager.PlayTargetAnimation(enemyAttack.attackName, true, 0.1f, true, false);
                currentAttack = enemyAttack;
            }
            else
            {
                enemyStateManager.SwitchState(enemyStateManager.movementState);
            }
        }
        else if (enemyStateManager.canDoCombo)
        {
            enemyStateManager.idleState.CheckPlayerInFieldofView(enemyStateManager, enemyStateManager.idleState.maxFieldofViewDistance);
            if (enemyStateManager.idleState.distanceToTarget < attackRange)
            {
                enemyStateManager.animatorManager.PlayTargetAnimation(currentAttack.nextLightAttack.attackName, true, 0.1f, true, false);
                enemyStateManager.animator.SetBool("canDoCombo", false);
                currentAttack = currentAttack.nextLightAttack;
            }
            else
            {
                enemyStateManager.SwitchState(enemyStateManager.movementState);
            }
        }
        else
            return;
    }

    public void HandleEnemyRotation(EnemyStateManager enemyStateManager, Vector3 rotateTowards)
    {
        Quaternion targetRotation;
        Quaternion enemyRotation;
        Debug.DrawLine(enemyStateManager.agent.nextPosition, enemyStateManager.agent.nextPosition + Vector3.up, Color.black, 0.2f);
        Vector3 directionToNextPosition = (rotateTowards - transform.position).normalized;
        directionToNextPosition.y = 0;

        if (directionToNextPosition == Vector3.zero)
            directionToNextPosition = enemyStateManager.body.forward;

        Debug.DrawLine(transform.position, transform.position + directionToNextPosition * 2, Color.red, 0.2f);
        targetRotation = Quaternion.LookRotation(directionToNextPosition);
        enemyRotation = Quaternion.Slerp(enemyStateManager.body.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        enemyStateManager.body.rotation = targetRotation;
    }
}
