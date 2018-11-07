using UnityEngine;
using System.Collections;

public class BaseManager {

    public bool IsOnUpdate = false;
    public bool IsFixedUpdate = false;
    public bool IsLateUpdate = false;

    public virtual void Init() { }
    public virtual void OnFixedUpdate(float deltaTime) { }
    public virtual void OnUpdate(float deltaTime) { }
    public virtual void OnDispose() { }
}
