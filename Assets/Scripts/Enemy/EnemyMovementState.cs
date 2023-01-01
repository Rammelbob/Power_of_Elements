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
        EnableMovement();
    }

    public override void UpdateState()
    {
        HandleMovmentChecks();

        if (enemyStateManager.idleState.distanceToTarget < enemyStateManager.combatState.attackRange)
        {
            enemyStateManager.SwitchState(enemyStateManager.combatState);
        }

        if (Vector3.Distance(transform.position, enemyStateManager.startPosition) < 0.5 && !enemyStateManager.idleState.isAngered)
        {
            enemyStateManager.SwitchState(enemyStateManager.idleState);
        }
    }

    public bool HandleMovmentChecks()
    {
        HandleEnemyMovment(enemyStateManager.agent.velocity.magnitude);
        if (enemyStateManager.idleState.CheckPlayerInFieldofView(enemyStateManager.idleState.maxFieldofViewDistance, enemyStateManager.idleState.fieldofViewAngle))
        {
            SetAgentDestination();
        }
        else
        {
            enemyStateManager.idleState.CheckPlayerInFieldofView(angerResetDistance, enemyStateManager.idleState.fieldofViewAngle);
            if (enemyStateManager.idleState.playerInStraightLineTo)
            {
                SetAgentDestination();
                angerResetEnd = Time.time + angerResetTime;
            }


            if (angerResetEnd <= Time.time)
            {
                enemyStateManager.idleState.isAngered = false;
                enemyStateManager.idleState.targetPosition = enemyStateManager.startPosition;
                SetAgentDestination();
                return false;
            }
        }
        return true;
    }

    private void SetAgentDestination()
    {
        if (enemyStateManager.idleState.isAngered)
            enemyStateManager.agent.SetDestination(enemyStateManager.idleState.targetPosition);
        else
            enemyStateManager.agent.SetDestination(enemyStateManager.startPosition);

    }

    public void HandleEnemyMovment(float value)
    {
        enemyStateManager.animatorManager.animator.SetFloat("movementSpeed", value, 0.01f, Time.deltaTime);
    }

    public void EnableMovement()
    {
        SetAgentDestination();
        HandleEnemyMovment(enemyStateManager.agent.velocity.magnitude);
        enemyStateManager.rb.isKinematic = false;
        enemyStateManager.agent.updateRotation = true;
        enemyStateManager.agent.isStopped = false;
    }

    public void DisableMovement()
    {
        HandleEnemyMovment(0);
        enemyStateManager.rb.isKinematic = true;
        enemyStateManager.agent.updateRotation = false;
        enemyStateManager.agent.isStopped = true;
    }
}
