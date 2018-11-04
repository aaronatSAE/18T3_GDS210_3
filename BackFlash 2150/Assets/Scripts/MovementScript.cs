using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{

    /* State Bools*/
    bool runningOn;
    bool crouchOn;
    bool crouchToggle;
    public bool inJump;
    public bool midAir;

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
    public float walkTurnDelay;
    public float runSpeed;
    public float runTurnDelay;
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

    public float lookTime;

    // Use this for initialization
    void Start()
    {
        runningOn = false;
        crouchOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        /* Look Direction */
        if (midAir == false)
        {
            if (Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.D))
            {
            }

            else
            {
                if (Input.GetKey(KeyCode.A))
                {
                    lookLeft = true;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    lookLeft = false;
                }
            }
        }

        /* Looking Animation*/
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


        /* Crouch Functions */
        /* Crouch Button*/
        if (rollingLeft == false && rollingRight == false)
        {
            if (midAir == false)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    crouchOn = !crouchOn;
                }
            }
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
            // crouchToggle controls position of player when it scales
        }
        else
        {
            transform.localScale = new Vector3(0.75f, 2, 0.75f);
            if (crouchToggle == true)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                crouchToggle = false;
            }
            // crouchToggle controls position of player when it scales
        }


        /* Jump Mechanics*/
        /* Stationary Jump */
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) || (Input.GetKeyUp(KeyCode.A) && Input.GetKeyUp(KeyCode.D)))
        {
            rJumpDirection = 0;
        }

        /* Running Jump Direction */
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
        // Values set in Inspector

        /* Jump button */
        if (midAir == false)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
            {
                inJump = true;
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
        // Values set in Inspector

        /* Rolling Mechanics */
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
                //transform.Translate(Vector3.right * Time.deltaTime * rollingSpeed);
                rb.AddForce(Vector3.right * rollingSpeed);
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
                //transform.Translate(Vector3.left * Time.deltaTime * rollingSpeed);
                rb.AddForce(Vector3.left * rollingSpeed);
            }
            if (timeRolling > rollDelay)
            {
                rollingLeft = false;
            }
        }

        /* Walking, Running and Crouch Mechanics */
        if (crouchOn == false && midAir == false || rollingLeft == true || rollingRight == true)
        {
            if ((Input.GetKeyUp(KeyCode.D) || (Input.GetKeyUp(KeyCode.A))))
            {
                runTime = 0;
            }
            // Resets running timer for when a player decides to walk after a run.

            /* Running */
            if (runningOn == true)
            {
                lookTime += Time.deltaTime;

                if (lookTime > runTurnDelay)
                {
                    if (Input.GetKey(KeyCode.D))
                    {
                        transform.Translate(Vector3.right * Time.deltaTime * runningSpeed);
                        runTime += Time.deltaTime;
                    }

                    if (Input.GetKey(KeyCode.A))
                    {
                        transform.Translate(Vector3.left * Time.deltaTime * runningSpeed);
                        runTime += Time.deltaTime;
                    }

                    if ((Input.GetKeyDown(KeyCode.D) || (Input.GetKeyDown(KeyCode.A))))
                    {
                        lookTime = 0;
                    }
                }
            }
            // Manages running speed, direction and delay before running
            // Also manages whenever player lifts fingers off movement keys to determinte runup for running jump.

            /* Walking */
            else
            {
                lookTime += Time.deltaTime;
                if (lookTime > walkTurnDelay)
                {
                    if (Input.GetKey(KeyCode.A))
                    {
                        transform.Translate(Vector3.left * Time.deltaTime);
                        //rb.AddForce(Vector3.left * walkSpeed);
                    }

                    if (Input.GetKey(KeyCode.D))
                    {
                        transform.Translate(Vector3.right * Time.deltaTime);
                        //rb.AddForce(Vector3.right * walkSpeed);
                    }

                    if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
                    {
                        lookTime = 0;
                    }
                }
            }
            // Manages running speed, direction and delay before running


            /* Jump Mechanic Limits */
            // Add "|| gunMode == true" for when gunMode is implemented!

        }

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
                    //Dictates how long the transform lasts for the jump.

                    else
                    {
                        //if (airTime > jumpDelay)
                        //{
                            airTime = 0;
                            inJump = false;
                        //}
                    }
                    //Acts as cooldown for after the jump.
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
                        airTime = 0;
                        inJump = false;
                    }
                }
            }
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        midAir = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        midAir = true;
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
