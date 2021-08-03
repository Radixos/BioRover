using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OxygenBar : MonoBehaviour
{
    public Slider OxygenBar;

    private int maxOxygen = 100;
    private int currentOxygen;

    public static OxygenBar instance;



    private void Awake()
    {
        instance = this;

    }

    void Start()
    {
        currentOxygen = maxOxygen;
        OxygenBar.maxValue = maxOxygen;
        OxygenBar

    }

