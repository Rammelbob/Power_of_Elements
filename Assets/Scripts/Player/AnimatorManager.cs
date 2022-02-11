using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;
    Rigidbody rb;
    int climbingX;
    int climbingY;
    int movementSpeed;
    int enemyMovementSpeed;


    private void Awake()
    {
        climbingX = Animator.StringToHash("InputX");
        climbingY = Animator.StringToHash("InputY");
        movementSpeed = Animator.StringToHash("movementSpeed");
        enemyMovementSpeed = Animator.StringToHash("movementSpeed");
        rb = GetComponent<Rigidbody>();
    }

    public void UpdateAnimatorValues(float moveSpeed,Vector2 moveDir)
    {
        animator.SetFloat(climbingX, moveDir.x, 0.1f, Time.deltaTime);
        animator.SetFloat(climbingY, moveDir.y, 0.1f, Time.deltaTime);
        animator.SetFloat(movementSpeed, moveSpeed, 0.1f, Time.deltaTime);
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting, float transitionTime,bool useRootMotion,bool stopRb)
    {
        if (stopRb)
            rb.velocity = Vector3.zero;
        animator.SetBool("isInteracting", isInteracting);
        animator.SetBool("useRootMotion", useRootMotion);
        animator.CrossFade(targetAnimation, transitionTime);
    }

    public void SetEnemyAnimatorValues(float movementSpeed)
    {
        animator.SetFloat(enemyMovementSpeed, movementSpeed, 0, Time.deltaTime);
    }
}
