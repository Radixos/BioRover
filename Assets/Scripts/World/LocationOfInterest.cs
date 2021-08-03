using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationOfInterest : MonoBehaviour
{
    public bool playerAtLocation = false;

    private BoxCollider boxCollider;
    public GameObject triggerBoxVisual;

    //// Variables for following objectives
    //private bool isActive;

    //public ObjectiveManager objectiveManager;
    //public int objectiveOrder; // Order in the objectives list

    //public MissionManager missionManager;
    //public int missionOrder; // Order in the missions list

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();

        //if (objec)
        //    SetKeyValues(true);
        //else
        //    SetKeyValues(false);

    }

    //private void Update()
    //{
    //    if(!playerAtLocation)
    //    {
    //        if (!isActive)
    //            if (objectiveOrder == objectiveManager.currentObjective)
    //                SetKeyValues(true);
    //    }

    //}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerAtLocation = true;
            triggerBoxVisual.SetActive(false);
            boxCollider.enabled = false;
        }
    }

    //void SetKeyValues(bool state)
    //{
    //    triggerBoxVisual.SetActive(state);
    //    boxCollider.enabled = state;
    //    isActive = state;
    //}
}
