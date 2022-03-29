using UnityEngine;

public abstract class EnemyBaseState : MonoBehaviour
{
    public EnemyStateManager enemyStateManager;

    public abstract void EnterState();

    public abstract void UpdateState();
}
