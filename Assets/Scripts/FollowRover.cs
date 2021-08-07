using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRover : MonoBehaviour
{
    [SerializeField] private GameObject rover;

    void Update()
    {
        this.transform.position = rover.transform.position;
    }
}
