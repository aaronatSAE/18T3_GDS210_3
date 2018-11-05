using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [Header("State Bools")]
    bool runningOn;
    bool crouchOn;
    bool crouchToggle;
    public bool inJump;
    public bool midAir;
    public bool ledgeCollided;
    public bool ledgeHang;

    [Space(10)]
    public float rollingSpeed;
    public float rollDistance;
    public float timeRolling;
    public float rollDelay;
    public float rollDuration;
    public float airTime;
    public float jumpDelay;
    public float jumpDuration;

    [Header("Walk/Run Values")]
    public float walkSpeed;
    public float walkTurnDelay;
    public float runningSpeed;
    public float runTurnDelay;
    public float runTime;
    public float rJumpRunUp;

    [Header("Running Jump Values")]
    public float rJumpHeight;
    public float rJumpPropulsion;
    public float rJumpDirection;
    float rJumpSpeed;

    [Header("Leaping Jump Values")]
    public float lJumpHeight;
    public float lJumpPropulsion;
    public float lJumpDirection;
    float lJumpSpeed;

    [Header("Stationary Jump Values")]
    public float sJumpHeight;
    float sJumpSpeed;

    [Space(10)]
    //public Transform target;
    //public float speed;

    public Rigidbody rb;
    public GameObject ledgeCollider;
    public GameObject floorCollider;
    public GameObject ledgeGrabbed;
    public GameObject emptyLedge;
    public bool climbing;
    public float climbTimer;

    public float ledgeTimer;
    public float ledgeDelay;

    public bool rollingRight;
    public bool rollingLeft;

    /* Player Look */
    public bool lookLeft;
    public Animator lookingAnimation;

    public float lookTime;

    Component rbComponent;

    // Use this for initialization
    void Start()
    {
        runningOn = false;
        crouchOn = false;
        rbComponent = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ledgeCollided == true)
        {
            ledgeTimer += Time.deltaTime;

            if (ledgeDelay > ledgeTimer)
            {
                ledgeGrabbed = ledgeCollider.GetComponent<LedgeCollision>().ledgeGrabbed;


                gameObject.transform.parent = ledgeGrabbed.transform;

                if (lookLeft == false)
                {
                    gameObject.transform.localPosition = new Vector3(-0.7789993f, -0.952f, 0);
                }
                else
                {
                    gameObject.transform.localPosition = new Vector3(0.7789993f, -0.952f, 0);
                }
            }

            if (ledgeHang == false)
            {
                GetComponent<Rigidbody>().isKinematic = true;
                ledgeHang = true;
            }
        }
        // Detects which ledge in the map was grabbed and, depending on the player orientation, snaps the player to the ledge.
        // Also makes the rigidbody of the player a kinematic to keep the player in place.

        else
        {
            ledgeHang = false;
            GetComponent<Rigidbody>().isKinematic = false;
        }

        if (ledgeHang == true)
        {

            if (Input.GetKeyDown(KeyCode.S))
            {
                if (lookLeft == false)
                {
                    gameObject.transform.localPosition = new Vector3(-0.7789993f, -1.3f, 0);
                }
                else
                {
                    gameObject.transform.localPosition = new Vector3(0.7789993f, -1.3f, 0);
                }
                GetComponent<Rigidbody>().isKinematic = false;
                ledgeCollided = false;
                ledgeHang = false;
                rb.velocity = new Vector3(0, -4, 0);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                climbing = true;
                if (lookLeft == false)
                {
                    GetComponent<Rigidbody>().isKinematic = false;
                    ledgeCollided = false;
                    ledgeHang = false;
                    ledgeGrabbed.GetComponent<Animator>().SetTrigger("climbRight");
                }
            }

            if (climbTimer >= 0.785f)
            {
                gameObject.transform.parent = emptyLedge.transform;
            }


            if (climbing == true)
            {
                climbTimer += Time.deltaTime;
            }
        }
        // Dictates actions the player may take while they hang on a ledge.



        else
        {
            //ledgeGrabbed = emptyLedge;
            ledgeTimer = 0;
            //ledgeHang = false;
            //GetComponent<Rigidbody>().isKinematic = false;


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
                if (lookLeft == true)
                {
                    rJumpDirection = -rJumpPropulsion;
                }
                if (lookLeft == false)
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
                            rb.velocity = new Vector3(2f, -2f, 0);
                            runTime += Time.deltaTime;
                        }

                        if (Input.GetKey(KeyCode.A))
                        {
                            transform.Translate(Vector3.left * Time.deltaTime * runningSpeed);
                            rb.velocity = new Vector3(-2f, -2f, 0);
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
                            //rb.AddForce(new Vector3(0, sJumpHeight) * sJumpSpeed);
                            //sJumpHeight value approx. = 0.075

                            rb.velocity = new Vector3(0, sJumpHeight, 0);
                            //sJumpHeight value approx. = 5
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
                                //rb.AddForce(new Vector3(rJumpDirection, rJumpHeight) * rJumpSpeed);
                                // rJumpDirection value = propulsion -> approx. = 0.075
                                // rJumpHeigh value approx. = 0.075

                                rb.velocity = new Vector3(rJumpDirection, rJumpHeight, 0);

                                // rJumpDirection value approx. = 
                                // rJumpHeight value approx. = 
                            }
                            else
                            {
                                //rb.AddForce(new Vector3(lJumpDirection, lJumpHeight) * lJumpSpeed);
                                // lJumpDirection value approx. = 0.055
                                // lJumpHeight value approx. = 0.095

                                rb.velocity = new Vector3(lJumpDirection, lJumpHeight, 0);
                                // lJumpDirection value approx. = 0.055
                                // lJumpHeight value approx. = 0.095
                            }
                        }

                        else
                        {
                            airTime = 0;
                            runTime = 0;
                            inJump = false;
                        }
                    }
                }
            }
        }
        ledgeCollided = ledgeCollider.GetComponent<LedgeCollision>().collided;

        Vector3 down = transform.TransformDirection(Vector3.down) * 1.2f;
        Vector3 frontDown = new Vector3(transform.position.x + 0.38f, transform.position.y, transform.position.z);
        Vector3 backDown = new Vector3(transform.position.x - 0.38f, transform.position.y, transform.position.z);

        bool rayIntersectsSomething = Physics.Raycast(transform.position, down, 1.2f) || Physics.Raycast(frontDown, down, 1.2f) || Physics.Raycast(backDown, down, 1.2f);

        Debug.DrawRay(transform.position, down, Color.green);
        Debug.DrawRay(frontDown, down, Color.red);
        Debug.DrawRay(backDown, down, Color.cyan);

        if (rayIntersectsSomething)
        {
            midAir = false;
        }

        else
        {
            midAir = true;
        }

        //private void OnCollisionEnter(Collision collision)
        //{
        //    midAir = false;
        //}

        //private void OnCollisionExit(Collision collision)
        //{
        //    midAir = true;
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "LedgeCollider")
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
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
