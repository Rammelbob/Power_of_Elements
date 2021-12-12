using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotion : MonoBehaviour
{
    AnimatorManager animatorManager;
    public Animator animator;
    NavMeshAgent agent;
    EnemyCombat enemyCombat;

    public bool isInteracting;
    public bool useRootMotion;

    [Header("Field Of View")]
    public float maxFieldofViewDistance;
    public float fieldofViewAngle;
    public LayerMask targetMask;
    public float angerResetTime;
    public float angerResetDistance;
    float angerResetEnd;
    public EnemyStyle enemyStyle;

    bool isAngered;
    bool playerInFieldofView;

    Transform targetPosition;
    Vector3 startPosition;
    float distanceToTarget;

    [Header("Defensive")]
    public float distanceToStart;


    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        agent = GetComponent<NavMeshAgent>();
        enemyCombat = GetComponent<EnemyCombat>();
        startPosition = transform.position;
        angerResetEnd = Time.time;
    }
   

    void FixedUpdate()
    {
        if (isInteracting)
            return;
        CheckPlayerInFieldofView();
        HandleMovement();
        HandleCombat();
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
        if (enemyStyle == EnemyStyle.Defensive)
        {
            if ((startPosition - transform.position).magnitude > distanceToStart)
            {
                isAngered = false;
                return;
            }
        }

        Collider[] colliderAround = Physics.OverlapSphere(transform.position, maxFieldofViewDistance, targetMask);

        if (colliderAround.Length != 0)
        {
            targetPosition = colliderAround[0].transform;
            Vector3 directionToTarget = (targetPosition.position - transform.position).normalized;
            distanceToTarget = (targetPosition.position - transform.position).magnitude;


            RaycastHit hit;
            if(Physics.Raycast(transform.position, directionToTarget, out hit, maxFieldofViewDistance))
                playerInFieldofView = hit.transform.gameObject.CompareTag("Player");

            if (Vector3.Angle(transform.forward, directionToTarget) < fieldofViewAngle / 2 && playerInFieldofView)
            {
                if (!isAngered)
                    isAngered = true;

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
        if (distanceToTarget < enemyCombat.combatRange && isAngered && playerInFieldofView)
        {
            agent.isStopped = true;
            enemyCombat.HandleAttack();
        }
        else if (agent.isStopped)
            agent.isStopped = false;
    }

    private void LateUpdate()
    {
        isInteracting = animator.GetBool("isInteracting");
        useRootMotion = animator.GetBool("useRootMotion");
    }
}
