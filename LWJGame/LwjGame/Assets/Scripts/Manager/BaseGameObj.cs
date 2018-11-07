using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseGameObj : MonoBehaviour {

    protected Dictionary<string, BaseCtrl> Ctrls = new Dictionary<string, BaseCtrl>(); //控制器集合
    protected Dictionary<string, BaseManager> Managers = new Dictionary<string, BaseManager>(); //管理器集合

    /// <summary>
    /// 初始化
    /// </summary>
    void Awake()
    {
        OnAwake();
    }

    void Start()
    {
        OnStart();
    }

    /// <summary>
    /// 固定帧更新
    /// </summary>
    void FixedUpdate()
    {
        OnFixedUpdate();
    }

    /// <summary>
    /// 每一帧更新
    /// </summary>
    void Update()
    {
        OnUpdate();
    }

    void LateUpdate()
    {
        OnLateUpdate();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        OnTriggerCollide2D(coll);
    }

    void OnDestroy()
    {
        OnDestroyMe();
    }

    protected virtual void OnStart() { }
    protected virtual void OnLateUpdate() { }
    protected virtual void OnTriggerCollide2D(Collider2D coll) { }

    protected virtual void OnAwake()
    {
        foreach (var ctrl in Ctrls)
        {
            if (ctrl.Value != null)
            {
                ctrl.Value.Init();
            }
        }

        foreach (var mgr in Managers)
        {
            if (mgr.Value != null)
            {
                mgr.Value.Init();
            }
        }
    }
    
    protected virtual void OnFixedUpdate()
    {
        foreach (var ctrl in Ctrls)
        {
            if (ctrl.Value != null && ctrl.Value.IsFixedUpdate)
            {
                ctrl.Value.OnFixedUpdate(Time.deltaTime);
            }
        }

        foreach (var mgr in Managers)
        {
            if (mgr.Value != null && mgr.Value.IsFixedUpdate)
            {
                mgr.Value.OnFixedUpdate(Time.deltaTime);
            }
        }
    }

    protected virtual void OnUpdate()
    {
        foreach (var ctrl in Ctrls)
        {
            if (ctrl.Value != null && ctrl.Value.IsOnUpdate)
            {
                ctrl.Value.OnUpdate(Time.deltaTime);
            }
        }

        foreach (var mgr in Managers)
        {
            if (mgr.Value != null && mgr.Value.IsOnUpdate)
            {
                mgr.Value.OnUpdate(Time.deltaTime);
            }
        }
    }
    
    protected virtual void OnDestroyMe()
    {
        foreach (var ctrl in Ctrls)
        {
            if (ctrl.Value != null)
            {
                ctrl.Value.OnDispose();
            }
        }

        foreach (var mgr in Managers)
        {
            if (mgr.Value != null)
            {
                mgr.Value.OnDispose();
            }
        }

        Managers.Clear();
        Ctrls.Clear();
    }

    public T AddManager<T>() where T : BaseManager, new()
    {
        var type = typeof(T);
        var mgr = new T();
        Managers.Add(type.Name, mgr);

        return Managers[type.Name] as T;
    }

    public T GetManager<T>() where T : BaseManager
    {
        var type = typeof(T);
        if (!Managers.ContainsKey(type.Name))
        {
            return null;
        }

        return Managers[type.Name] as T;
    }

    public T AddCtrl<T>() where T : BaseCtrl, new()
    {
        var type = typeof(T);
        var ctrl = new T();

        Ctrls.Add(type.Name, ctrl);
        return Ctrls[type.Name] as T;
    }

    public T GetCtrl<T>() where T : BaseCtrl
    {
        var type = typeof(T);

        if (!Ctrls.ContainsKey(type.Name))
        {
            return null;
        }

        return Ctrls[type.Name] as T;
    }
}
