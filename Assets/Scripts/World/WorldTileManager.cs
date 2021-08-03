using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTileManager : MonoBehaviour
{
    private Transform roverTransform;

    public GameObject centerTile;

    // Start is called before the first frame update
    void Start()
    {
        roverTransform = FindObjectOfType<RoverController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tilePosition = new Vector3(transform.position.x, roverTransform.transform.position.y, transform.position.z);

        if (Vector3.Distance(tilePosition, roverTransform.position) <= 400)
            centerTile.SetActive(true);
        else
            centerTile.SetActive(false);
    }
}
