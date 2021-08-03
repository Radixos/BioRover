using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectOfInterest : MonoBehaviour
{
    public GameObject infoCanvas;
    private float maxScanValue;
    private float currentScanValue;

    public Image scanBarFill;

    private bool playerInRange;

    public bool isScanned;

    // To avoid unwanted interaction after scan completion
    public Collider triggerCollider;

    // Start is called before the first frame update
    void Start()
    {
        maxScanValue = 2.0f;
        currentScanValue = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isScanned)
        {
            if (playerInRange)
            {
                infoCanvas.transform.LookAt(new Vector3(Camera.main.transform.position.x,
                    infoCanvas.transform.position.y,
                    Camera.main.transform.position.z));

                if (currentScanValue < maxScanValue)
                {
                    if (Input.GetKey(KeyCode.E))
                    {
                        currentScanValue += Time.deltaTime;

                        if (currentScanValue >= maxScanValue)
                        {
                            isScanned = true;

                            playerInRange = false;
                            infoCanvas.SetActive(false);
                            triggerCollider.enabled = false;
                        }

                    }
                }

                scanBarFill.fillAmount = currentScanValue / maxScanValue;

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            infoCanvas.SetActive(true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            infoCanvas.SetActive(false);
        }

    }
}
