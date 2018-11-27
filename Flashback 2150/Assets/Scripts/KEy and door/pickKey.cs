using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickKey : MonoBehaviour
{
    public Component doorcolliderhere;
    public GameObject Keygone;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void OnTriggerStay ()
    {
        if(Input.GetKey(KeyCode.E))
        doorcolliderhere.GetComponent<BoxCollider>().enabled = true;

        if(Input.GetKey(KeyCode.E))
        Keygone.SetActive(false);
	}
}
