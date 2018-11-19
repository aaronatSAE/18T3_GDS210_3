using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {

    Vector3 moveDirection = Vector3.up;
    public float speed1 = 100;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void move()
    {
        if (transform.position.y >= 50)
        {
            moveDirection = Vector3.down;
        }
        else if(transform.position.y <= 0);
        {
            moveDirection = Vector3.up;
        }
        // implicit else... if it's in between, it should keep moving in the same direction it last was...

        transform.Translate(moveDirection * Time.deltaTime * speed1);
    }
}
