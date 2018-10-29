using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour {

        /* State Bools*/
    bool runningOn;
    bool crouchOn;
    bool crouchToggle;
    public bool inJump;

    public float runningSpeed;
    public float rollingSpeed;
    public float rollDistance;
    public float timeRolling;
    public float rollDelay;
    public float rollDuration;
    public float airTime;
    public float jumpDelay;
    public float jumpDuration;

        /* Movement Speed */
    public float walkSpeed;
    public float runSpeed;
    public float runTime;
    public float rJumpRunUp;

        /* Jump Heights */
    public float sJumpHeight;
    public float rJumpHeight;
    public float lJumpHeight;

        /* Jump Propulsion */
    public float rJumpPropulsion;
    public float rJumpDirection;
    public float rJumpSpeed = 10;

    public float lJumpPropulsion;
    public float lJumpDirection;
    public float lJumpSpeed;

    public float sJumpSpeed;

    public Transform target;
    public float speed;

    public Rigidbody rb;

    public bool rollingRight;
    public bool rollingLeft;

    /* Player Look */
    public bool lookLeft;
    public Animator lookingAnimation;

    // Use this for initialization
    void Start ()
    {
        runningOn = false;
        crouchOn = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
            /* Look Direction */
        if (Input.GetKeyDown(KeyCode.A))
        {
            lookLeft = true;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            lookLeft = false;
        }

        if (lookLeft == true)
        {
            lookingAnimation.SetBool("left?", true);
        } 
        else
        {
            lookingAnimation.SetBool("left?", false);
        }

            /* Run Button */
        if (Input.GetKey(KeyCode.LeftShift))
        {
            runningOn = true;
        }
        else
        {
            runningOn = false;
        }

            /* Crouch Button*/
        if (Input.GetKeyDown(KeyCode.C))
        {
            crouchOn =! crouchOn;
        }

            /* Crouch/Standing Scales */
        if (crouchOn == true)
        {
            transform.localScale = new Vector3(1.1f, 1f, 1.1f);

            if (crouchToggle == false)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
                crouchToggle = true;
            }
            
        }
        else
        {
            transform.localScale = new Vector3(0.75f, 2, 0.75f);
            if (crouchToggle == true)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                crouchToggle = false;
            }
        }

            /* Jump button */
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
        {
            inJump = true;
        }

        //else
        //{
        //    inJump = false;
        //}

            /* Running Jump Direction */
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) || (Input.GetKeyUp(KeyCode.A) && Input.GetKeyUp(KeyCode.D)))
            {
                rJumpDirection = 0;
            }

            else
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    rJumpDirection = -rJumpPropulsion;
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    rJumpDirection = rJumpPropulsion;
                }
            }

            /* Leap Jump Direction */
            if (lookLeft == true)
        {
            lJumpDirection = -lJumpPropulsion;
        }
            else
        {
            lJumpDirection = lJumpPropulsion;
        }

            /* Roll direction */
        if (timeRolling > rollDelay)
        {
            if (crouchOn == true)
            {
                if (Input.GetKeyDown(KeyCode.D))
                {
                    timeRolling = 0;
                    rollingRight = true;
                }

                if (Input.GetKeyDown(KeyCode.A))
                {
                    timeRolling = 0;
                    rollingLeft = true;
                }

            }
        }

        timeRolling += Time.deltaTime;

            /* Rolling movement */
        if (rollingRight == true)
        {
            if (timeRolling < rollDuration)
            {
                transform.Translate(Vector3.right * Time.deltaTime * rollingSpeed);
            }

            if (timeRolling > rollDelay)
            {
                rollingRight = false;
            }
        }

        if (rollingLeft == true)
        {
            if (timeRolling < rollDuration)
            {
                transform.Translate(Vector3.left * Time.deltaTime * rollingSpeed);
            }

            if (timeRolling > rollDelay)
            {
                rollingLeft = false;
            }
        }

            /* Walking and Running Functions */
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

                if ((Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.A))))
                {
                    runTime += Time.deltaTime;
                }
                
                else
                {
                    runTime = 0;
                }
            }

            else
            {
                if (Input.GetKey(KeyCode.D))
                {
                    //transform.Translate(Vector3.right * Time.deltaTime);
                    rb.AddForce(Vector3.right * walkSpeed);
                }

                if (Input.GetKey(KeyCode.A))
                {
                    //transform.Translate(Vector3.left * Time.deltaTime);
                    rb.AddForce(Vector3.left * walkSpeed);
                }
            }
        }

            /* If crouch is on, jumping features are disabled */
            // Add "|| gunMode == true" for when gunMode is implemented!
        if (crouchOn == true)
        {

        }

        else
        {
            if (inJump == true)
            {
                airTime += Time.deltaTime;

                    /* Stationary Jump */
                if (runningOn == false)
                {
                    if (airTime < jumpDuration)
                    {
                        rb.AddForce(new Vector3(0, sJumpHeight) * sJumpSpeed);
                    }
                    //  Dictates how long the transform lasts for the jump.

                    else
                    {
                        if (airTime > jumpDelay)
                        {
                            airTime = 0;
                            inJump = false;
                        }
                    }
                    //  Acts as cooldown for after the jump.
                }
                    /* Running Jump Test */
                else
                {
                    if (airTime < jumpDuration)
                    {
                        if (runTime > rJumpRunUp)
                        {
                            rb.AddForce(new Vector3(rJumpDirection, rJumpHeight) * rJumpSpeed);
                        }
                        else
                        {
                            rb.AddForce(new Vector3(lJumpDirection, lJumpHeight) * lJumpSpeed);
                        }
                    }

                    else
                    {
                        if (airTime > jumpDelay)
                        {
                            airTime = 0;
                            inJump = false;
                        }
                    }
                }
            }
        }    

            //if (airTime > 1)
            //{
            //    if (inJump == true)
            //    {
            //        if (airTime < jumpDelay)
            //        {
            //            if (runningOn == true)
            //            {
            //                transform.Translate(0, rJumpHeight, 0);
            //                airTime = 0;
            //            }

            //            else
            //            {
            //                transform.Translate(0, 0.2f, 0);
            //                rb.AddForce(Vector3.up, ForceMode.Impulse);
            //                //rb.AddForce(new Vector3(0, sJumpHeight, 0), ForceMode.Force);
            //                airTime = 0;
            //            }
            //        }
            //        else
            //        {
            //            inJump = false;
            //        }
            //    }
            //}
        }
    }
