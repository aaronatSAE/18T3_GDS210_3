using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RechargeStation : MonoBehaviour {

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
        if (other.transform.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                //other.GetComponent<PlayerHealth>().shield = 5;
                other.transform.GetComponent<PlayerHealth>().shield = 5;
            }
        }
    }
}
