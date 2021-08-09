using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    //void Update()
    //{

    //}

    public MusicIntensity musicEmitter;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Rover_Prefab")
        {
            musicEmitter.calm = 0;
            musicEmitter.fade = 25;
            musicEmitter.gondola_idle = 50;
        }
    }

}
