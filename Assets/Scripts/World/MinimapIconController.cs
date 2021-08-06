using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIconController : MonoBehaviour
{
    //private SpriteRenderer spriteRenderer;

    private float deactivateDelay;
    private float deactivateTimer;

    // Start is called before the first frame update
    void Start()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();

        deactivateDelay = 3.0f;
        deactivateTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (deactivateTimer >= deactivateDelay)
        {
            deactivateTimer = 0.0f;
            gameObject.SetActive(false);
        }
        else
            deactivateTimer += Time.deltaTime;
    }
}