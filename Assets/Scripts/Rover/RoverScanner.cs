using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Scanner;

public class RoverScanner : MonoBehaviour
{
<<<<<<< refs/remotes/origin/main
    private float coolDownTimer;
    private float coolDownDelay;

    private bool isScanning;
    public Image scanID;
    public GameObject laserSource;
    private float targetAngle; // Angle to stop the scan

    private RoverController playerCont;

    private ObjectPool minimapIconPool;
    private List<GameObject> scannedObjects = new List<GameObject>();
=======
    private LayerMask minimapLayer;
    private float coolDownTimer;
    private float coolDownDelay;

    private float scanRadius;
    private const float maxScanRadius = 50.0f;
    private bool isScanning;

    public Image scanID;
    public Transform scanEffectMinimap;

    public ScannerCameraEffect scanEffect;
    public float m_ScanVelocity = 15f;
    [Range(1f, 20f)] public float m_ScanWidth = 10f;
    public Color m_Leading;
    public Color m_Middle;
    public Color m_Trail;
    public Color m_HorizontalBar;

    private RoverController playerCont;

    // INTERACTING WITH POI

>>>>>>> levels

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< refs/remotes/origin/main
=======
        minimapLayer = 1 << 8;

>>>>>>> levels
        coolDownDelay = 3.0f;
        coolDownTimer = coolDownDelay;

        GraphicsSettings.renderPipelineAsset = null;

        isScanning = false;

<<<<<<< refs/remotes/origin/main
        playerCont = FindObjectOfType<RoverController>();

        minimapIconPool = GameObject.Find("Minimap Icon Pool").GetComponent<ObjectPool>();
=======
        scanEffect.m_Material.SetFloat("_ScanWidth", m_ScanWidth);
        scanEffect.m_Material.SetColor("_LeadColor", m_Leading);
        scanEffect.m_Material.SetColor("_MidColor", m_Middle);
        scanEffect.m_Material.SetColor("_TrailColor", m_Trail);
        scanEffect.m_Material.SetColor("_HBarColor", m_HorizontalBar);

        playerCont = GetComponent<RoverController>();
>>>>>>> levels
    }

    // Update is called once per frame
    void Update()
    {
        if (coolDownTimer >= coolDownDelay)
        {
            if (Input.GetKeyDown(KeyCode.Q) && !isScanning)
            {
                isScanning = true;
<<<<<<< refs/remotes/origin/main

                laserSource.SetActive(true);
                coolDownTimer = 0.0f;

                targetAngle = Mathf.RoundToInt(transform.eulerAngles.y - 1);

                if (targetAngle < 0)
                    targetAngle = 359;

                playerCont.roverEnergy -= 5;
=======
                coolDownTimer = 0.0f;
                scanEffectMinimap.gameObject.SetActive(true);
                playerCont.roverEnergy -= 5;

                scanEffect.m_Origin = transform.position;
>>>>>>> levels
            }
        }
        else if (!isScanning)
            coolDownTimer += Time.deltaTime;

        scanID.fillAmount = coolDownTimer / coolDownDelay;

<<<<<<< refs/remotes/origin/main
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
=======
    }

    void FixedUpdate()
    {
        if (isScanning)
        {
            // POI = Points Of Interest
            Collider[] POI = Physics.OverlapSphere(transform.position, scanRadius, minimapLayer, QueryTriggerInteraction.Collide);

            if (POI.Length > 0)
            {
                for (int i = 0; i < POI.Length; i++)
                    POI[i].GetComponent<MinimapIconController>().ActivateIcon();
            }

            scanRadius += m_ScanVelocity * Time.deltaTime;

            if (scanRadius >= maxScanRadius)
            {
                coolDownTimer = 0f;
                scanRadius = 0f;
                isScanning = false;
                scanEffectMinimap.gameObject.SetActive(false);
            }

            scanEffect.SetScanDistance(scanRadius);
            ParticleSystem.ShapeModule particleShape = scanEffectMinimap.GetComponent<ParticleSystem>().shape;
            particleShape.radius = scanRadius;

        }

        scanEffectMinimap.position = transform.position;

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.gameObject.CompareTag("POI"))
    //    {
    //        inRangeOfPOI = true;
    //    }
    //}


}


>>>>>>> levels
