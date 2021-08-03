using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Scanner;

public class RoverScanner : MonoBehaviour
{
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


    // Start is called before the first frame update
    void Start()
    {
        minimapLayer = 1 << 8;

        coolDownDelay = 3.0f;
        coolDownTimer = coolDownDelay;

        GraphicsSettings.renderPipelineAsset = null;

        isScanning = false;

        scanEffect.m_Material.SetFloat("_ScanWidth", m_ScanWidth);
        scanEffect.m_Material.SetColor("_LeadColor", m_Leading);
        scanEffect.m_Material.SetColor("_MidColor", m_Middle);
        scanEffect.m_Material.SetColor("_TrailColor", m_Trail);
        scanEffect.m_Material.SetColor("_HBarColor", m_HorizontalBar);

        playerCont = GetComponent<RoverController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (coolDownTimer >= coolDownDelay)
        {
            if (Input.GetKeyDown(KeyCode.Q) && !isScanning)
            {
                isScanning = true;
                coolDownTimer = 0.0f;
                scanEffectMinimap.gameObject.SetActive(true);
                playerCont.roverEnergy -= 5;

                scanEffect.m_Origin = transform.position;
            }
        }
        else if (!isScanning)
            coolDownTimer += Time.deltaTime;

        scanID.fillAmount = coolDownTimer / coolDownDelay;

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


