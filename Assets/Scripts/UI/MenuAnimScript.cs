using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimScript : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GrowButton()
    {
        anim.SetBool("GrowButton", true);
    }

    public void ShrinkButton()
    {
        anim.SetBool("GrowButton", false);
    }

}
