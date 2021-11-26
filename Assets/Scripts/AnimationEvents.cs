using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public CombatManager combat;
    public PlayerManager playerManager;

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

    private void OnAnimatorMove()
    {
        if (playerManager.useRootMotion)
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
}
