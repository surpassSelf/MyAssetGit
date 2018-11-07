using UnityEngine;
using System.Collections;

public class UIBase : BaseGameObj
{
    protected GameEventManager mClientEvent = new GameEventManager(GameEventManager.DispactchType.Client);
    protected GameEventManager mSocketEvent = new GameEventManager(GameEventManager.DispactchType.SockeEvent);

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    protected virtual void Show()
    {

    }
}
