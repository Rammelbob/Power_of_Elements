using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeClimb : MonoBehaviour
{
    AnimatorManager animatorManager;

    bool inPosition;
    bool isLerping;
    float t;
    float delta;
    Vector3 startPos;
    Vector3 targetPos;


    float hor;
    float vert;
    public bool isMid = true;

    public float possitionOffset;
    public float offsetFromWall = 0.5f;
    public float speed_multiplier = 0.2f;
    public float climbSpeed = 3;
    public float roateSpeed = 5;
    public float rayForwardTowardsWall = 1;
    public float rayTowardsMoveDir = 0.5f;
    public float rayDownwardsWall = 1;

    public IKSnapshot baseIKsnapshot;
    public FreeClimbAnimHook a_hook;
    public Transform direction;
    public Transform body;

    Transform helper;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        helper = new GameObject().transform;
        helper.name = "climb helper";
        a_hook.Init(this, helper);
        animatorManager = GetComponent<AnimatorManager>();
        //CheckForClimb();
    }

    public bool CheckForClimb()
    {
        Vector3 origin = transform.position;
        origin.y += .02f;
        Vector3 dir = direction.forward;
        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit, 0.5f))
        {
            helper.position = PosWithOffset(origin, hit.point);
            InitForClimb(hit);
            return true;
        }
        return false;
    }

    void InitForClimb(RaycastHit hit)
    {
        helper.transform.rotation = Quaternion.LookRotation(-hit.normal);
        startPos = transform.position;
        targetPos = hit.point + (hit.normal * offsetFromWall);
        t = 0;
        inPosition = false;
        //Play animation
    }

    public void Tick(float d_time, Vector2 input)
    {
        delta = d_time;
        if (!inPosition)
        {
            GetInPosition();
            return;
        }

        if (!isLerping)
        {
            hor = input.x;
            vert = input.y;
            float m = Mathf.Abs(hor) + Mathf.Abs(vert);

            Vector3 h = helper.right * hor;
            Vector3 v = helper.up * vert;
            Vector3 moveDir = (h + v).normalized;

            if (isMid)
            {
                if (moveDir == Vector3.zero)
                    return;
            }
            else
            {
                bool canMove = CanMove(moveDir);
                if (!canMove || moveDir == Vector3.zero)
                    return;
            }

            isMid = !isMid;

            t = 0;
            isLerping = true;
            startPos = transform.position;
            Vector3 tp = helper.position - transform.position;
            float d = Vector3.Distance(helper.position, startPos) / 2;
            tp *= possitionOffset;
            tp += transform.position;
            targetPos = (isMid) ? tp : helper.position;

            a_hook.CreatePosition(targetPos, moveDir, isMid, delta);
        }
        else
        {
            t += d_time * climbSpeed;
            if (t > 1)
            {
                t = 1;
                isLerping = false;
            }

            Vector3 cp = Vector3.Lerp(startPos, targetPos, t);
            transform.position = cp;
            body.rotation = Quaternion.Slerp(body.rotation, helper.rotation, d_time * roateSpeed);
        }
    }

    void GetInPosition()
    {
        t += delta * 10;

        if (t > 1)
        {
            t = 1;
            inPosition = true;

            a_hook.CreatePosition(targetPos, Vector3.zero, true, delta);
        }
        Vector3 tp = Vector3.Lerp(startPos, targetPos, t);
        transform.position = tp;
        body.rotation = Quaternion.Slerp(body.rotation, helper.rotation, delta * roateSpeed);
    }

    bool CanMove(Vector3 moveDir)
    {
        Vector3 origin = transform.position;
        float dis = rayTowardsMoveDir;
        Vector3 dir = moveDir;
        RaycastHit hit;

        if (Physics.Raycast(origin, dir, out hit, dis))
        {
            helper.position = PosWithOffset(origin, hit.point);
            helper.rotation = Quaternion.LookRotation(-hit.normal);
            return true;
        }

        origin += moveDir * dis;
        dir = helper.forward;
        float dis2 = rayForwardTowardsWall;

        if (Physics.Raycast(origin, dir, out hit, dis2))
        {
            helper.position = PosWithOffset(origin, hit.point);
            helper.rotation = Quaternion.LookRotation(-hit.normal);
            return true;
        }

        origin = origin + (dir * dis2);
        dir = -moveDir;

        if (Physics.Raycast(origin, dir, out hit, dis2))
        {
            helper.position = PosWithOffset(origin, hit.point);
            helper.rotation = Quaternion.LookRotation(-hit.normal);
            return true;
        }


        origin += dir * dis2;
        dir = -Vector3.up;
        float dis3 = rayDownwardsWall;

        if (Physics.Raycast(origin, dir, out hit, dis3))
        {
            float angle = Vector3.Angle(-helper.forward, hit.normal);
            if (angle < 40)
            {
                helper.position = PosWithOffset(origin, hit.point);
                helper.rotation = Quaternion.LookRotation(-hit.normal);
                return true;
            }
        }
        return false;
    }

    Vector3 PosWithOffset(Vector3 origin, Vector3 target)
    {
        Vector3 direction = origin - target;
        direction.Normalize();
        Vector3 offset = direction * offsetFromWall;
        return target + offset;
    }
    
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(helper.position, 0.5f);
    //}
}

[System.Serializable]
public class IKSnapshot
{
    public Vector3 rh, lh, lf, rf;
}
