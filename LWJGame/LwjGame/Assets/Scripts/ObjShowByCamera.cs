using UnityEngine;
using System.Collections;

public class ObjShowByCamera : MonoBehaviour {

    private void OnBecameVisible()
    {
        Debug.LogError("Obj Became Visible~~~~!");
    }

    private void OnBecameInvisible()
    {

        Debug.LogError("Obj Became InVisible~~~~!");
    }
}
