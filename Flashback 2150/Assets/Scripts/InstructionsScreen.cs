using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsScreen : MonoBehaviour {

    public GameObject instructionsEGO;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void InstructionsON()
    {
        instructionsEGO.SetActive(true);
    }

    public void InstructionsOFF()
    {
        instructionsEGO.SetActive(false);
    }
}
