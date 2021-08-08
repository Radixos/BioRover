using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System;

public class MusicIntensity : MonoBehaviour
{
    StudioEventEmitter MusicTense;



    // Start is called before the first frame update
        StudioEventEmitter MusicEmitter;
   
        [Range(0, 100)] [SerializeField] int calm = 100;
        [Range(0, 50)] [SerializeField] int fade = 50;
        [Range(0, 100)] [SerializeField] int gondola_idle = 100;

        int prevcalm = 100;
        int prevfade = 50;
        int prev = 100;


    private void Start()
    {
        MusicEmitter = GetComponent<StudioEventEmitter>();

        MusicEmitter.SetParameter("calm", (float)calm);
        MusicEmitter.SetParameter("fade", (float)fade);
        MusicEmitter.SetParameter("gondola_idle", (float)gondola_idle);

    }


        private void Update()
        {
            if (calm != prevcalm)
            {
                MusicEmitter.SetParameter("calm", (float)calm);
                prevcalm = calm;
            }

            if (fade != prevfade)
            {
                MusicEmitter.SetParameter("fade", (float)fade);
                fade = prevfade;
            }

            if (gondola_idle != calm)
            {
                MusicEmitter.SetParameter("fade", (float)fade);
                prevfade = fade;
            }
        }
    }

