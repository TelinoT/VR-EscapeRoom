using UnityEngine;
using System.Collections;

public class BreakableGlass : MonoBehaviour
{
    public GameObject particles;
    public GameObject brokenGlass;

    public float breakForce = 0.5f;

    private bool isBroken = false;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (isBroken) return;

        if (collision.gameObject.CompareTag("Hammer"))
        {

            this.GetComponent<AudioSource>().Play();
            
            float impactVelocity = collision.relativeVelocity.magnitude;
            
            if (impactVelocity > breakForce)
            {
                Break();
            }
        }
    }
    
    public void Break()
    {
        isBroken = true;

        Instantiate(particles, transform.position, Quaternion.identity);
        Instantiate(brokenGlass, transform.position, transform.rotation);

        foreach (Transform child in transform)
        {
            child.GetComponent<Collider>().enabled = false;
            child.GetComponent<MeshRenderer>().enabled = false;
        }
        
        Destroy(gameObject, 2f);
    }
}
