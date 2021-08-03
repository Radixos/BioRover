//----------------------------------------------------------------------------------------------------
//Please note: Colliders matter when dropping a rover in a water! (same height as water surface level)
//----------------------------------------------------------------------------------------------------
//Attach to rover that needs to be dropped at random location
//----------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDrop : MonoBehaviour
{
    //[SerializeField] private GameObject dropObject;

    [SerializeField] private int xRange;
    [SerializeField] private int zRange;
    [SerializeField] private GameObject waterSurface;

    void Start()
    {
        Debug.Log(waterSurface.transform.position.y);
        this.transform.position = new Vector3(Random.Range(-xRange, xRange), waterSurface.transform.position.y, Random.Range(-zRange, zRange));
    }
}