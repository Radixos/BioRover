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

    [Range(0, 100)] public int calm = 100;
    [Range(0, 50)] public int fade = 50;
    [Range(0, 100)] public int gondola_idle = 100;

    float prevcalm = 100;
    float prevfade = 50;
    float prevGondola = 100;


    private void Start()
    {
        MusicEmitter = GetComponent<StudioEventEmitter>();

        MusicEmitter.SetParameter("calm", (float)calm);
        MusicEmitter.SetParameter("fade", (float)fade);
        MusicEmitter.SetParameter("Gondola_idle", (float)gondola_idle);
    }


    private void Update()
    {
        if (calm != prevcalm)
        {
            //MusicEmitter.SetParameter("calm", (float)calm);

            //for(int i = 0; i < MusicEmitter.Params.Length; i++)
            //{
            //    if(MusicEmitter.Params[i].Name == "calm")
            //    {
            //        MusicEmitter.Params[i].Value = (float)calm;
            //        break;
            //    }
            //}

            if(calm > prevcalm)
            {
                prevcalm += (float)(3f * Time.deltaTime);

                for (int i = 0; i < MusicEmitter.Params.Length; i++)
                {
                    if (MusicEmitter.Params[i].Name == "calm")
                    {
                        MusicEmitter.Params[i].Value = (float)prevcalm;
                        break;
                    }
                }

                if(prevcalm > calm)
                    prevcalm = calm;
            }
            else if(calm < prevcalm)
            {
                prevcalm -= (float)(3f * Time.deltaTime);

                for (int i = 0; i < MusicEmitter.Params.Length; i++)
                {
                    if (MusicEmitter.Params[i].Name == "calm")
                    {
                        MusicEmitter.Params[i].Value = (float)prevcalm;
                        break;
                    }
                }

                if (prevcalm < calm)
                    prevcalm = calm;
            }
        }


        if (fade != prevfade)
        {
            //MusicEmitter.SetParameter("fade", (float)fade);

            for (int i = 0; i < MusicEmitter.Params.Length; i++)
            {
                if (MusicEmitter.Params[i].Name == "fade")
                {
                    MusicEmitter.Params[i].Value = (float)fade;
                    break;
                }
            }

            prevfade = fade;
        }

        if (gondola_idle != prevGondola)
        {
            //MusicEmitter.SetParameter("Gondola_idle", (float)gondola_idle);

            for (int i = 0; i < MusicEmitter.Params.Length; i++)
            {
                if (MusicEmitter.Params[i].Name == "Gondola_idle")
                {
                    MusicEmitter.Params[i].Value = (float)gondola_idle;
                    break;
                }
            }

            prevGondola = gondola_idle;
        }
    }
}

