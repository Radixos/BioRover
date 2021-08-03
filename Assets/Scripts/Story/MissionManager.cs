using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public List<Mission> storyMissions;
    public int curentStoryMission = 0;
    public bool storyMissionsComplete = false;

    private float nextMissionDelay;
    private float nextMissionTimer;
   
    // Start is called before the first frame update
    void Start()
    {
        nextMissionDelay = 1.5f;
        nextMissionTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        StoryMissions();
    }

    void StoryMissions()
    {
        if (storyMissions[curentStoryMission].missionCompleted)
        {
            if (nextMissionTimer >= nextMissionDelay)
            {
                storyMissions[curentStoryMission].gameObject.SetActive(false);

                if (curentStoryMission + 1 < storyMissions.Count)
                {
                    ++curentStoryMission;
                    storyMissions[curentStoryMission].gameObject.SetActive(true);
                }
                else
                    storyMissionsComplete = true;
            }
            else
                nextMissionTimer += Time.deltaTime;

        }
    }
}
