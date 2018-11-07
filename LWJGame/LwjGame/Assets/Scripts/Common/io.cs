using UnityEngine;
using System.Collections;
using System.IO;

public class io : CSSingleton<io> {

    private StreamWriter mWriter;
    private string mstrDocPath;

	public void WriteTxt(string sContent)
    {
#if UNITY_EDITOR
        mstrDocPath = Application.dataPath + "/../PersistentPath";
#elif UNITY_STANDALONE_WIN
                mstrDocPath = Application.dataPath + "/PersistentPath";
#elif UNITY_STANDALONE_OSX
                mstrDocPath = Application.dataPath + "/PersistentPath";
#else
                mstrDocPath = Application.persistentDataPath;
#endif
        mstrDocPath = "Log.txt";
        mWriter = new StreamWriter(mstrDocPath, true);
        mWriter.Write(sContent);
        mWriter.Close();
    }
}
