using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [Space(10)]
    public float timeRolling;
    public float airTime;
    public float jumpDelay;
    public float jumpDuration;
    public GameObject playerNose;
    public Material noseMaterial;

    [Header("State Bools")]
    bool runningOn;
    bool crouchOn;
    bool crouchToggle;
    bool gunMode;
    bool gunAim;
    public bool inJump;
    public bool midAir;
    public bool ledgeCollided;
    public bool ledgeHang;

    [Header("Gun Values")]
    public float gunTimer;
    public float gunDelay;

    [Header("Rolling Values")]
    public float rollingSpeed;
    public float rollDistance;
    public float rollDelay;
    public float rollDuration;
    public float crouchTurnDelay;
    public float rollFallMomentumX;
    public float rollFallMomentumY;

    [Header("Walk/Run Values")]
    public float walkSpeed;
    public float walkTurnDelay;
    public float runningSpeed;
    public float runTurnDelay;
    public float runTime;
    public float rJumpRunUp;
    public Vector3 impactSpeed;

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

    [Header("Ledge Floor Stuff")]
    public Rigidbody rb;
    public GameObject ledgeCollider;
    public GameObject floorCollider;
    public GameObject ledgeGrabbed;
    public GameObject emptyLedge;
    public bool climbing;
    public float climbTimer;
    public float climbDelay;
    public float ledgeTimer;
    public float ledgeDelay;
    public GameObject ledgeUnderneath;
    public float hangTimer;
    public float hangDelay;

    [Space(10)]
    //public Transform target;
    //public float speed;

    public bool rollingRight;
    public bool rollingLeft;

    /* Player Look */
    public bool lookLeft;
    public Animator lookingAnimation;
    public Animator lookSprite;

    public float lookTime;

    public bool debug;

    public bool lookDelay;

    Component rbComponent;

    // Use this for initialization
    void Start()
    {
        runningOn = false;
        crouchOn = false;
        rbComponent = GetComponent<Rigidbody>();
        noseMaterial = playerNose.GetComponent<Renderer>().material;
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

        gunTimer += Time.deltaTime;

        if (rb.velocity.x == 0 && rb.velocity.y == 0 && rb.velocity.z == 0 && gunTimer > gunDelay)
        {
            /* Gun Mode/Aim ON/OFF */
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (gunMode == true)
                {
                    gunMode = !gunMode;
                    gunAim = false;
                    gunTimer = 0;

                }
                else
                {
                    gunMode = !gunMode;
                    gunAim = true;
                    gunTimer = 0;

                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && gunMode == true)
            {
                if (gunAim == false)
                {
                    gunAim = true;
                    gunTimer = 0;

                }
            }

            if (gunAim == true && ((lookLeft == true && Input.GetKey(KeyCode.A) || lookLeft == false && Input.GetKey(KeyCode.D))))
            {
                gunAim = false;
                gunTimer = 0;
            }

            if (gunMode == true && ((lookLeft == false && Input.GetKey(KeyCode.A) || lookLeft == true && Input.GetKey(KeyCode.D))))
            {
                gunTimer = 0;
            }
        }


        if (gunAim == true)
        {
            noseMaterial.color = Color.red;
        }
        else if (gunMode == true)
        {
            noseMaterial.color = Color.yellow;
        }
        else
        {
            noseMaterial.color = Color.green;
        }

        if (ledgeHang == true)
        {
            if (hangTimer > hangDelay)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    if (lookLeft == false)
                    {
                        hangTimer = 0;
                        gameObject.transform.localPosition = new Vector3(-0.7789993f, -1.3f, 0);
                    }
                    else
                    {   
                        hangTimer = 0;
                        gameObject.transform.localPosition = new Vector3(0.7789993f, -1.3f, 0);
                    }

                    GetComponent<Rigidbody>().isKinematic = false;
                    ledgeCollided = false;
                    ledgeHang = false;
                    rb.velocity = new Vector3(0, -4, 0);
                }
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                if (hangTimer > climbDelay)
                {
                    climbing = true;
                    if (lookLeft == false)
                    {
                        ledgeTimer = 0;
                        hangTimer = 0;
                        GetComponent<Rigidbody>().isKinematic = false;
                        ledgeCollided = false;
                        ledgeHang = false;
                        ledgeGrabbed.GetComponent<Animator>().SetTrigger("climbRight");
                    }

                    if (lookLeft == true)
                    {
                        ledgeTimer = 0;
                        hangTimer = 0;
                        GetComponent<Rigidbody>().isKinematic = false;
                        ledgeCollided = false;
                        ledgeHang = false;
                        ledgeGrabbed.GetComponent<Animator>().SetTrigger("climbLeft");
                    }
                }
            }

            if (climbTimer >= climbDelay)
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
                climbing = false;
                climbTimer = 0;

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
                //lookSprite.SetBool("left?", true);
            }
            else
            {
                lookingAnimation.SetBool("left?", false);
                //lookSprite.SetBool("left?", false);
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
                lookTime += Time.deltaTime;
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
            if (midAir == false && gunMode == false)
            {
                if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && crouchOn == false)
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
                if (debug == false)
                {
                    if (crouchOn == true)
                    {
                        if (lookLeft == true)
                        {
                            if (Input.GetKeyDown(KeyCode.D))
                            {
                                lookTime = 0;
                            }

                            else if (Input.GetKeyDown(KeyCode.A))
                            {
                                if (lookTime > crouchTurnDelay)
                                {
                                    timeRolling = 0;
                                    rollingLeft = true;
                                }
                            }
                        }

                        if (lookLeft == false)
                        {
                            if (Input.GetKeyDown(KeyCode.A))
                            {
                                lookTime = 0;
                            }

                            else if (Input.GetKeyDown(KeyCode.D))
                            {
                                if (lookTime > crouchTurnDelay)
                                {
                                    timeRolling = 0;
                                    rollingRight = true;
                                }
                            }
                        }
                    }
                }

                if (debug == true)
                {
                    if (crouchOn == true)
                    {
                        if (lookLeft == true)
                        {
                            if (Input.GetKeyDown(KeyCode.D))
                            {
                                lookTime = 0;
                                lookDelay = false;
                            }

                            else if (Input.GetKeyDown(KeyCode.A))
                            {
                                if (lookTime > crouchTurnDelay)
                                {
                                    timeRolling = 0;
                                    rollingLeft = true;
                                }
                            }
                        }

                        if (lookLeft == false)
                        {
                            if (Input.GetKeyDown(KeyCode.A))
                            {
                                lookTime = 0;
                                lookDelay = false;
                            }

                            else if (Input.GetKeyDown(KeyCode.D))
                            {
                                if (lookTime > crouchTurnDelay)
                                {
                                    timeRolling = 0;
                                    rollingRight = true;
                                }
                            }
                        }
                    }
                }
                
            }

            if (rollingLeft == true || rollingRight == true)
            {
                lookTime = 0;
            }

            timeRolling += Time.deltaTime;

            /* Rolling movement */
            if (rollingRight == true)
            {
                if (midAir == true)
                {
                    rb.AddForce(new Vector3(-rollFallMomentumX, -rollFallMomentumY, 0));
                }

                else
                {
                    if (timeRolling < rollDuration)
                    {
                        //transform.Translate(Vector3.right * Time.deltaTime * rollingSpeed);
                        //rb.AddForce(Vector3.right * rollingSpeed);
                        rb.velocity = Vector3.right * rollingSpeed;
                    }

                    if (timeRolling > rollDelay)
                    {
                        rollingRight = false;
                        rb.velocity = Vector3.zero;
                    }
                }
            }

            if (rollingLeft == true)
            {
                if (midAir == true)
                {
                    rb.AddForce(new Vector3(rollFallMomentumX, -rollFallMomentumY, 0));
                }

                else
                {
                    if (timeRolling < rollDuration)
                    {
                        //transform.Translate(Vector3.left * Time.deltaTime * rollingSpeed);
                        //rb.AddForce(Vector3.left * rollingSpeed);
                        rb.velocity = Vector3.left * rollingSpeed;
                    }

                    if (timeRolling > rollDelay)
                    {
                        rollingLeft = false;
                        rb.velocity = Vector3.zero;
                    }
                }
            }

            /* Walking, Running and Crouch Mechanics */
            if (crouchOn == false && midAir == false && gunAim == false && gunTimer > gunDelay || rollingLeft == true || rollingRight == true)
            {
                if ( Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftShift) )
                {
                    runTime = 0;
                }
                // Resets running timer for when a player decides to walk after a run.

                /* Running */
                if (gunMode == false)
                {
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
                }

                // Above = Running Enabled
                // Below = Running Disabled
            }
            // Manages running speed, direction and delay before running
            // Also manages whenever player lifts fingers off movement keys to determinte runup for running jump.


            /* Walking */
            if ((crouchOn == false && midAir == false && gunAim == false && gunTimer > gunDelay || rollingLeft == true || rollingRight == true) && hangTimer > hangDelay)
            {
                lookTime += Time.deltaTime;

                if (gunMode == false)
                {
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

                // Above = Holstered Walk
                // Below = Gun Walk 

                else
                {
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

            }
                // Manages running speed, direction and delay before running
        }


            /* Jump Mechanic Limits */
            if (crouchOn == true || gunMode == true)
            {

            }

            // Above = Jump Disabled
            // Below = Jump Enabled

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
                        // Dictates the jump height.

                        else
                        {
                            //if (airTime > jumpDelay)
                            //{
                            airTime = 0;
                            inJump = false;
                        //}
                    }
                    // Acts as cooldown for after the jump.
                    // This step may not be neccessary.
                }


                    /* Running Jump Test */
                    else
                    {
                        if (airTime < jumpDuration)
                        {
                            if (runTime >   rJumpRunUp)
                            {
                                //rb.AddForce(new Vector3(rJumpDirection, rJumpHeight) * rJumpSpeed);
                                // rJumpDirection value = propulsion -> approx. = 0.075
                                // rJumpHeigh value approx. = 0.075

                                rb.velocity = new Vector3(rJumpDirection, rJumpHeight, 0);

                                // rJumpDirection value approx. = 
                                // rJumpHeight value approx. = 
                            }

                            // Above = Running Jump.
                            // Below = Leap Jump.

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
        
        ledgeCollided = ledgeCollider.GetComponent<LedgeCollision>().collided;
        // Determines ledge that has been grabbed.


        Vector3 down = transform.TransformDirection(Vector3.down) * 1.2f;
        Vector3 frontDown = new Vector3(transform.position.x + 0.38f, transform.position.y, transform.position.z);
        Vector3 backDown = new Vector3(transform.position.x - 0.38f, transform.position.y, transform.position.z);
        Vector3 right = transform.TransformDirection(Vector3.right) * 15f;
        Vector3 left = transform.TransformDirection(Vector3.left) * 15f;

        bool rayIntersectsSomething = Physics.Raycast(transform.position, down, 1.2f) || Physics.Raycast(frontDown, down, 1.2f) || Physics.Raycast(backDown, down, 1.2f);

        Debug.DrawRay(transform.position, down, Color.green);
        Debug.DrawRay(frontDown, down, Color.red);
        Debug.DrawRay(backDown, down, Color.cyan);
        // Raycasts bottom of player to detect floor.

        if (lookLeft == false)
        {
            Debug.DrawRay(playerNose.transform.position, right, Color.blue);
        }
        if (lookLeft == true)
        {
            Debug.DrawRay(playerNose.transform.position, left, Color.green);
        }


        if (rayIntersectsSomething)
        {
            midAir = false;
        }
        
        // Above = Floor Detected = On The Ground
        // Below = Floor Not Detected = In Mid Air

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

        hangTimer += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "LedgeCollider")
        {
            //ledgeUnderneath = collision.gameObject;

            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
            // Removes Collisions Between Player And Ledge.
            // Basically, climbing is mimicked through the use of an animator and timers. The ledge that is being grabbed is in itself a physical object with a collider.
            // This is because the ledge detector on the player is a trigger and requires a collidable source for input.
            // When ordered, the ledge will animate above the platform for where the ledge is assigned.
            // As it animates moving back to its default position, the ledge must pass the player that it was carrying, thus the collision ignore.
        }

        impactSpeed = collision.relativeVelocity;

        //if (collision.gameObject.tag == "Floors")
        //{
        if (impactSpeed.y > 0.1)
        {
            if (gunMode == true)
            {
                crouchOn = !crouchOn;
                gunTimer = 0;
                gunAim = true;
            }
        } 

        //else if (impactSpeed.x == 0)
        //{
        //    if (gunMode == true)
        //    {
        //        crouchOn = !crouchOn;
        //    }
        //}
        /* Dirty fix for crouched drops not altering stance. Doesn't feel like it's guaranteed to work. */

        //}
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "LedgeFootDetector" && gunMode == false)
        {
            ledgeUnderneath = other.gameObject;

            if (hangTimer > hangDelay)
            {
                if (other.gameObject.name == "LFootDetection" && lookLeft == false && Input.GetKeyDown(KeyCode.S))
                {
                    //ledgeUnderneath.GetComponent<Animator>().SetTrigger("climbRight");
                    hangTimer = 0;
                    ledgeUnderneath.GetComponentInParent<Animator>().SetTrigger("hangRight");
                }
                else if (other.gameObject.name == "RFootDetection" && lookLeft == true && Input.GetKeyDown(KeyCode.S))
                {
                    //ledgeUnderneath.GetComponent<Animator>().SetTrigger("climbRight");
                    hangTimer = 0;
                    ledgeUnderneath.GetComponentInParent<Animator>().SetTrigger("hangLeft");
                }
            }

            //if (climbing == false && Input.GetKeyDown(KeyCode.S))
            //{
            //    if (other.gameObject.name == "LFootDetection")
            //    {
            //        ledgeCo
            //    }

            //    else if (other.gameObject.name == "RFootDetection")
            //    {

            //    }

        }
    }

    private void OnTriggerExit(Collider other)
    {
            ledgeUnderneath = emptyLedge;
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
