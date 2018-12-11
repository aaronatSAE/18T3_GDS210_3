using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeCollision : MonoBehaviour {

    public bool collided;
    public GameObject ledgeGrabbed;

    public bool playerLeft;
    public bool ledgeLeft;

	// Use this for initialization
	void Start ()
    {
        collided = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LedgeCollider")
        {
            ledgeLeft = other.GetComponent<FootDetection>().left;
            playerLeft = GetComponentInParent<MovementScript>().lookLeft;

            if (ledgeLeft != playerLeft)
            {
                collided = true;
                ledgeGrabbed = other.gameObject;
            }

            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "LedgeCollider")
        {
            collided = false;
        }
    }
}
