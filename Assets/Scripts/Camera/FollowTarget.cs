using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public GameObject target;

    public bool LookAtTarget;

    public float xoffset, yoffset, zoffset;

    void Update()
    {
        transform.position = target.transform.position + new Vector3(xoffset, yoffset, zoffset);

        if (LookAtTarget)
            transform.LookAt(target.transform.position);
    }
}
