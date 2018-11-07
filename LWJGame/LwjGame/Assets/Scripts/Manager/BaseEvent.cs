using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseEvent
{
    public Dictionary<int, GameEventDelegate> mDicEvent = new Dictionary<int, GameEventDelegate>();

    public void Reg(int eventId, GameEventDelegate.CallBack callBack)
    {
        GameEventDelegate evt = null;
        if (!mDicEvent.ContainsKey(eventId))
        {
            evt = new GameEventDelegate(eventId);
            mDicEvent.Add(eventId, evt);
        }
        else
        {
            evt = mDicEvent[eventId];
        }

        if(evt != null) evt.AddCallBack(callBack);
    }

    public void UnReg(int eventId, GameEventDelegate.CallBack callBack)
    {
        if (mDicEvent.ContainsKey(eventId))
        {
            GameEventDelegate evt = mDicEvent[eventId];
            evt.RemoveCallBack(callBack);
        }
    }

    public void UnReg(int eventId)
    {
        if (mDicEvent.ContainsKey(eventId))
        {
            mDicEvent.Remove(eventId);
        }
    }
}

public class GameEventDelegate
{
    public delegate void CallBack(int eventId, params object[] datas);

    List<CallBack> callBackLis = new List<CallBack>();
    List<CallBack> processLis = new List<CallBack>();
    private int nEventId;

    public GameEventDelegate(int eID)
    {
        nEventId = eID;
    }

    public void AddCallBack(CallBack cb)
    {
        if (!callBackLis.Contains(cb))
        {
            callBackLis.Add(cb);
        }
        else
        {
            if (Debug.developerConsoleVisible) Debug.LogError("AddCallBack Same");
        }
    }

    public void RemoveCallBack(CallBack cb)
    {
        callBackLis.Remove(cb);
    }

    public void ProceEvent(params object[] data)
    {
        processLis.AddRange(callBackLis);
        foreach (var cb in processLis)
        {
            cb(nEventId, data);
        }

        processLis.Clear();
    }
}
