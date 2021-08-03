using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform player;
    public Transform mainCamera;
    public float heightY;

    void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = player.position.y + heightY;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, mainCamera.eulerAngles.y, 0f);
    }
}
