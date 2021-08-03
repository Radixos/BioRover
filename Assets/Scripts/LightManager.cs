//----------------------------------------------------------------------------------------------------------------
//---------------------This script needs to be attached to the light producing object (light)---------------------
//----------------------------------------------------------------------------------------------------------------

using UnityEngine;

public class LightManager : MonoBehaviour
{
    public GameObject direction;
    public bool hasHit;
    public Vector3 hitPos;

    void FixedUpdate()
    {
        Vector3 localDirection = direction.transform.position;

        RaycastHit raycastHit;

        float sphereRadius = direction.GetComponent<SphereCollider>().radius * 2;
        Vector3 oddThing = new Vector3(sphereRadius, sphereRadius, sphereRadius);

        if (Physics.Raycast(this.transform.position, localDirection - this.transform.position + oddThing, out raycastHit, Mathf.Infinity))
        {
            if (raycastHit.collider.gameObject.name == direction.name)
            {
                Debug.DrawRay(this.transform.position, localDirection - this.transform.position + oddThing, Color.green, Time.deltaTime, true);
                Debug.Log("Hit " + raycastHit.collider.gameObject.name);
                hasHit = true;
                hitPos = raycastHit.point;
            }
            else
            {
                Debug.DrawRay(this.transform.position, localDirection - this.transform.position + oddThing, Color.red, Time.deltaTime, true);
                hasHit = false;
            }
        }
    }
}