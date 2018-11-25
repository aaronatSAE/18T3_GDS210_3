using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootDetection : MonoBehaviour {

    public bool left;
    public GameObject LFootDetector;
    public GameObject RFootDetector;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (left == true)
        {
            RFootDetector.SetActive(false);
            LFootDetector.SetActive(true);
        }
        else
        {
            RFootDetector.SetActive(true);
            LFootDetector.SetActive(false);
        }
	}
}
