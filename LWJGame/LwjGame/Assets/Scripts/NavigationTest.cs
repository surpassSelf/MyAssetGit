using UnityEngine;
using System.Collections;

public class NavigationTest : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
        GameObject targetObj = GameObject.Find("target");
        if (targetObj != null)
        {
            GetComponent<NavMeshAgent>().destination = targetObj.transform.position;
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
