using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour {

    public bool runningOn;
    public bool crouchOn;
    public bool inJump;
    public int runningSpeed;
    public int rollDistance;
    public float timeRolling;
    public float rollDelay;
    public float airTime;

    // Use this for initialization
    void Start ()
    {
        runningOn = false;
        crouchOn = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            runningOn = true;
        }

        else
        {
            runningOn = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            crouchOn =! crouchOn;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
        {
            inJump = true;
        }

        else
        {
            inJump = false;
        }

        timeRolling += Time.deltaTime;
        
        if (timeRolling > rollDelay)
        {
            if (crouchOn == true)
            {
                if (Input.GetKey(KeyCode.D))
                {
                    transform.Translate(rollDistance,0,0);
                    timeRolling = 0;
                }

                if (Input.GetKey(KeyCode.A))
                {
                    transform.Translate(-rollDistance, 0, 0); ;
                    timeRolling = 0;
                }
            }
        }

        if (crouchOn == false && inJump == false)
        {
            if (runningOn == true)
            {
                if (Input.GetKey(KeyCode.D))
                {
                    transform.Translate(Vector3.right * Time.deltaTime * runningSpeed);
                }

                if (Input.GetKey(KeyCode.A))
                {
                    transform.Translate(Vector3.left * Time.deltaTime * runningSpeed);
                }
            }

            else
            {
                if (Input.GetKey(KeyCode.D))
                {
                    transform.Translate(Vector3.right * Time.deltaTime);
                }

                if (Input.GetKey(KeyCode.A))
                {
                    transform.Translate(Vector3.left * Time.deltaTime);
                }
            }
        }

        airTime += Time.deltaTime;
        if (airTime > 3)
        {
            if (inJump == true)
            {
                if (runningOn == true)
                {
                    transform.Translate(0, 1, 0);
                    airTime = 0;
                    inJump = false;
                }

                else
                {
                    transform.Translate(0, 1.5f, 0);
                    airTime = 0;
                    inJump = false;
                }
            }
        }
    }
}
