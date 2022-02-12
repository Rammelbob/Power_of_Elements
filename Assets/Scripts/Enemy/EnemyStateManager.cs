using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    EnemyBaseState currentState;

    public EnemyIdleState idleState;
    public EnemyMovementState movementState;
    public EnemyCombatState combatState;
    public NavMeshAgent agent;
    public AnimatorManager animatorManager;
    public Animator animator;
    public Vector3 startPosition;
    public Transform body;

    [Header("Enemy Flags")]
    public bool isInteracting;
    public bool useRootMotion;
    public bool canDoCombo;
    public bool canRotate;


    private void Awake()
    {
        idleState = GetComponentInChildren<EnemyIdleState>();
        movementState = GetComponentInChildren<EnemyMovementState>();
        combatState = GetComponentInChildren<EnemyCombatState>();
        agent.updatePosition = false;
        agent.updateRotation = false;
        startPosition = transform.position;
    }

    void Start()
    {
        SwitchState(idleState);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(EnemyBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    private void LateUpdate()
    {
        canRotate = animator.GetBool("canRotate");
        isInteracting = animator.GetBool("isInteracting");
        useRootMotion = animator.GetBool("useRootMotion");
    }
}