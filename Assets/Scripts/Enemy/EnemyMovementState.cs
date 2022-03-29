using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementState : EnemyBaseState
{
    public float angerResetTime;
    public float angerResetDistance;
    float angerResetEnd;

    public override void EnterState()
    {
        SetAgentDestination();
        HandleEnemyMovment();
        enemyStateManager.rb.isKinematic = false;
        enemyStateManager.agent.updateRotation = true;
        enemyStateManager.agent.isStopped = false;
    }

    public override void UpdateState()
    {
        if (enemyStateManager.isInteracting)
            return;

        if (enemyStateManager.idleState.CheckPlayerInFieldofView(enemyStateManager.idleState.maxFieldofViewDistance))
        {
            SetAgentDestination();
        }
        else
        {
            enemyStateManager.idleState.CheckPlayerInFieldofView(angerResetDistance);
            if (enemyStateManager.idleState.playerInStraightLineTo)
            {
                SetAgentDestination();
                angerResetEnd = Time.time + angerResetTime;
            }
                

            if (angerResetEnd <= Time.time)
            {
                enemyStateManager.idleState.isAngered = false;
                SetAgentDestination();
            }
        }
        
        HandleEnemyMovment();
        

        if (enemyStateManager.idleState.distanceToTarget < enemyStateManager.combatState.attackRange)
        {
            enemyStateManager.SwitchState(enemyStateManager.combatState);
        }

        if (Vector3.Distance(transform.position, enemyStateManager.startPosition) < 0.5 && !enemyStateManager.idleState.isAngered)
        {
            enemyStateManager.SwitchState(enemyStateManager.idleState);
        }
    }

    private void SetAgentDestination()
    {
        if (enemyStateManager.idleState.isAngered)
            enemyStateManager.agent.SetDestination(enemyStateManager.idleState.targetPosition.position);
        else
            enemyStateManager.agent.SetDestination(enemyStateManager.startPosition);

    }

    public void HandleEnemyMovment()
    {
        enemyStateManager.animatorManager.SetEnemyAnimatorValues(enemyStateManager.agent.speed);
    }
}
