using UnityEngine;
using System.Collections;

public class UITest : UIBase {

    protected override void OnAwake()
    {
        base.OnAwake();
        OutDebug.Init();
    }

    protected override void OnStart()
    {
        base.OnStart();

        mClientEvent.Reg(ClientMsgId.TestOne, MsgCallBack);
        mClientEvent.Reg(ClientMsgId.TestOne, MsgCallBack2);
        mClientEvent.Reg(ClientMsgId.ResetCallBack, UnRegCallBack);
    }

    private void MsgCallBack(int eventID, params object[] objs)
    {
        if (objs.Length > 0) Debug.LogError(objs[0].ToString());
    }

    private void MsgCallBack2(int eventID, params object[] objs)
    {
        if (objs.Length > 0) Debug.LogError(objs[0].ToString() + "2");
    }

    private void UnRegCallBack(int eventID, params object[] objs)
    {
        mClientEvent.UnReg(ClientMsgId.TestOne);
    }
}
