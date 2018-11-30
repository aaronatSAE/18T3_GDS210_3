using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public float shield;

	// Use this for initialization
	void Start ()
    {
        shield = 5;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (shield > 5)
        {
            shield = 5;
        }
	}
}
