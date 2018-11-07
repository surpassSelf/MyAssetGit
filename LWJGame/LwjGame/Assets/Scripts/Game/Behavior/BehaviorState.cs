using UnityEngine;
using System.Collections;

public class BehaviorState
{
    private int nBehavior;
    public int mnBehavior { get { return nBehavior; } }

    private string strStateName;
    public string mstrStateName { get { return strStateName; } }

    public delegate void DelStart(CSGameRoleBase role, BehaviorState lastState);
    public delegate void DelEnd(CSGameRoleBase role, BehaviorState nextState);
    public delegate int DelUpdate(CSGameRoleBase role);

    private DelStart mDelStart;
    private DelEnd mDelEnd;
    private DelUpdate mDelUpdate;

    public BehaviorState(int nState, DelStart delStart, DelEnd delEnd, DelUpdate delUpdate, string strStateName)
    {
        this.nBehavior = nState;
        mDelStart = delStart;
        mDelEnd = delEnd;
        mDelUpdate = delUpdate;
        this.strStateName = strStateName;
    }

    public void Start(CSGameRoleBase role, BehaviorState lastState)
    {
        if (mDelStart != null)
        {
            mDelStart(role, lastState);
        }
    }

    public void End(CSGameRoleBase role, BehaviorState nextState)
    {
        if (mDelEnd != null)
        {
            mDelEnd(role, nextState);
        }
    }

    public int Update(CSGameRoleBase role)
    {
        if (mDelUpdate != null)
        {
            return mDelUpdate(role);
        }
        
        return mnBehavior;
    }
}
