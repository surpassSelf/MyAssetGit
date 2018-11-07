using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FSMState
{
    private CSGameRoleBase Role;
    public CSGameRoleBase mRole { get { return Role; } }

    private bool mbIsWait = false;
    private float mfWaitStartTime = 0f;
    private float mfWaitTime = 0f;
    private int mnWaitBehavior = -1;

    private BehaviorState StartBehavior = null;
    private BehaviorState mStartBehavior { get { return StartBehavior; } set { StartBehavior = value; } }

    private BehaviorState mCurrentBehavior = null;
    public int mCurrentState { get { return mCurrentBehavior == null ? -1 : mCurrentBehavior.mnBehavior; } }

    private BehaviorState OverrideBehavior = null;
    public BehaviorState mOverrideBehavior { get { return OverrideBehavior; } set { OverrideBehavior = value; } }

    private List<BehaviorState> lisBehaviorStates = new List<BehaviorState>();
    private List<BehaviorState> mLisBehaviorStates { get { return lisBehaviorStates; } set { lisBehaviorStates = value; } }
    public FSMState(CSGameRoleBase roleBase)
    {
        Role = roleBase;
    }

    public BehaviorState GetBehavior(int nBehavior)
    {
        for (int i = 0; i < mLisBehaviorStates.Count; i++)
        {
            BehaviorState beState = mLisBehaviorStates[i];
            if (beState.mnBehavior == nBehavior)
            {
                return beState;
            }
        }

        return null;
    }

    public void InitAddBehavior(BehaviorState behaviorState)
    {
        if (!mLisBehaviorStates.Contains(behaviorState))
        {
            mLisBehaviorStates.Add(behaviorState);
        }
    }

    public void Start(int nBehavior)
    {
        mStartBehavior = GetBehavior(nBehavior);
    }

    public void Release()
    {
        mLisBehaviorStates.Clear();
    }

    public void Reset()
    {
        ResetWait();
        mLisBehaviorStates.Clear();
    }

    public void ResetWait()
    {
        mbIsWait = false;
    }

    /// <summary>
    /// 设置等待状态
    /// </summary>
    /// <param name="waitState">等待执行状态</param>
    /// <param name="waitTime">状态执行时间</param>
    /// <param name="isWaitLastState">是否等待执行上一个等待状态</param>
    public void SetWait(int waitBehavior, float waitTime, bool isWaitLastState)
    {
        if (!mbIsWait || !isWaitLastState)
        {
            mnWaitBehavior = waitBehavior;
            mfWaitTime = waitTime;
            mfWaitStartTime = Time.time;
            mbIsWait = true;
        }
    }

    public void Switch2(int switchState, bool isNotReset)
    {
        BehaviorState behavior = GetBehavior(switchState);
        if (behavior != null)
        {
            ResetWait();
            if (behavior == mCurrentBehavior)
            {
                if (!isNotReset)
                {
                    mCurrentBehavior.End(mRole, mCurrentBehavior);
                    mCurrentBehavior.Start(mRole, mCurrentBehavior);
                }
            }
            else
            {
                if (mCurrentBehavior != null)
                {
                    mCurrentBehavior.End(mRole, behavior);
                }

                BehaviorState lastBehavior = mCurrentBehavior;
                mCurrentBehavior = behavior;
                mCurrentBehavior.Start(mRole, lastBehavior);
            }
        }
    }

    public void UpdateBehaviors()
    {
        if (mCurrentBehavior == null)
        {
            if (mStartBehavior != null)
            {
                mCurrentBehavior = mStartBehavior;
                mCurrentBehavior.Start(mRole, null);
            }
        }
        else
        {
            int nBehavior = mCurrentBehavior.Update(mRole);
            nBehavior = CheckFSMCurBehavior(nBehavior);
            if (nBehavior != mCurrentBehavior.mnBehavior)
            {
                if (mCurrentBehavior == mOverrideBehavior)
                {
                    mOverrideBehavior = null;
                }

                if (nBehavior == -1 && (mStartBehavior != null))
                {
                    nBehavior = mStartBehavior.mnBehavior;
                }

                BehaviorState behavior = GetBehavior(nBehavior);
                if (behavior == null)
                {
                    behavior = mStartBehavior;
                }

                if (behavior != null)
                {
                    Switch2(behavior.mnBehavior, true);
                }
                else
                {
                    mCurrentBehavior = null;
                }
            }
        }
    }

    private int CheckFSMCurBehavior(int nCurBehavior)
    {
        if (!mbIsWait || (Time.time - mfWaitStartTime) < mfWaitTime)
        {
            return nCurBehavior;
        }

        ResetWait();
        return mnWaitBehavior;
    }
}
