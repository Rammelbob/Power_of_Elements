using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnmationEvents : MonoBehaviour
{
    public CombatManager combat;
    public AnimatorManager animatorManager;

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
        combat.ComboAttack2();
    }

    private void OnAnimatorMove()
    {
        if (animatorManager.useRootMotion)
        {
            Animator animator = GetComponent<Animator>();
            if (animator && animator.deltaPosition != Vector3.zero)
            {
                Vector3 newPosition = transform.parent.position;
                newPosition.x += animator.deltaPosition.x;
                newPosition.z += animator.deltaPosition.z;
                transform.parent.position = newPosition;
            }
        }
    }
}
