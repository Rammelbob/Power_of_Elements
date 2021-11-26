using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotion : MonoBehaviour
{
    AnimatorManager animatorManager;
    NavMeshAgent agent;

    [Header("Field Of View")]
    public float maxFieldofViewDistance;
    public float fieldofViewAngle;
    public LayerMask targetMask;
    public float angerResetTime;
    public float angerResetDistance;
    float angerResetEnd;

    bool isAngered;
    bool playerInFieldofView;

    Transform targetPosition;
    Vector3 startPosition;
    public float combatRange;
    float distanceToTarget;


    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;
        angerResetEnd = Time.time;
    }
   

    void FixedUpdate()
    {
        CheckPlayerInFieldofView();
        HandleMovement();
        //HandleCombat();
    }

    private void HandleMovement()
    {
        if (isAngered)
        {
            agent.SetDestination(targetPosition.position);
            animatorManager.SetEnemyAnimatorValues(agent.velocity.magnitude);
        }
        else
        {
            agent.SetDestination(startPosition);
            animatorManager.SetEnemyAnimatorValues(agent.velocity.magnitude);
        }
       
    }

    private void CheckPlayerInFieldofView()
    {
        Collider[] colliderAround = Physics.OverlapSphere(transform.position, maxFieldofViewDistance, targetMask);

        if (colliderAround.Length != 0)
        {
            targetPosition = colliderAround[0].transform;
            Vector3 directionToTarget = (targetPosition.position - transform.position).normalized;

            RaycastHit hit;
            if(Physics.Raycast(transform.position, directionToTarget, out hit, maxFieldofViewDistance))
                playerInFieldofView = hit.transform.gameObject.CompareTag("Player");

            if (Vector3.Angle(transform.forward, directionToTarget) < fieldofViewAngle / 2 && playerInFieldofView)
            {
                if (!isAngered)
                    isAngered = true;

                distanceToTarget = (targetPosition.position - transform.position).magnitude;
            }
            else
            {
                if (playerInFieldofView || distanceToTarget > angerResetDistance)
                    angerResetEnd = Time.time + angerResetTime;
                
                if (angerResetEnd <= Time.time)
                    isAngered = false;
            }
        }
    }

    private void HandleCombat()
    {
        if (distanceToTarget < combatRange && isAngered)
        {
            agent.isStopped = true;
            animatorManager.PlayTargetAnimationEnemy("Fight", 0.2f);
        }
        else if(agent.isStopped)
            agent.isStopped = false;
    }
}
