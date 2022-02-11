using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public float maxFieldofViewDistance;
    public float fieldofViewAngle;
    public LayerMask targetMask;
    public Transform targetPosition;
    public float distanceToTarget;
    public bool isAngered;
    public bool playerInStraightLineTo;

    public override void EnterState(EnemyStateManager enemyStateManager)
    {
        enemyStateManager.animatorManager.SetEnemyAnimatorValues(0);
    }

    public override void UpdateState(EnemyStateManager enemyStateManager)
    {
        if (CheckPlayerInFieldofView(enemyStateManager, maxFieldofViewDistance))
        {
            enemyStateManager.SwitchState(enemyStateManager.movementState);
        }
        
    }

    public bool CheckPlayerInFieldofView(EnemyStateManager enemyStateManager,float checkRadius)
    {
        Collider[] colliderAround = Physics.OverlapSphere(transform.position, checkRadius, targetMask);
        playerInStraightLineTo = false;
        if (colliderAround.Length != 0)
        {
            targetPosition = colliderAround[0].transform;
            Vector3 directionToTarget = (targetPosition.position - transform.position).normalized;
            distanceToTarget = (targetPosition.position - enemyStateManager.body.position).magnitude;


            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToTarget, out hit, checkRadius))
            {
                playerInStraightLineTo = hit.transform.gameObject.CompareTag("Player");
            }

            if (Vector3.Angle(enemyStateManager.body.forward, directionToTarget) < fieldofViewAngle / 2 && playerInStraightLineTo)
            {
                isAngered = true;
                return true;
            }
                
        }
        return false;
    }
}
