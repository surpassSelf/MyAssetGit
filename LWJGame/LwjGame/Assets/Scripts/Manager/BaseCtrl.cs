using UnityEngine;
using System.Collections;

public abstract class BaseCtrl {

    public bool IsOnUpdate = false;
    public bool IsFixedUpdate = false;
    public bool IsLateUpdate = false;

    public abstract void Init();
    public abstract void OnFixedUpdate(float deltaTime);
    public abstract void OnUpdate(float deltaTime);
    public abstract void OnDispose();
}
