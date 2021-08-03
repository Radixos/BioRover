using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objective : MonoBehaviour
{
    public enum ObjectiveType { TRIGGER = 0, INTERACT = 1 }

    public ObjectiveType objectiveType;
    public bool isCompleted;

    // UI
    private Text objectiveNameDisplay;
    [Header("UI")]
    public string objectiveName;

    [Header("Trigger Type")]
    public LocationOfInterest triggerCollider;

    [Header("Interact Type")]
    public List<ObjectOfInterest> interactingObjects = new List<ObjectOfInterest>();

    // Start is called before the first frame update
    //void Start()
    //{
       // objectiveNameDisplay = GameObject.Find("Objective Name Display").GetComponent<Text>();
        //objectiveNameDisplay.color = Color.white;
    //}

    // Update is called once per frame
    void Update()
    {
        switch (objectiveType)
        {
            case ObjectiveType.TRIGGER:
                TriggerTypeLogic();
                break;
            case ObjectiveType.INTERACT:
                InteractTypeLogic();
                break;
        }
    }

    void TriggerTypeLogic()
    {
        if (triggerCollider.playerAtLocation)
        {
            isCompleted = true;
            objectiveNameDisplay.text = "Objective Completed!";
            objectiveNameDisplay.color = Color.green;
        }
        else
            objectiveNameDisplay.text = objectiveName;
    }

    void InteractTypeLogic()
    {
        int completedInteractingObjects = 0;

        for (int i = 0; i < interactingObjects.Count; i++)
        {
            if (interactingObjects[i].isScanned)
                completedInteractingObjects++;
        }

        if (completedInteractingObjects == interactingObjects.Count)
        {
            isCompleted = true;
            objectiveNameDisplay.text = "Objective Completed!";
            objectiveNameDisplay.color = Color.green;
        }
        else
            objectiveNameDisplay.text = objectiveName + ": " + completedInteractingObjects.ToString() + "/" + interactingObjects.Count.ToString();

    }

    public void DeactivateObjective()
    {
        SetObjectiveObjectState(false);
    }

    public void InitialiseObjective()
    {
        SetObjectiveObjectState(true);

        objectiveNameDisplay = GameObject.Find("Objective Name Display").GetComponent<Text>();
        objectiveNameDisplay.color = Color.white;
    }

    private void SetObjectiveObjectState(bool state)
    {
        switch (objectiveType)
        {
            case ObjectiveType.TRIGGER:
                triggerCollider.gameObject.SetActive(state);
                break;
            case ObjectiveType.INTERACT:
                for (int i = 0; i < interactingObjects.Count; i++)
                {
                    interactingObjects[i].enabled = state;
                    interactingObjects[i].triggerCollider.enabled = state;
                }

                break;
        }
    }
}
