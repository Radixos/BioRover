using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoverEnergy : MonoBehaviour
{
    private RoverController playerController;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private RawImage visionImage;
    public float alphaFloat;
    void Start()
    {
        playerController = this.gameObject.GetComponent<RoverController>();
        playerController.roverEnergy = 100;
        alphaFloat = 0;
        visionImage.color = new Color(0, 0, 0, alphaFloat);
    }

    void FixedUpdate()
    {
        if (playerController.roverEnergy > 0 && (playerController.moveX != 0 
            || playerController.moveZ != 0))
        {
            playerController.roverEnergy -= Time.deltaTime;          
        }

        if (playerController.lightCaustics.GetComponent<LightManager>().hasHit == true
            && playerController.roverEnergy < 100 && (playerController.moveX == 0 && 
            playerController.moveZ == 0))
        {
            playerController.roverEnergy += Time.deltaTime;
        }

        alphaFloat = (100 - playerController.roverEnergy) * 0.009f;
        visionImage.color = new Color(0, 0, 0, alphaFloat);
        energyText.text = playerController.roverEnergy.ToString();
    }   
}