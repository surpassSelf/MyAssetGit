using UnityEngine;
using System.Collections;

public class LookAtPoint : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    public Vector3 targetPoint;

    [HideInInspector]
    [SerializeField]
    public float radius;

    private void Start()
    {

    }
}
