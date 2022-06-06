using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeClimbAnimHook : MonoBehaviour
{

    public PlayerAnimatorManager animatorManager;
    IKSnapshot ikBase;
    IKSnapshot current = new IKSnapshot();
    IKSnapshot next = new IKSnapshot();
    IKGoals goals = new IKGoals();

    public float w_rh;
    public float w_rf;
    public float w_lf;
    public float w_lh;

    Vector3 rh, lh, rf, lf;
    Transform h;
    bool isMirrored;
    bool isLeft;
    Vector3 prevMoveDir = new Vector3(0, 1, 0);

    float delta;
    float lerpSpeed;
    bool firstTime;


    public void Init(FreeClimb c, Transform helper)
    {
        ikBase = c.baseIKsnapshot;
        h = helper;
    }

    public void CreatePosition(Vector3 origin, Vector3 movedir, bool isMid, float delta)
    {
        this.delta = delta;
        HandleAnim(movedir, isMid);

        if (!isMid)
        {
            UpdateGoals(movedir);
            prevMoveDir = movedir;
        }
        else
        {
            UpdateGoals(prevMoveDir);
        }

        IKSnapshot ik = CreateSnapshot(origin);
        CopySnapshot(ref current, ik);

        SetIKPosition(isMid, goals.lf, current.lf, AvatarIKGoal.LeftFoot);
        SetIKPosition(isMid, goals.lh, current.lh, AvatarIKGoal.LeftHand);
        SetIKPosition(isMid, goals.rf, current.rf, AvatarIKGoal.RightFoot);
        SetIKPosition(isMid, goals.rh, current.rh, AvatarIKGoal.RightHand);

        UpdateIKWeigth(AvatarIKGoal.LeftFoot, 1);
        UpdateIKWeigth(AvatarIKGoal.LeftHand, 1);
        UpdateIKWeigth(AvatarIKGoal.RightFoot, 1);
        UpdateIKWeigth(AvatarIKGoal.RightHand, 1);
    }

    void UpdateGoals(Vector3 moveDir)
    {
        isLeft = (moveDir.x <= 0);

        if (firstTime)
        {
            goals.lh = firstTime;
            goals.lf = firstTime;
            goals.rh = firstTime;
            goals.rf = firstTime;
            firstTime = false;
            lerpSpeed = 150f;
            return;
        }
        else
        {
            lerpSpeed = 5;
        }

        if (moveDir.x != 0)
        {
            goals.lh = isLeft;
            goals.lf = isLeft;
            goals.rh = !isLeft;
            goals.rf = !isLeft;
        }
        else
        {
            bool isEnabeled = isMirrored;
            if (moveDir.y > 0)
            {
                isEnabeled = !isEnabeled;
            }

            goals.lh = isEnabeled;
            goals.lf = isEnabeled;
            goals.rh = !isEnabeled;
            goals.rf = !isEnabeled;
        }

    }

    void HandleAnim(Vector3 moveDir, bool isMid)
    {
        if (isMid)
        {
            if (moveDir.y != 0)
            {
                if (moveDir.x == 0)
                {
                    isMirrored = !isMirrored;
                    animatorManager.animator.SetBool("IsMirrored", isMirrored);
                }
                else
                {
                    if (moveDir.y < 0)
                    {
                        isMirrored = (moveDir.x < 0);
                        animatorManager.animator.SetBool("IsMirrored", isMirrored);
                    }
                    else
                    {
                        isMirrored = (moveDir.x > 0);
                        animatorManager.animator.SetBool("IsMirrored", isMirrored);
                    }
                }
                //animatorManager.PlayTargetAnimation("climb_up", false, 0.1f);
            }
        }
        else
        {
            //play animation
        }
    }

    public IKSnapshot CreateSnapshot(Vector3 o)
    {
        IKSnapshot r = new IKSnapshot();
        r.rh = GetPosActual(LocalToWorld(ikBase.rh), AvatarIKGoal.RightHand);
        r.rf = GetPosActual(LocalToWorld(ikBase.rf), AvatarIKGoal.RightFoot);
        r.lh = GetPosActual(LocalToWorld(ikBase.lh), AvatarIKGoal.LeftHand);
        r.lf = GetPosActual(LocalToWorld(ikBase.lf), AvatarIKGoal.LeftFoot);
        return r;
    }

    public float wallOffset = 0.1f;

    Vector3 GetPosActual(Vector3 o, AvatarIKGoal goal)
    {
        Vector3 r = o;
        Vector3 origin = o;
        Vector3 dir = h.forward;
        origin += -(dir * 0.2f);
        RaycastHit hit;


        bool isHit = false;
        if (Physics.Raycast(origin, dir, out hit, 2f))
        {
            Vector3 _r = hit.point + (hit.normal * wallOffset);
            r = _r;
            isHit = true;

            if (goal == AvatarIKGoal.LeftFoot || goal == AvatarIKGoal.RightFoot)
            {
                if (hit.point.y > transform.position.y - 0.1f)
                {
                    isHit = false;
                }
            }
        }

        if (!isHit)
        {
            switch (goal)
            {
                case AvatarIKGoal.LeftFoot:
                    r = LocalToWorld(ikBase.lf);
                    break;
                case AvatarIKGoal.LeftHand:
                    r = LocalToWorld(ikBase.lh);
                    break;
                case AvatarIKGoal.RightFoot:
                    r = LocalToWorld(ikBase.rf);
                    break;
                case AvatarIKGoal.RightHand:
                    r = LocalToWorld(ikBase.rh);
                    break;
                default:
                    break;
            }

        }
        return r;
    }

    Vector3 LocalToWorld(Vector3 p)
    {
        Vector3 r = h.position;
        r += h.right * p.x;
        r += h.forward * p.z;
        r += h.up * p.y;
        return r;
    }

    public void CopySnapshot(ref IKSnapshot to, IKSnapshot from)
    {
        to.rh = from.rh;
        to.rf = from.rf;
        to.lh = from.lh;
        to.lf = from.lf;
    }

    void SetIKPosition(bool isMid, bool isTrue, Vector3 pos, AvatarIKGoal goal)
    {
        if (isMid)
        {
            if (isTrue)
            {
                Vector3 p = GetPosActual(pos, goal);
                UpdateIKPosition(goal, p);
            }
        }
        else
        {
            if (!isTrue)
            {
                Vector3 p = GetPosActual(pos, goal);
                UpdateIKPosition(goal, p);
            }
        }
    }

    public void UpdateIKPosition(AvatarIKGoal goal, Vector3 pos)
    {
        switch (goal)
        {
            case AvatarIKGoal.LeftFoot:
                lf = pos;
                break;
            case AvatarIKGoal.LeftHand:
                lh = pos;
                break;
            case AvatarIKGoal.RightFoot:
                rf = pos;
                break;
            case AvatarIKGoal.RightHand:
                rh = pos;
                break;
        }
    }
    public void UpdateIKWeigth(AvatarIKGoal goal, float w)
    {
        switch (goal)
        {
            case AvatarIKGoal.LeftFoot:
                w_lf = w;
                break;
            case AvatarIKGoal.LeftHand:
                w_lh = w;
                break;
            case AvatarIKGoal.RightFoot:
                w_rf = w;
                break;
            case AvatarIKGoal.RightHand:
                w_rh = w;
                break;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        delta = Time.deltaTime;
        SetIKPos(AvatarIKGoal.LeftHand, lh, w_lh);
        SetIKPos(AvatarIKGoal.LeftFoot, lf, w_lf);
        SetIKPos(AvatarIKGoal.RightFoot, rf, w_rf);
        SetIKPos(AvatarIKGoal.RightHand, rh, w_rh);
    }

    void SetIKPos(AvatarIKGoal goal, Vector3 tp, float w)
    {
        IKStates ikState = GetIKStates(goal);
        if (ikState == null)
        {
            ikState = new IKStates();
            ikState.goal = goal;
            ikStates.Add(ikState);
        }

        if (w == 0)
        {
            ikState.isSet = false;
        }

        if (!ikState.isSet)
        {
            ikState.position = GoalBodyBones(goal).position;
            ikState.isSet = true;
        }

        ikState.positionWeight = w;
        ikState.position = Vector3.Lerp(ikState.position, tp, delta * lerpSpeed);

        animatorManager.animator.SetIKPositionWeight(goal, ikState.positionWeight);
        animatorManager.animator.SetIKPosition(goal, ikState.position);
    }

    Transform GoalBodyBones(AvatarIKGoal goal)
    {
        switch (goal)
        {
            case AvatarIKGoal.LeftFoot:
                return animatorManager.animator.GetBoneTransform(HumanBodyBones.LeftFoot);

            case AvatarIKGoal.LeftHand:
                return animatorManager.animator.GetBoneTransform(HumanBodyBones.LeftHand);

            case AvatarIKGoal.RightFoot:
                return animatorManager.animator.GetBoneTransform(HumanBodyBones.RightFoot);

            default:
            case AvatarIKGoal.RightHand:
                return animatorManager.animator.GetBoneTransform(HumanBodyBones.RightHand);
        }
    }

    IKStates GetIKStates(AvatarIKGoal goal)
    {
        IKStates r = null;
        foreach (IKStates i in ikStates)
        {
            if (i.goal == goal)
            {
                r = i;
                break;
            }
        }
        return r;
    }

    List<IKStates> ikStates = new List<IKStates>();

    class IKStates
    {
        public AvatarIKGoal goal;
        public Vector3 position;
        public float positionWeight;
        public bool isSet = false;
    }

    private void OnEnable()
    {
        firstTime = true;
    }
}

public class IKGoals
{
    public bool rh;
    public bool rf;
    public bool lh;
    public bool lf;
}
