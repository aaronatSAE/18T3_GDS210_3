using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCollision : MonoBehaviour {

    public bool midAir;
    public GameObject floorGrabbed;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerStay(Collider other)
    {
            midAir = false;
    }

    private void OnTriggerExit(Collider other)
    {
         midAir = true;
         floorGrabbed = other.gameObject;
    }
}
