using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Diagnostics;


public class BubblesJump : MonoBehaviour
{
    FMOD.Studio.EventInstance Jumpsound;
    [EventRef]
    public string eventName;

    private void Start()
    {
        Jumpsound = RuntimeManager.CreateInstance(eventName);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jumpsound.start();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Jumpsound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            Jumpsound.release();
        }

    }


}