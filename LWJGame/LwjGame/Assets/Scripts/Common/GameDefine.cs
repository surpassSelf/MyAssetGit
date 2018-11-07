using UnityEngine;
using System.Collections;

public enum ClientMsgId
{
    None,
    TestOne,
    ResetCallBack,
}

public enum ControlState
{
    Idle,
    JoyStick,
    Mouse,
    Key,
}

public enum CSMotion
{
    Static = 0,
    Stand = 1,
    Walk = 2,
    Attack = 3,
    Attack2 = 4,
    Attack3 = 5,
    BeAttack = 6,
    Dead = 7,
    Mining = 8,
    ShowStand = 9,
    Run = 10,
    WaKuang = 11,
    GuWu = 12,
    RunOverDoSomething = 13,
}
