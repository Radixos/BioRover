using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoverController : MonoBehaviour
{
    [SerializeField] private float roverSpeed;
    public float roverEnergy;
    private Rigidbody roverRigidbody;

    public GameObject lightCaustics;
    public float moveX, moveZ;

    public Transform minimapIcon;
    public Transform cameraTransform;

    public LayerMask groundlayers;

    public float jumpForce = 10;

    private SphereCollider col;

    public GameObject Bubbles;
    private float BubbleTimer = 0.5f;



    void Start()
    {
        roverSpeed = 750.0f;
        roverRigidbody = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();

    }
    void FixedUpdate()
    {
        //Vector3 toMove = Vector3.MoveTowards(transform.position, lightCaustics.GetComponent<LightManager>().hitPos, 1.0f);
        //roverRigidbody.AddTorque(toMove / 2000);
        minimapIcon.position = transform.position;

        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(moveX, 0f, moveZ).normalized;

        Bubbles.transform.position = transform.position - Vector3.up;


        if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position, -Vector3.up, 2f))
        {
            roverRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Bubbles.SetActive(true);

        }


        if (Bubbles.activeSelf)

        {
            BubbleTimer -= Time.deltaTime;
            if (BubbleTimer <= 0)
            {
                Bubbles.SetActive(false);
                BubbleTimer = 0.5f;

            }
        }


        if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.z, -direction.x) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                Vector3 force = moveDir.normalized * roverSpeed * (roverEnergy / 100) * Time.deltaTime;
                roverRigidbody.AddTorque(force);




            }
       
    }


}