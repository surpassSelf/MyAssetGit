using UnityEngine;
using System.Collections;

public class UIJoystickPanel : UIBase {

    private GameObject goDragArea;
    private GameObject mGoDragArea { get { return goDragArea ?? (goDragArea = transform.Find("zone").gameObject); } }

    private GameObject goTouchPoint;
    private GameObject mGoTouchPoint { get { return goTouchPoint ?? (goTouchPoint = transform.Find("joystick/touch").gameObject); } }

    private GameObject goArea;
    private GameObject mGoArea { get { return goArea ?? (goArea = transform.Find("joystick/area").gameObject); } }

    private GameObject goJoystick;
    private GameObject mGoJoystick { get { return goJoystick ?? (goJoystick = transform.Find("joystick").gameObject); } }

    private int mnAreaRadius = 0;
    private Vector3 mMouseLocalPos = Vector3.zero;

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override void OnStart()
    {
        base.OnStart();

        UIEventListener.Get(mGoDragArea).onDrag = OnDragTouch;
        UIEventListener.Get(mGoDragArea).onPress = OnPressTouch;
        mnAreaRadius = mGoArea.GetComponent<UIWidget>().height / 2;
    }

    private void OnDragTouch(GameObject go, Vector2 delta)
    {
        mMouseLocalPos += (Vector3)delta;
        SetTouchPos(mMouseLocalPos);
    }

    private void OnPressTouch(GameObject go, bool state)
    {
        if (state)
        {
            OnPressTouch(state, (Vector3)UICamera.currentTouch.pos - new Vector3(Screen.width / 2, Screen.height / 2, 0));
        }
        else
        {
            OnPressTouch(state);
        }
    }

    private void OnPressTouch(bool state, Vector3 pos = default(Vector3))
    {
        if (state)
        {
            mMouseLocalPos = pos - mGoJoystick.transform.localPosition;
            if (mMouseLocalPos.sqrMagnitude > 40000)
            {
                return;
            }
            else if(mMouseLocalPos.sqrMagnitude > 1600)
            {
                SetTouchPos(mMouseLocalPos);
            }
        }
        else
        {
            mMouseLocalPos = Vector3.zero;
            mGoTouchPoint.transform.localPosition = Vector3.zero;
        }
    }

    private void SetTouchPos(Vector3 mouseLocalPos)
    {
        if (mouseLocalPos.magnitude > mnAreaRadius) mGoTouchPoint.transform.localPosition = mouseLocalPos.normalized * mnAreaRadius;
        else mGoTouchPoint.transform.localPosition = mouseLocalPos;
    }
}
