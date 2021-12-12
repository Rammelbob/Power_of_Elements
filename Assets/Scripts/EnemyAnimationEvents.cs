using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    public EnemyLocomotion enemyLocomotion;

    private void OnAnimatorMove()
    {
        if (enemyLocomotion.useRootMotion)
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
