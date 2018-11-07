using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public class MyEditorExcuteScript : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
        Debug.Log("Hi ~~~~~~~~~~~~!");
        StartCoroutine(Say());
    }

    IEnumerator Say()
    {
        while (true)
        {
            Debug.Log("What Fuck!");
            yield return null;
        }
    }
	
	// Update is called once per frame
	void Update () 
	{
        
	}
	
	//Script OnDestory Call This
	void Destory()
	{
	
	}
}
