using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public List<Objective> objectives = new List<Objective>();
    public int currentObjective = 0;
    public bool allObjectivesCompleted = false;

    private float nextObjectiveDelay;
    private float nextObjectiveTimer;

    // Start is called before the first frame update
    void Start()
    {
        nextObjectiveDelay = 1.5f;
        nextObjectiveTimer = 0.0f;

        objectives[currentObjective].InitialiseObjective();
    }

    // Update is called once per frame
    void Update()
    {
        if(objectives[currentObjective].isCompleted)
        {
            if (nextObjectiveTimer >= nextObjectiveDelay)
            {
                objectives[currentObjective].DeactivateObjective();
                objectives[currentObjective].gameObject.SetActive(false);

                if (currentObjective + 1 < objectives.Count)
                {
                    ++currentObjective;
                    objectives[currentObjective].gameObject.SetActive(true);
                    objectives[currentObjective].InitialiseObjective();
                }
                else
                    allObjectivesCompleted = true;
            }
            else
                nextObjectiveTimer += Time.deltaTime;

        }
    }

}
