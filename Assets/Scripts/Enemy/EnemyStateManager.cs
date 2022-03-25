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
    public Rigidbody rb;

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
        startPosition = transform.position;
    }

    void Start()
    {
        SwitchState(idleState);
    }

    void Update()
    {
        currentState.UpdateState();
    }

    public void SwitchState(EnemyBaseState state)
    {
        currentState = state;
        currentState.EnterState();
    }

    private void LateUpdate()
    {
        canRotate = animator.GetBool("canRotate");
        isInteracting = animator.GetBool("isInteracting");
        useRootMotion = animator.GetBool("useRootMotion");
        canDoCombo = animator.GetBool("canDoCombo");
    }
}
