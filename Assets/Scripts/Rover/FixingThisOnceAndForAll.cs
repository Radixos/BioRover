using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoverController : MonoBehaviour
{
    [SerializeField] private float roverSpeed;
    public float roverEnergy;
    private Rigidbody roverRigidbody;

    [SerializeField] private Canvas UICanvas;
    public GameObject lightCaustics;
    public float moveX, moveZ;

    public Transform minimapIcon;
    public Transform cameraTransform;

    void Start()
    {
        roverSpeed = 750.0f;
        roverRigidbody = GetComponent<Rigidbody>();    
    }

    void FixedUpdate()
    {
        //Vector3 toMove = Vector3.MoveTowards(transform.position, lightCaustics.GetComponent<LightManager>().hitPos, 1.0f);
        //roverRigidbody.AddTorque(toMove / 2000);
        minimapIcon.position = transform.position;

        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(moveX, 0f, moveZ).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.z, -direction.x) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            Vector3 force = moveDir.normalized * roverSpeed * (roverEnergy / 100) * Time.deltaTime;
            roverRigidbody.AddTorque(force);
        }
    }
}