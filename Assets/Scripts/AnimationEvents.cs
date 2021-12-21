using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public CombatManager combat;
    public PlayerManager playerManager;
    Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void CanCombo()
    {
        combat.CanCombo();
    }

    public void ResetCombo()
    {
        combat.ResetCombo();
    }

    public void ComboAttack2()
    {
        combat.ComboAttack();
    }

    public void SetIFrame(int value)
    {
        combat.SetIFrame(value);
    }

    public void EnableCombo()
    {
        animator.SetBool("canDoCombo", true);
    }

    public void DisalbeCombo()
    {
        animator.SetBool("canDoCombo", false);
    }

    private void OnAnimatorMove()
    {
        if (playerManager.useRootMotion)
        {
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
}
