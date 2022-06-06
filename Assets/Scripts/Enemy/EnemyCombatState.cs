using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatState : EnemyBaseState
{
    public float combatRange;
    public float chaseOrDodgeValue;
    public float rotationSpeed;
    

    public override void EnterState()
    {
        enemyStateManager.movementState.DisableMovement();
    }

    public override void UpdateState()
    {
        HandleEnemyRotation(enemyStateManager.idleState.targetPosition.position);

        if (enemyStateManager.isInteracting)
            return;

        enemyStateManager.idleState.CheckPlayerInFieldofView(enemyStateManager.idleState.maxFieldofViewDistance);
        if (enemyStateManager.idleState.distanceToTarget  < combatRange - chaseOrDodgeValue)
        {
            enemyStateManager.animatorManager.PlayTargetAnimation("Enemy_Dodge_Back", true, 0.1f, true);
        }
        else if(enemyStateManager.idleState.distanceToTarget  > combatRange + chaseOrDodgeValue)
        {
            enemyStateManager.SwitchState(enemyStateManager.attackState);
        }
       
    }

    public void HandleEnemyRotation(Vector3 rotateTowards)
    {
        //if (!enemyStateManager.canRotate)
        //    return;

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
