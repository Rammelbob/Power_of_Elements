using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Speeds")]
    public float moveSpeed;
    //public float axisSpeed;
    //public float rotationSpeed;
    //public float strafSpeed;

    [Header("Physics")]
    public float gravity;
    public float jumpForce;
    public float jumpDecendSpeed;
    

    [Header("External")]
    public LayerMask discludePlayer;
    public Transform direction;
    public Transform body;
    public Animator anim;

    [Header("Surface Control")]
    public float surfaceSlideSpeed;
    //public float slopeClimbingSpeed;
    //public float slopeDecendSpeed;


    private bool isGrounded, isClimbing, isMovementPressed = true;
    private float currentGravity;
    private float delta;
    private float wallDistance = 0.3f;
    private float rotationSpeed = 15;
    private HumanBodyBones[] bones = {HumanBodyBones.Head, HumanBodyBones.LeftUpperArm, HumanBodyBones.RightUpperArm, HumanBodyBones.RightHand, HumanBodyBones.LeftHand,
                                        HumanBodyBones.Spine, HumanBodyBones.RightLowerLeg, HumanBodyBones.LeftLowerLeg, HumanBodyBones.RightFoot, HumanBodyBones.LeftFoot };
    #endregion

    private void Update()
    {
        Gravity();
        SimpleMove();
        Jump();
        FinalMovement();
        Rotation();
    }
    private float jumpHeight;
    private Vector3 moveVector;
    private void SimpleMove()
    {
        Vector3 dir = Input.GetAxis("Vertical") * direction.forward + Input.GetAxis("Horizontal") * direction.right;
        dir.Normalize();
        moveVector = CollisionSlopeCheck(new Vector3(dir.x, 0, dir.z));
    }

    private void FinalMovement()
    {
        Vector3 movement = new Vector3(moveVector.x * moveSpeed, currentGravity + jumpHeight, moveVector.z * moveSpeed) * delta;
        movement = transform.TransformDirection(movement);
        transform.position += movement;
    }

    private Vector3 CollisionSlopeCheck(Vector3 dir)
    {
        Vector3 d = transform.TransformDirection(dir);
        Vector3 l;
        Ray ray;
        RaycastHit hit;
        for (int i = 0; i < bones.Length; i++)
        {
            l = anim.GetBoneTransform(bones[i]).position;
            ray = new Ray(l, d);

            if (Physics.Raycast(ray, out hit, 2, discludePlayer))
            {
                if (hit.distance <= wallDistance)
                {
                    Debug.DrawLine(l, hit.point, Color.yellow, 0.2f);
                    Vector3 temp = Vector3.Cross(hit.normal, d);
                    Debug.DrawRay(hit.point, temp * 20, Color.green, 0.2f);

                    Vector3 myDirection = Vector3.Cross(temp, hit.normal);
                    Debug.DrawRay(hit.point, myDirection * 20, Color.red, 0.2f);

                    Vector3 dir2 = myDirection * surfaceSlideSpeed * moveSpeed * delta;

                    RaycastHit wCheck = WallCheckDetails(dir2, l);
                    if (wCheck.transform != null)
                    {
                        dir2 *= wCheck.distance * 0.5f;
                    }
                    transform.position += dir2;
                    return Vector3.zero;
                }
            }
            if (i == 0)
            {
                if (Physics.Raycast(ray.origin, Vector3.up, wallDistance, discludePlayer))
                {
                    RaycastHit wCheck = WallCheckDetails(Vector3.up, l);
                    if (wCheck.transform != null)
                    {
                        transform.position += Vector3.down;
                    }
                }
            }
        }
        return dir;
    }

    private RaycastHit WallCheckDetails(Vector3 dir,Vector3 pos)
    {
        Vector3 l = pos;
        Ray ray = new Ray(l, dir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 0.2f, discludePlayer))
        {
            return hit;
        }
        return hit;
    }

    private void Jump()
    {
        if (isGrounded)
        {
            if (jumpHeight > 0)
                jumpHeight = 0;

            if (Input.GetButtonDown("Jump"))
            {
                transform.position += Vector3.up * wallDistance * 2;
                jumpHeight += jumpForce;
                
            }
        }
        else
        {
            if (jumpHeight > 0)
            {
                jumpHeight -= (jumpHeight * jumpDecendSpeed * delta);
            }
            else
            {
                jumpHeight = 0;
            }
        }
    }

    private void Gravity()
    {
        delta = Time.deltaTime;
        Vector3 SpherePos = new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 2 - (Vector3.one / 2).y, transform.position.z);
        isGrounded = Physics.CheckSphere(SpherePos, wallDistance, discludePlayer);

        if (isGrounded)
            currentGravity = 0;
        else
            currentGravity += gravity * delta;

        if (isGrounded)
        {
            Ray ray = new Ray(transform.position, Vector3.down * 2);
            RaycastHit hit;

            if (!Input.GetButtonDown("Jump"))
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, discludePlayer))
                {
                    if (hit.distance <= 2)
                    {
                        Debug.DrawRay(ray.origin, ray.direction * 20, Color.green, 0.2f);
                        Vector3 needed = new Vector3(transform.position.x, hit.point.y + transform.localScale.y, transform.position.z);
                        transform.position = needed;
                    }
                    else if (hit.distance > 2)
                    {
                        isGrounded = true;
                        currentGravity += gravity * delta;
                    }
                }
            }
        }
    }

    void Rotation()
    {
        if (isClimbing)
        {
            return;
        }
        Vector3 positionTolookAt = new Vector3(moveVector.x, 0, moveVector.z);

        Quaternion currentrotation = body.rotation;

        if (isMovementPressed && positionTolookAt != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionTolookAt);
            body.rotation = Quaternion.Slerp(currentrotation, targetRotation, rotationSpeed * delta);
        }
    }


    private void OnDrawGizmos()
    {
        Vector3 SpherePos = new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 2 - (Vector3.one / 2).y, transform.position.z);
        if (!isGrounded)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawWireSphere(SpherePos, wallDistance);

        //Gizmos.color = Color.black;
        //Gizmos.DrawLine(anim.GetBoneTransform(HumanBodyBones.Head).position, anim.GetBoneTransform(HumanBodyBones.Head).position + body.forward);
        //Gizmos.DrawLine(anim.GetBoneTransform(HumanBodyBones.RightUpperArm).position, anim.GetBoneTransform(HumanBodyBones.RightUpperArm).position + body.forward);
        //Gizmos.DrawLine(anim.GetBoneTransform(HumanBodyBones.LeftUpperArm).position, anim.GetBoneTransform(HumanBodyBones.LeftUpperArm).position + body.forward);
        //Gizmos.DrawLine(anim.GetBoneTransform(HumanBodyBones.Spine).position, anim.GetBoneTransform(HumanBodyBones.Spine).position + body.forward);
        //Gizmos.DrawLine(anim.GetBoneTransform(HumanBodyBones.RightLowerLeg).position, anim.GetBoneTransform(HumanBodyBones.RightLowerLeg).position + body.forward);
        //Gizmos.DrawLine(anim.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position, anim.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position + body.forward);
        //Gizmos.DrawLine(anim.GetBoneTransform(HumanBodyBones.RightFoot).position, anim.GetBoneTransform(HumanBodyBones.RightFoot).position + body.forward);
        //Gizmos.DrawLine(anim.GetBoneTransform(HumanBodyBones.LeftFoot).position, anim.GetBoneTransform(HumanBodyBones.LeftFoot).position + body.forward);
        //Gizmos.DrawLine(anim.GetBoneTransform(HumanBodyBones.RightHand).position, anim.GetBoneTransform(HumanBodyBones.RightHand).position + body.forward);
        //Gizmos.DrawLine(anim.GetBoneTransform(HumanBodyBones.LeftHand).position, anim.GetBoneTransform(HumanBodyBones.LeftHand).position + body.forward);
    }
}
