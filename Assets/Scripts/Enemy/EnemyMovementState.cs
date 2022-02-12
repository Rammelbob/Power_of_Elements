using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementState : EnemyBaseState
{
    public float angerResetTime;
    public float angerResetDistance;
    public float rotationSpeed;
    float angerResetEnd;

    public override void EnterState(EnemyStateManager enemyStateManager)
    {
        SetAgentDestination(enemyStateManager);
    }

    public override void UpdateState(EnemyStateManager enemyStateManager)
    {
        if (enemyStateManager.isInteracting)
            return;

        if (enemyStateManager.idleState.CheckPlayerInFieldofView(enemyStateManager,enemyStateManager.idleState.maxFieldofViewDistance))
        {
            SetAgentDestination(enemyStateManager);
        }
        else
        {
            enemyStateManager.idleState.CheckPlayerInFieldofView(enemyStateManager, angerResetDistance);
            if (enemyStateManager.idleState.playerInStraightLineTo)
            {
                SetAgentDestination(enemyStateManager);
                angerResetEnd = Time.time + angerResetTime;
            }
                

            if (angerResetEnd <= Time.time)
            {
                enemyStateManager.idleState.isAngered = false;
                SetAgentDestination(enemyStateManager);
            }
        }
        
        HandleEnemyRotation(enemyStateManager,enemyStateManager.agent.nextPosition);
        HandleEnemyMovment(enemyStateManager);
        

        if (enemyStateManager.idleState.distanceToTarget < enemyStateManager.combatState.attackRange)
        {
            enemyStateManager.SwitchState(enemyStateManager.combatState);
        }

        if (Vector3.Distance(transform.position, enemyStateManager.startPosition) < 0.5 && !enemyStateManager.idleState.isAngered)
        {
            enemyStateManager.SwitchState(enemyStateManager.idleState);
        }
    }

    private void SetAgentDestination(EnemyStateManager enemyStateManager)
    {
        if (enemyStateManager.idleState.isAngered)
            enemyStateManager.agent.SetDestination(enemyStateManager.idleState.targetPosition.position);
        else
            enemyStateManager.agent.SetDestination(enemyStateManager.startPosition);

    }

    public void HandleEnemyRotation(EnemyStateManager enemyStateManager,Vector3 rotateTowards)
    {
        Quaternion targetRotation;
        Quaternion enemyRotation;
        Debug.DrawLine(enemyStateManager.agent.nextPosition, enemyStateManager.agent.nextPosition + Vector3.up, Color.black, 0.2f);
        Vector3 directionToNextPosition = (rotateTowards - transform.position).normalized;
        directionToNextPosition.y = 0;
        if (Vector3.Distance(enemyStateManager.body.position, enemyStateManager.agent.nextPosition) < 1f || !enemyStateManager.agent.hasPath)
            return;

        if (directionToNextPosition == Vector3.zero)
            directionToNextPosition = enemyStateManager.body.forward;

        Debug.DrawLine(transform.position, transform.position + directionToNextPosition * 2, Color.red, 0.2f);
        targetRotation = Quaternion.LookRotation(directionToNextPosition);
        enemyRotation = Quaternion.Slerp(enemyStateManager.body.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        enemyStateManager.body.rotation = enemyRotation;
    }

    public void HandleEnemyMovment(EnemyStateManager enemyStateManager)
    {
        enemyStateManager.animatorManager.SetEnemyAnimatorValues(5f);
    }
}
