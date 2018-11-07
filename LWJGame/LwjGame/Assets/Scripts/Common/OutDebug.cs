using UnityEngine;
using System.Collections;
using System.Text;
using System.Threading;

public class OutDebug : MonoBehaviour
{
    private static object _lock = new object(); //要保证全局只有一个，锁线程的时候需要的一个标识符
    private static int mCurrThreadId = -1;

    private static OutDebug Instance;
    public static OutDebug mInstance { get { return Instance; } }
    
    public static OutDebug Init()
    {
        lock (_lock)
        {
            if (Instance == null)
            {
                GameObject outDebug = GameObject.Find("OutDebug");
                if (outDebug == null)
                {
                    outDebug = new GameObject("OutDebug");
                    GameObject.DontDestroyOnLoad(outDebug);
                    Instance = outDebug.AddComponent<OutDebug>();
                }
                else
                {
                    Instance = outDebug.GetComponent<OutDebug>();
                }

                mCurrThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            return Instance;
        }
    }

    private void Start()
    {
        Application.logMessageReceived -= DebugLogCallBack;
        Application.logMessageReceived += DebugLogCallBack;

        Application.logMessageReceivedThreaded -= MultiThreadDebugLogCallBack;
        Application.logMessageReceivedThreaded += MultiThreadDebugLogCallBack;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= DebugLogCallBack;
        Application.logMessageReceivedThreaded -= MultiThreadDebugLogCallBack;
    }

    private void DebugLogCallBack(string logStr, string stackTrace, LogType type)
    {
        if(mCurrThreadId == Thread.CurrentThread.ManagedThreadId)
            OutLogStr(logStr, stackTrace, type);
    }

    private void MultiThreadDebugLogCallBack(string logStr, string stackTrace, LogType type)
    {
        if (mCurrThreadId != Thread.CurrentThread.ManagedThreadId)
            OutLogStr(logStr, stackTrace, type);
    }

    private void OutLogStr(string logStr, string stackTrace, LogType type)
    {
        string strTitle = "";
        switch (type)
        {
            case LogType.Error:
                strTitle = "[Error]";
                break;
            case LogType.Assert:
                strTitle = "[Assert]";
                break;
            case LogType.Warning:
                strTitle = "[Warning]";
                break;
            case LogType.Log:
                strTitle = "[Log]";
                break;
            case LogType.Exception:
                strTitle = "[Exception]";
                break;
        }

        StringBuilder strBuilder = new StringBuilder();
        strBuilder.Append(strTitle + "\r\n");
        strBuilder.Append("Log：" + logStr + "\r\n");
        strBuilder.Append(stackTrace + "\r\n");

        io.mInstance.WriteTxt(strBuilder.ToString());
    }
}


