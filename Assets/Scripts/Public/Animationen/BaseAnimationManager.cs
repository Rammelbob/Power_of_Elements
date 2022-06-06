using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAnimationManager : MonoBehaviour
{
    public Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting, float transitionTime,bool useRootMotion)
    {
        animator.SetBool("useRootMotion", useRootMotion);
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnimation, transitionTime);
    }

}
