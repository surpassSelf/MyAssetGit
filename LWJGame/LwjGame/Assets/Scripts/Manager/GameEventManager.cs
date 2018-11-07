using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GameEventManager : BaseManager
{
    public enum DispactchType
    {
        Client,
        SockeEvent,
        RedPoint,
    }

    struct MsgCBStruct
    {
        public int eventID;
        public GameEventDelegate.CallBack CallBack;
    }

    private List<MsgCBStruct> mCBLis = new List<MsgCBStruct>();

    private static BaseEvent mClientEvent = new BaseEvent();
    private static BaseEvent mSocketEvent = new BaseEvent();
    private static BaseEvent mRedPointEvent = new BaseEvent();

    public BaseEvent mDispatchEvent;

    public GameEventManager(DispactchType type)
    {
        switch (type)
        {
            case DispactchType.Client:
                mDispatchEvent = mClientEvent;
                break;
            case DispactchType.SockeEvent:
                mDispatchEvent = mSocketEvent;
                break;
            case DispactchType.RedPoint:
                mDispatchEvent = mRedPointEvent;
                break;
        }
    }

    public void Reg(ClientMsgId msgID, GameEventDelegate.CallBack callBack)
    {
        Reg((int)msgID, callBack);
    }

    public void Reg(int eventID, GameEventDelegate.CallBack callBack)
    {
        foreach (var cb in mCBLis)
        {
            if (cb.eventID == eventID && cb.CallBack == callBack)
            {
                return;
            }
        }

        MsgCBStruct msg;
        msg.eventID = eventID;
        msg.CallBack = callBack;

        mCBLis.Add(msg);
        mDispatchEvent.Reg(eventID, callBack);
    }

    public void UnReg(ClientMsgId msgId, GameEventDelegate.CallBack callBack)
    {
        UnReg((int)msgId, callBack);
    }

    public void UnReg(ClientMsgId msgId)
    {
        UnReg((int)msgId);
    }

    public void UnReg(int eventID, GameEventDelegate.CallBack callBack)
    {
        for (int i = 0; i < mCBLis.Count; i++)
        {
            MsgCBStruct msg = mCBLis[i];
            if (msg.eventID == eventID && msg.CallBack == callBack)
            {
                mCBLis.RemoveAt(i);
                break;
            }
        }

        mDispatchEvent.UnReg(eventID, callBack);
    }

    public void UnReg(int eventID)
    {
        for (int i = mCBLis.Count - 1; i >= 0; i--)
        {
            MsgCBStruct msg = mCBLis[i];
            if (msg.eventID == eventID)
            {
                mCBLis.RemoveAt(i);
            }
        }

        mDispatchEvent.UnReg(eventID);
    }

    public void UnRegAll()
    {
        for (int i = 0; i < mCBLis.Count; i++)
        {
            MsgCBStruct msg = mCBLis[i];
            mDispatchEvent.UnReg(msg.eventID, msg.CallBack);
        }

        mCBLis.Clear();
    }

    public void SendMsg(ClientMsgId msgId, params object[] objs)
    {
        SendMsg((int)msgId, objs);
    }

    private void SendMsg(int eventId, params object[] objs)
    {
        if (mDispatchEvent.mDicEvent.ContainsKey(eventId))
        {
            mDispatchEvent.mDicEvent[eventId].ProceEvent(objs);
        }
    }

    public override void OnDispose()
    {
        base.OnDispose();

        mCBLis.Clear();
        mClientEvent = null;
        mSocketEvent = null;
        mRedPointEvent = null;
    }
}
