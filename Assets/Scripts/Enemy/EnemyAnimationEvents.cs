using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    Animator animator;
    public EnemyStateManager enemyStateManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public void EnableCombo()
    {
        animator.SetBool("canDoCombo", true);
    }

    public void DisableCombo()
    {
        animator.SetBool("canDoCombo", false);
    }

    public void EnableRotation()
    {
        animator.SetBool("canRotat", true);
    }

    public void DisableRotation()
    {
        animator.SetBool("canRotate", false);
    }

    public void OpenRightDamageCollider()
    {
        enemyStateManager.weaponCollider.EnableCombatCollider();
    }

    public void CloseRightDamageCollider()
    {
        enemyStateManager.weaponCollider.DisableCombatCollider(); ;
    }

    private void OnAnimatorMove()
    {
        Animator animator = GetComponent<Animator>();
        if (animator && animator.deltaPosition != Vector3.zero)
        {
            Vector3 newPosition = transform.parent.position;
            newPosition.x += animator.deltaPosition.x;
            newPosition.z += animator.deltaPosition.z;
            newPosition.y += animator.deltaPosition.y;
            transform.parent.position = newPosition;
        }
    }
}
