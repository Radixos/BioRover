using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rovercontroller : MonoBehaviour
{
    [SerializeField]
    private float roverSpeed;
    private float speedX, speedY;
    private Rigidbody roverRigidbody;

    void Start()
    {
        roverSpeed = 750.0f;
        roverRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        speedX = Input.GetAxis("Horizontal");
        speedY = Input.GetAxis("Vertical");
        roverRigidbody.AddTorque(new Vector3(speedX, 0, speedY) * roverSpeed * Time.deltaTime);
    }
} 