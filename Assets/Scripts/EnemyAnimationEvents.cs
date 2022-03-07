using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public void EnableCombo()
    {
        animator.SetBool("canDoCombo", true);
    }

    public void DisalbeCombo()
    {
        animator.SetBool("canDoCombo", false);
    }

    public void OpenRightDamageCollider()
    {
        return;
    }

    public void CloseRightDamageCollider()
    {
        return;
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
