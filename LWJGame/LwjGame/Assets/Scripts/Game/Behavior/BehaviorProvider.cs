using UnityEngine;
using System.Collections;

public abstract class BehaviorProvider
{
    protected BehaviorProvider() { }
    public abstract bool InitializeFSM(FSMState fsmState);
    public abstract void Reset();
}
