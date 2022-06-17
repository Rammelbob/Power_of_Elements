using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    PlayerManager playerManager;
    Vector3 movement;
    Vector3 deltaPositionBuffer;


    private void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
    }

    public void EnableCombo()
    {
        playerManager.playerAnimatorManager.animator.SetBool("canDoCombo", true);
    }

    public void DisalbeCombo()
    {
        playerManager.playerAnimatorManager.animator.SetBool("canDoCombo", false);
    }

    public void DoWeaponCombo()
    {
        playerManager.playerAttacker.HandleWeaponCombo();
    }

    public void EnableRotation()
    {
        playerManager.playerAnimatorManager.animator.SetBool("canRotate", true);
    }

    public void DisableRoation()
    {
        playerManager.playerAnimatorManager.animator.SetBool("canRotate", false);
    }

    private void OnAnimatorMove()
    {
        if (playerManager.useRootMotion)
        {
            deltaPositionBuffer = playerManager.playerAnimatorManager.animator.deltaPosition;
            if (playerManager.playerAnimatorManager.animator && deltaPositionBuffer != Vector3.zero)
            {
                movement = deltaPositionBuffer * playerManager.playerLocomotion.movementMultiplier;
                if (playerManager.playerLocomotion.rb.useGravity)
                    movement.y = playerManager.playerLocomotion.rb.velocity.y;
                playerManager.playerLocomotion.rb.velocity = movement;
            }
        }
    }
}
