using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementState : EnemyBaseState
{
    public float angerResetTime;
    public float angerResetDistance;
    float angerResetEnd;

    public override void EnterState(EnemyStateManager enemyStateManager)
    {
        SetAgentDestination(enemyStateManager);
        enemyStateManager.rb.isKinematic = false;
        enemyStateManager.agent.updateRotation = true;
        enemyStateManager.agent.updatePosition = true;
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

    public void HandleEnemyMovment(EnemyStateManager enemyStateManager)
    {
        enemyStateManager.animatorManager.SetEnemyAnimatorValues(enemyStateManager.agent.speed);
    }
}
