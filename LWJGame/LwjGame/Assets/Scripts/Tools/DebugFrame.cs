using UnityEngine;
using System.Collections;

public class DebugFrame : MonoBehaviour {

    private float nLastTimeInterval = 0;
    private float nTimeInterval = 1f;
    private float fFps = 0;

	void Start () {
        nLastTimeInterval = Time.realtimeSinceStartup;
    }

    void OnGUI()
    {
        GUIStyle sty = new GUIStyle();
        sty.fontSize = 20;
        sty.fontStyle = FontStyle.Bold;
        sty.normal.textColor = Color.green;

        string sFps = "FPS: " + (int)Mathf.Abs(fFps);
        string sTime = "Time: " + Time.realtimeSinceStartup;
        //GUILayout.Label(sFps.ToString(), sty, GUILayout.Height(50), GUILayout.Width(50));
        GUI.Label(new Rect(0, 0, 50, 30), sFps, sty);
        GUI.Label(new Rect(0, 20, 50, 30), sTime, sty);
    }

    float fFrames = 0;
	void Update ()
    {
        ++fFrames;

        float timeNow = Time.realtimeSinceStartup;
        if (timeNow >= nLastTimeInterval + nTimeInterval)
        {
            fFps = fFrames / (nLastTimeInterval - timeNow);

            fFrames = 0;
            nLastTimeInterval = timeNow;
        }
	}
}
