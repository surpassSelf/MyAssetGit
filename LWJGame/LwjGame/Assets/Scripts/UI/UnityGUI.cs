using UnityEngine;
using System.Collections;

public class UnityGUI : MonoBehaviour {

    public GameEventManager mClientEvent = new GameEventManager(GameEventManager.DispactchType.Client);
    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 50), "OnClick One"))
        {
            mClientEvent.SendMsg(ClientMsgId.TestOne, "My Event Msg");
        }

        if (GUI.Button(new Rect(0, 100, 100, 50), "OnClick Two"))
        {
            mClientEvent.SendMsg(ClientMsgId.ResetCallBack);
        }

        if (GUI.Button(new Rect(0, 200, 100, 50), "OnClick Three"))
        {

        }

        if (GUI.Button(new Rect(0, 300, 100, 50), "OnClick Four"))
        {

        }

        if (GUI.Button(new Rect(0, 400, 100, 50), "OnClick Five"))
        {

        }
    }
}
