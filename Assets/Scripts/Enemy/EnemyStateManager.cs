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
    public EnemyExhaustedState exhaustedState;
    public NavMeshAgent agent;
    public BaseAnimationManager animatorManager;
    public Animator animator;
    public Vector3 startPosition;
    public Transform body;
    public Rigidbody rb;
    public EnemyStats enemyStats;
    public DamageCollider weaponCollider;

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
        exhaustedState = GetComponentInChildren<EnemyExhaustedState>();
        enemyStats.takeDamage += GetAgro;
        startPosition = transform.position;
    }

    void Start()
    {
        SwitchState(idleState);
    }

    void Update()
    {
        enemyStats.GetStatByEnum(StatEnum.Stamina).statvalues.ChangeCurrentStat(enemyStats.regenPerSec * Time.deltaTime);
        if (idleState.isAngered)
            combatState.HandleEnemyRotation(idleState.targetPosition.position);

        if (isInteracting)
            return;

        currentState.UpdateState();
    }

    public void SwitchState(EnemyBaseState state)
    {
        currentState = state;
        currentState.EnterState();
    }

    public void GetAgro()
    {
        idleState.CheckPlayerInFieldofView(idleState.maxFieldofViewDistance, 360);
    }

    private void LateUpdate()
    {
        canRotate = animator.GetBool("canRotate");
        isInteracting = animator.GetBool("isInteracting");
        useRootMotion = animator.GetBool("useRootMotion");
        canDoCombo = animator.GetBool("canDoCombo");
    }
}
