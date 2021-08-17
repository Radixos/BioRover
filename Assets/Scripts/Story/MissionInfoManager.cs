using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionInfoManager : MonoBehaviour
{
    public List<Mission> allMissions;

    public Text missionDisplay;
    public Text objectiveDisplay;

    private int trackingMission = 0;

    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            if (trackingMission + 1 >= allMissions.Count)
                trackingMission = 0;
            else
                trackingMission++;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (trackingMission - 1 < 0)
                trackingMission = allMissions.Count - 1;
            else
                trackingMission--;
        }

        // Displaying Mission Info
        missionDisplay.text = allMissions[trackingMission].GetMissionInfo();
        missionDisplay.color = allMissions[trackingMission].GetMissionInfoColor();

        // Displaying Objective Info
        objectiveDisplay.text = allMissions[trackingMission].objectiveSet.
            objectives[allMissions[trackingMission].objectiveSet.currentObjective].GetObjectiveInfo();
        objectiveDisplay.color = allMissions[trackingMission].objectiveSet.
            objectives[allMissions[trackingMission].objectiveSet.currentObjective].GetObjectiveInfoColor();

    }
}
