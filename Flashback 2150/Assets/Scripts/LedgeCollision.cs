using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeCollision : MonoBehaviour {

    public bool collided;
    public GameObject ledgeGrabbed;
    public GameObject player;
    public bool ledgeDirection;
    public bool playerDirection;

	// Use this for initialization
	void Start ()
    {
        collided = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        playerDirection = player.GetComponent<MovementScript>().lookLeft;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LedgeCollider")
        {
            ledgeGrabbed = other.gameObject;
            ledgeDirection = ledgeGrabbed.GetComponent<FootDetection>().left;
            if (ledgeDirection != playerDirection)
            {
                collided = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "LedgeCollider")
        {
            if (ledgeDirection != playerDirection)
            {
                collided = false;
            }
        }
    }
}
