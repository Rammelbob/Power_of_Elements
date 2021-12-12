using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;
    int horizontal;
    int vertical;
    int climbingX;
    int climbingY;
    int enemyMovementSpeed;


    private void Awake()
    {
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
        climbingX = Animator.StringToHash("InputX");
        climbingY = Animator.StringToHash("InputY");
        enemyMovementSpeed = Animator.StringToHash("MovementSpeed");
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting, Vector2 moveDir)
    {

        animator.SetFloat(climbingX, moveDir.x, 0.1f, Time.deltaTime);
        animator.SetFloat(climbingY, moveDir.y, 0.1f, Time.deltaTime);


        float snappedHorizontal;
        float snappedVertical;

        #region Snapped Horizontal
        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            snappedHorizontal = 0.5f;
        }
        else if (horizontalMovement > 0.55f)
        {
            snappedHorizontal = 1;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            snappedHorizontal = -0.5f;
        }
        else if (horizontalMovement < -0.55f)
        {
            snappedHorizontal = -1;
        }
        else
        {
            snappedHorizontal = 0;
        }
        #endregion
        #region Snapped Vertical
        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            snappedVertical = 0.5f;
        }
        else if (verticalMovement > 0.55f)
        {
            snappedVertical = 1;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            snappedVertical = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            snappedVertical = -1;
        }
        else
        {
            snappedVertical = 0;
        }
        #endregion

        if (isSprinting)
        {
            snappedHorizontal = horizontalMovement;
            snappedVertical = 2;
        }

        animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting, float transitionTime,bool useRootMotion)
    {
        animator.SetBool("isInteracting", isInteracting);
        animator.SetBool("useRootMotion", useRootMotion);
        animator.CrossFade(targetAnimation, transitionTime);
    }

    public void SetEnemyAnimatorValues(float movementSpeed)
    {
        animator.SetFloat(enemyMovementSpeed, movementSpeed, 0.1f, Time.deltaTime);
    }
}
