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
    public bool crouchOn;
    bool crouchToggle;
    bool gunMode;
    bool gunAim;
    public bool inJump;
    public bool midAir;
    public bool ledgeCollided;
    public bool ledgeHang;

    [Header("Damage Grace Values")]
    public bool gracePeriod;
    public bool invincible;
    public float graceTimer;
    public float graceDelay;

    [Header("Gun Values")]
    public float gunTimer;
    public float gunDelay;
    public float shotDelay;

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

    [Header("Audio Timers")]
    public float movementTimer;
    public float runSoundDelay;
    public float walkSoundDelay;

    [Header("Audio Sources")]
    public AudioSource playerAudioSource;
    public AudioClip[] gunShot;
    public AudioClip[] footStep;
    public AudioClip landing;


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
        // If gracePeriod is true, the player will be invincible for the duration of the grace period delay.
        if (gracePeriod == true)
        {
            invincible = true;

            graceTimer += Time.deltaTime;
            if (graceTimer > graceDelay)
            {
                gracePeriod = false;
                invincible = false;
                graceTimer = 0;
            }
        }

        // Detects which ledge in the map was grabbed and, depending on the player orientation, snaps the player to the ledge.
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
        // Also makes the rigidbody of the player a kinematic to keep the player in place.

        // If the player hasn't grabbed onto a ledge, the player is affected by gravity.
        else
        {
            ledgeHang = false;
            GetComponent<Rigidbody>().isKinematic = false;
        }

        // Controls when a player do gun-related actions.
        gunTimer += Time.deltaTime;
        if (rb.velocity.x == 0 && rb.velocity.y == 0 && rb.velocity.z == 0 && gunTimer > gunDelay)
        {
            /* Gun Mode/Aim ON/OFF */
            if (Input.GetKey(KeyCode.Tab))
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

            // Controls switching between gun out and gun aim stances.
            if (Input.GetKey(KeyCode.Space) && gunMode == true)
            {
                if (gunAim == false)
                {
                    gunAim = true;
                    gunTimer = 0;

                }
            }

            // Disables gun aim when attempting to move.
            if (gunAim == true && ((lookLeft == true && Input.GetKey(KeyCode.A) || lookLeft == false && Input.GetKey(KeyCode.D))))
            {
                gunAim = false;
                gunTimer = 0;
            }

            // Restarts gun timer to add animation delay to actions placed by the player.
            if (gunMode == true && ((lookLeft == false && Input.GetKey(KeyCode.A) || lookLeft == true && Input.GetKey(KeyCode.D))))
            {
                gunTimer = 0;
            }
        }

        // Controls player nose colour for debugging purposes.
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

        // Controls player hang drop. Position of drop depends on whether the player looks left or right.
        // Disables kinematic mode to allow player to move after hanging.
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

            // Controls player climb. Animation is picked depending on player look direction.
            if (Input.GetKey(KeyCode.W))
            {
                if (hangTimer > climbDelay)
                {
                    climbing = true;
                    ledgeTimer = 0;
                    hangTimer = 0;
                    GetComponent<Rigidbody>().isKinematic = false;
                    ledgeCollided = false;
                    ledgeHang = false;

                    if (lookLeft == false)
                    {
                        ledgeGrabbed.GetComponent<Animator>().SetTrigger("climbRight");
                    }
                    if (lookLeft == true)
                    {
                        ledgeGrabbed.GetComponent<Animator>().SetTrigger("climbLeft");
                    }
                }
            }

            // Changes the player prefab parent to empty game ledge (an EGO within the scene). This allows the player to let go of the current ledge when climbing up.
            // There's probably a better way of doing this.
            if (climbTimer >= climbDelay)
            {
                gameObject.transform.parent = emptyLedge.transform;
            }

            // Timer used to "time" when the player lets go of the ledge.
            // There's probably a better way of doing this too.
            if (climbing == true)
            {
                climbTimer += Time.deltaTime;
            }
        }

        else
        {
            // Resets the ledge timer while not attached to ledge.
            ledgeTimer = 0;

            /* Look Direction */
            if (midAir == false)
            {
                // Ensures that climbing is set to false when the player is not climbing/not in mid air.
                climbing = false;
                climbTimer = 0;

                // If else statement that controls player look direction.
                if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) == false)
                {
                    lookLeft = true;
                }
                else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A) == false)
                {
                    lookLeft = false;
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
                    if (Input.GetKey(KeyCode.C))
                    {
                        // Temporary fix for instant crouch/uncrouch on button hold.
                        if (lookTime > walkTurnDelay)
                        {
                            crouchOn = !crouchOn;
                            lookTime = 0;
                        }
                    }
                }
            }


            /* Crouch/Standing Scales */
            if (crouchOn == true)
            {
                // Allows the lookTime timer to work while in crouch mode.
                lookTime += Time.deltaTime;

                //The rest of this if else statement controls the scale and position of the player while crouching and uncrouching.
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


            /* Jump Mechanics*/
            /* Stationary Jump */
            // Sets running jump direction to 0 incase of reasons.
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == false))
            {
                rJumpDirection = 0;
            }

            /* Running Jump Direction */
            else
            {
                // Sets jump direction based on look direction.
                if (lookLeft == true)
                {
                    rJumpDirection = -rJumpPropulsion;
                }
                if (lookLeft == false)
                {
                    rJumpDirection = rJumpPropulsion;
                }
            }
            // Values set in Inspector.

            /* Jump button */
            // Allows the player to jump under the conditions that they are not currently in midair, with the gun out and not crouched.
            if (midAir == false && gunMode == false)
            {
                if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) && crouchOn == false)
                {
                    midAir = true;
                    inJump = true;
                }
            }

            /* Leap Jump Direction */
            // Sets leap direction based on look direction.
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
                // Rolling mechanics when debug bool is false. The current way players roll.
                if (debug == false)
                {
                    if (crouchOn == true)
                    {
                        if (lookLeft == true)
                        {
                            if (Input.GetKey(KeyCode.D))
                            {
                                lookTime = 0;
                            }

                            else if (Input.GetKey(KeyCode.A))
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
                            if (Input.GetKey(KeyCode.A))
                            {
                                lookTime = 0;
                            }

                            else if (Input.GetKey(KeyCode.D))
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

                // Rolling mechanics when debug bool is true. This is used as a toggle to test more refined ways of rolling.
                if (debug == true)
                {
                    if (crouchOn == true)
                    {
                        if (Input.GetKeyDown(KeyCode.A))
                        {
                            if (lookLeft == true)
                            {
                                timeRolling = 0;
                                rollingLeft = true;
                            }
                            else
                            {
                                lookTime = 0;
                            }
                        }

                        if (Input.GetKeyDown(KeyCode.D))
                        {
                            if (lookLeft == false)
                            {
                                timeRolling = 0;
                                rollingRight = true;
                            }
                            else
                            {
                                lookTime = 0;
                            }
                        }
                    }
                }

            }

            // Sets the value of look time to 0 while the player is rolling. Looktime is a value used to add delays to player movement/animations.
            if (rollingLeft == true || rollingRight == true)
            {
                lookTime = 0;
            }

            timeRolling += Time.deltaTime;
            /* Rolling movement */
            if (rollingRight == true)
            {
                // Momentum added to the player while they roll right off of edges. Speeds up fall time to make falling less floaty.
                if (midAir == true)
                {
                    rb.AddForce(new Vector3(-rollFallMomentumX, -rollFallMomentumY, 0));
                }

                // Else statement controls rolling direction and speed.
                else
                {
                    if (timeRolling < rollDuration)
                    {
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
                // Momentum added to the player while they roll left off of edges.
                if (midAir == true)
                {
                    rb.AddForce(new Vector3(rollFallMomentumX, -rollFallMomentumY, 0));
                }

                // Else statement controls rolling direction and speed.
                else
                {
                    if (timeRolling < rollDuration)
                    {
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
            // If statement allows basic movement so long as the player isn't crouched, in mid air, aiming their gun or rolling. Gun delay is added to account for gun animations.
            if (crouchOn == false && midAir == false && gunAim == false && gunTimer > gunDelay || rollingLeft == true || rollingRight == true)
            {
                // Resets running timer for when a player decides to walk after a run.
                if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftShift) || (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) == true)
                {
                    runTime = 0;
                }

                /* Running */
                // Disables running if gun mode is on.
                if (gunMode == false)
                {
                    // runningOn is activated earlier in the script via Input.GetKey(KeyCode.LeftShift)
                    if (runningOn == true)
                    {
                        lookTime += Time.deltaTime;

                        if (lookTime > runTurnDelay)
                        {
                            // Running movement for left and right keys
                            if (Input.GetKey(KeyCode.D))
                            {
                                movementTimer += Time.deltaTime;
                                if (movementTimer > runSoundDelay)
                                {
                                    int i = Random.Range(0, 2);

                                    AudioSource.PlayClipAtPoint(footStep[i], transform.position);
                                    movementTimer = 0;
                                }

                                transform.Translate(Vector3.right * Time.deltaTime * runningSpeed);
                                rb.velocity = new Vector3(2f, -2f, 0);
                                runTime += Time.deltaTime;
                            }
                            if (Input.GetKey(KeyCode.A))
                            {
                                movementTimer += Time.deltaTime;
                                if (movementTimer > runSoundDelay)
                                {
                                    int i = Random.Range(0, 2);

                                    AudioSource.PlayClipAtPoint(footStep[i], transform.position);
                                    movementTimer = 0;
                                }

                                transform.Translate(Vector3.left * Time.deltaTime * runningSpeed);
                                rb.velocity = new Vector3(-2f, -2f, 0);
                                runTime += Time.deltaTime;
                            }

                            // Resets looktime for movement delay/animations
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
                        if (Input.GetKey(KeyCode.A) && runningOn == false)
                        {
                            transform.Translate(Vector3.left * Time.deltaTime);
                            //rb.AddForce(Vector3.left * walkSpeed);

                            movementTimer += Time.deltaTime;
                            if (movementTimer > walkSoundDelay)
                            {
                                int i = Random.Range(0, 2);

                                AudioSource.PlayClipAtPoint(footStep[i], transform.position);
                                movementTimer = 0;
                            }
                        }

                        if (Input.GetKey(KeyCode.D) && runningOn == false)
                        {
                            transform.Translate(Vector3.right * Time.deltaTime);
                            //rb.AddForce(Vector3.right * walkSpeed);

                            movementTimer += Time.deltaTime;
                            if (movementTimer > walkSoundDelay)
                            {
                                int i = Random.Range(0, 2);

                                AudioSource.PlayClipAtPoint(footStep[i], transform.position);
                                movementTimer = 0;
                            }
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
                            movementTimer += Time.deltaTime;
                            if (movementTimer > walkSoundDelay)
                            {
                                int i = Random.Range(0, 2);

                                AudioSource.PlayClipAtPoint(footStep[i], transform.position);
                                movementTimer = 0;
                            }

                            transform.Translate(Vector3.left * Time.deltaTime);
                            //rb.AddForce(Vector3.left * walkSpeed);
                        }

                        if (Input.GetKey(KeyCode.D))
                        {
                            movementTimer += Time.deltaTime;
                            if (movementTimer > walkSoundDelay)
                            {
                                int i = Random.Range(0, 2);

                                AudioSource.PlayClipAtPoint(footStep[i], transform.position);
                                movementTimer = 0;
                            }

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
                midAir = true;

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
                        if (runTime > rJumpRunUp)
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
        Vector3 frontDown = new Vector3(transform.position.x + 0.4f, transform.position.y, transform.position.z);
        Vector3 backDown = new Vector3(transform.position.x - 0.4f, transform.position.y, transform.position.z);
        Vector3 gunHeight = new Vector3(playerNose.transform.position.x, transform.position.y + 0.5f, transform.position.z);

        Vector3 right = transform.TransformDirection(Vector3.right) * 15f;
        Vector3 left = transform.TransformDirection(Vector3.left) * 15f;

        bool gunLineOfSight = lookLeft == true ? Physics.Raycast(transform.position, left, 15f) : Physics.Raycast(transform.position, right, 15f);

        Debug.DrawRay(transform.position, down * 0.9f, Color.green);
        Debug.DrawRay(frontDown, down * 0.9f, Color.red);
        Debug.DrawRay(backDown, down * 0.9f, Color.cyan);
        // Raycasts bottom of player to detect floor.

        if (lookLeft == false)
        {
            Debug.DrawRay(gunHeight, right, Color.blue);
        }
        if (lookLeft == true)
        {
            Debug.DrawRay(gunHeight, left, Color.green);
        }
     
        bool feetRay = Physics.Raycast(transform.position, down, 1.2f) || Physics.Raycast(frontDown, down, 1.2f) || Physics.Raycast(backDown, down, 1.2f);

        //bool feetRay = Physics.Raycast(transform.position, down, out feetHit);
        //bool feetRayFront = Physics.Raycast(transform.position, frontDown, out feetHitFront);
        //bool feetRayBack = Physics.Raycast(transform.position, backDown, out feetHitBack);

        RaycastHit feetHit;
        Physics.Raycast(transform.position, down * 1.2f, out feetHit);

        RaycastHit feetHitFront;
        Physics.Raycast(transform.position, frontDown * 1.2f, out feetHitFront);

        RaycastHit feetHitBack;
        Physics.Raycast(transform.position, backDown * 1.2f, out feetHitBack);

        if (feetRay)
        {
            if (midAir == true)
            {
                if (inJump == false && (feetHit.transform.tag == "Floor" || feetHitFront.transform.tag == "Floor" || feetHitBack.transform.tag == "Floor"))
                {
                    AudioSource.PlayClipAtPoint(landing, transform.position);
                }
            }
            midAir = false;
        }

        // Above = Floor Detected = On The Ground
        // Below = Floor Not Detected = In Mid Air

        else
        {
            midAir = true;
        }

        if (gunLineOfSight)
        {

        }



        if (gunAim == true && Input.GetKey(KeyCode.Space))
        {
            if (shotDelay < gunTimer)
            {
                RaycastHit hit;
                if (Physics.Raycast(gameObject.transform.position, lookLeft == true ? left * 15f : right * 15f, out hit))
                {
                    Debug.Log(hit.transform.name);
                }

                int i = Random.Range(0, 2);

                Animator enemyAnimations;
                AudioSource.PlayClipAtPoint(gunShot[i], transform.position);


                // playerAudioSource.Play(0);

                if (hit.transform.tag == "Enemy" && hit.transform.GetComponent<EnemyRifleman>().enabled == true)
                {
                    enemyAnimations = hit.transform.gameObject.GetComponentInParent<Animator>();

                    hit.transform.GetComponent<EnemyRifleman>().enemyHurt.Play(0);

                    if (lookLeft == true)
                    {
                        enemyAnimations.SetTrigger("hitFromRight");
                    }
                    else
                    {
                        enemyAnimations.SetTrigger("hitFromLeft");
                    }

                    hit.transform.GetComponent<EnemyRifleman>().enabled = false;
                }

                if (hit.transform.tag == "Enemy" && hit.transform.GetComponent<EnemyCrawler>().enabled == true)
                {
                    enemyAnimations = hit.transform.gameObject.GetComponentInParent<Animator>();

                    hit.transform.GetComponent<EnemyCrawler>().enemyHurt.Play(0);

                    if (lookLeft == true)
                    {
                        enemyAnimations.SetTrigger("hitFromRight");
                    }
                    else
                    {
                        enemyAnimations.SetTrigger("hitFromLeft");
                    }

                    hit.transform.GetComponent<EnemyCrawler>().enabled = false;
                }

                gunTimer = 0;
            }
        }



        hangTimer += Time.deltaTime;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "LedgeCollider" || collision.gameObject.tag == "Enemy")
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
        if (impactSpeed.y > 1)
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

    // Controls lowering down ledges.
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "LedgeFootDetector" && gunMode == false)
        {
            ledgeUnderneath = other.gameObject;

            if (hangTimer > hangDelay)
            {
                if (other.gameObject.name == "LFootDetection" && lookLeft == false && Input.GetKeyDown(KeyCode.S))
                {
                    hangTimer = 0;
                    ledgeUnderneath.GetComponentInParent<Animator>().SetTrigger("hangRight");
                }
                else if (other.gameObject.name == "RFootDetection" && lookLeft == true && Input.GetKeyDown(KeyCode.S))
                {
                    hangTimer = 0;
                    ledgeUnderneath.GetComponentInParent<Animator>().SetTrigger("hangLeft");
                }
            }
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
