using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        Debug.Log( Quaternion.Angle(transform.rotation, target.rotation));
    }
}
