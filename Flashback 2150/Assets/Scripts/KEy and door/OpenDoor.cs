using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public Animation HingeHere;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void OnTriggerStay ()
    {
        if (Input.GetKey(KeyCode.E))
            HingeHere.Play();
		
	}
}
