using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : BaseAnimationManager
{
    PlayerManager playerManager;

    protected override void Awake()
    {
        base.Awake();
        playerManager = GetComponent<PlayerManager>();
    }

    public void PlayPlayerTargetAnimation(string targetAnimation, bool isInteracting, float transitionTime,bool useRootMotion,bool stopRb)
    {
        if (stopRb)
            playerManager.playerLocomotion.rb.velocity = Vector3.zero;
        base.PlayTargetAnimation(targetAnimation, isInteracting, transitionTime, useRootMotion);
    }
}
