using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBarScript : MonoBehaviour
{
    private Slider energySlider;
    private GameObject player;

    void Start()
    {
        energySlider = gameObject.GetComponent<Slider>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        energySlider.value = player.GetComponent<RoverController>().roverEnergy;
    }
}