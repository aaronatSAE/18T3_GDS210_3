using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableLight : MonoBehaviour
{
    public GameObject myObject;
    // Use this for initialization
    void Start ()

    {
       
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void Light ()
    {
        gameObject.SetActive(false);
    }

}
