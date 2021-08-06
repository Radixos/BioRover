using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Scanner;

public class RoverScanner : MonoBehaviour
{
    private float coolDownTimer;
    private float coolDownDelay;

    private bool isScanning;
    public Image scanID;
    public GameObject laserSource;
    private float targetAngle; // Angle to stop the scan

    private RoverController playerCont;

    private ObjectPool minimapIconPool;
    private List<GameObject> scannedObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        coolDownDelay = 3.0f;
        coolDownTimer = coolDownDelay;

        GraphicsSettings.renderPipelineAsset = null;

        isScanning = false;

        playerCont = FindObjectOfType<RoverController>();

        minimapIconPool = GameObject.Find("Minimap Icon Pool").GetComponent<ObjectPool>();
    }

    // Update is called once per frame
    void Update()
    {
        if (coolDownTimer >= coolDownDelay)
        {
            if (Input.GetKeyDown(KeyCode.Q) && !isScanning)
            {
                isScanning = true;

                laserSource.SetActive(true);
                coolDownTimer = 0.0f;

                targetAngle = Mathf.RoundToInt(transform.eulerAngles.y - 1);

                if (targetAngle < 0)
                    targetAngle = 359;

                playerCont.roverEnergy -= 5;
            }
        }
        else if (!isScanning)
            coolDownTimer += Time.deltaTime;

        scanID.fillAmount = coolDownTimer / coolDownDelay;

        transform.position = playerCont.transform.position;

    }

    private void FixedUpdate()
    {
        if (isScanning) // Scanning device rotating around Bio-Rover when scan feature is activated
        {
            transform.Rotate(Vector3.up, 90 * Time.deltaTime);
            RaycastHit hit;

            // Checking if scanner has completed a 360 journey
            if (transform.eulerAngles.y >= targetAngle - 2.0f && transform.eulerAngles.y <= targetAngle)
            {
                isScanning = false;
                laserSource.SetActive(false);
                scannedObjects.Clear();
            }
            // Placing an icon at the object's position
            else if (Physics.Raycast(transform.position, transform.forward, out hit, 400.0f))
            {
                if (hit.collider.gameObject.name != "Terrain" &&
                    !scannedObjects.Contains(hit.collider.gameObject))
                {
                    Debug.Log(hit.collider.gameObject.name);

                    GameObject obj = minimapIconPool.GetPooledObject();

                    obj.transform.position = hit.collider.transform.position + Vector3.up * 5.0f;

                    // Object Differentiation
                    if (hit.collider.gameObject.name.Contains("MM_"))
                        obj.GetComponent<SpriteRenderer>().color = Color.yellow;
                    else if (hit.collider.gameObject.name.Contains("SM_"))
                        obj.GetComponent<SpriteRenderer>().color = Color.cyan;
                    else
                        obj.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);

                    obj.SetActive(true);
                    scannedObjects.Add(hit.collider.gameObject);
                }
            }
        }
        else // Scanner device facing camera direction
        {
            Vector3 lookVector = new Vector3(playerCont.cameraTransform.transform.forward.x,
                0, playerCont.cameraTransform.transform.forward.z);

            Quaternion lookRotation = Quaternion.LookRotation(lookVector);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 75 * Time.deltaTime);
        }
    }
}

