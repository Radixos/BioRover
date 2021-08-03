using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIconController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private float deactivateDelay;
    private float deactivateTimer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        deactivateDelay = 3.0f;
        deactivateTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(spriteRenderer.enabled)
        {
            if (deactivateTimer >= deactivateDelay)
            {
                deactivateTimer = 0.0f;
<<<<<<< refs/remotes/origin/main
                gameObject.SetActive(false);
=======
                spriteRenderer.enabled = false;
>>>>>>> levels
            }
            else
                deactivateTimer += Time.deltaTime;
        }
    }
<<<<<<< refs/remotes/origin/main
=======

    public void ActivateIcon()
    {
        spriteRenderer.enabled = true;
    }
>>>>>>> levels
}
