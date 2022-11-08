using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatState : EnemyBaseState
{
    public float rotationSpeed;

    public EnemyAttack dodgeAction;
    public List<EnemyAttack> enemyAttacks;
    public float attackRange;
    public EnemyAttack currentAttack;
    EnemyAttack selectedAttack;



    public override void EnterState()
    {
        enemyStateManager.movementState.DisableMovement();
    }

    public override void UpdateState()
    {
        HandleEnemyRotation(enemyStateManager.idleState.targetPosition.position);


        selectedAttack = enemyAttacks[Random.Range(0, enemyAttacks.Count)];

        enemyStateManager.idleState.CheckPlayerInFieldofView(enemyStateManager.idleState.maxFieldofViewDistance);
        if (enemyStateManager.idleState.distanceToTarget < attackRange)
        {
            enemyStateManager.movementState.DisableMovement();
            if (Random.Range(0, 101) % 10 == 0)
            {
                DoAction(dodgeAction);
            }
            else
            {
                DoAction(selectedAttack);
            }

        }
        else
        {
            //enemyStateManager.movementState.EnableMovement();
            //if (!enemyStateManager.movementState.HandleMovmentChecks())
                enemyStateManager.SwitchState(enemyStateManager.movementState);
        }
       
    }

    public void DoAction(EnemyAttack actionToDo)
    {
        if (enemyStateManager.enemyStats.CanUseStamina(actionToDo.staminaCost))
        {
            enemyStateManager.animatorManager.PlayTargetAnimation(actionToDo.name, true, 0.1f, true);
            enemyStateManager.enemyStats.GetStatByEnum(StatEnum.Stamina).statvalues.ChangeCurrentStat(-dodgeAction.staminaCost);
            currentAttack = selectedAttack;
        }
        else
        {
            enemyStateManager.SwitchState(enemyStateManager.exhaustedState);
        }
    }

    public void HandleEnemyRotation(Vector3 rotateTowards)
    {
        if (!enemyStateManager.canRotate)
            return;

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
