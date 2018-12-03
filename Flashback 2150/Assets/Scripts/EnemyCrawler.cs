using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrawler : MonoBehaviour {

    public float meleeDistance;
    public float weaponActiveDistance;
    public float movementSpeed;
    public bool alive;
    public bool playerSeen;
    public bool playerInvincible;
    public bool weaponOut;

    public float knockbackHeight;
    public float knockbackDistance;

    public float delayTimer;
    public float delayDuration;

    public float hitDistance;

    public bool ledgeDetected;

    public GameObject hitLeftGO;
    public GameObject hitRightGO;

    public string ledgeDirection;

	// Use this for initialization
	void Start ()
    {
        alive = true;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "LedgeFootDetector")
        {
            ledgeDetected = true;
            ledgeDirection = other.transform.name;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "LedgeFootDetector")
        {
            ledgeDetected = false;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 left = transform.TransformDirection(Vector3.left);

        Vector3 detectionHeight = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

        Debug.DrawRay(detectionHeight, right * 15f, Color.blue);
        Debug.DrawRay(detectionHeight, left * 15f, Color.green);

        bool gunLineOfSight = Physics.Raycast(transform.position, left, 15f) || Physics.Raycast(transform.position, right, 15f);

        delayTimer += Time.deltaTime;

        RaycastHit hitLeft;
        RaycastHit hitRight;

        if (Physics.Raycast(transform.position, left * 15f, out hitLeft))
        {
            if (hitLeft.transform.tag == "Player")
            {
                playerInvincible = hitLeft.transform.GetComponent<MovementScript>().invincible;
                playerSeen = true;

                if (delayDuration < delayTimer)
                {
                    if (hitLeft.distance > meleeDistance)
                    {
                        if (ledgeDetected == true && ledgeDirection == "LFootDetection")
                        {

                        }
                        else
                        {
                            gameObject.transform.Translate(-movementSpeed, 0, 0);
                        }
                    }
                }

                if (hitLeft.distance < weaponActiveDistance)
                {
                    if (weaponOut == false)
                    {
                        GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 1f);
                        GetComponent<BoxCollider>().center = new Vector3(0, 0, 0);
                        
                        delayTimer = 0;
                    }
                    weaponOut = true;
                }

                if (hitLeft.distance > weaponActiveDistance)
                {
                    if (weaponOut == true)
                    {
                        GetComponent<BoxCollider>().size = new Vector3(1f, 0.5f, 1f);
                        GetComponent<BoxCollider>().center = new Vector3(0, -0.25f, 0);
                        delayTimer = 0;
                    }
                    weaponOut = false;
                }

                if (hitLeft.distance <= meleeDistance)
                {
                    //hit.transform.Translate(new Vector3(hit.transform.position.x + knockbackDistance, hit.transform.position.y + knockbackHeight, hit.transform.position.z));
                    if (playerInvincible == false)
                    {
                        hitLeft.transform.GetComponent<PlayerHealth>().shield -= 1;
                        hitLeft.rigidbody.velocity = new Vector3(hitLeft.rigidbody.velocity.x - knockbackDistance, hitLeft.rigidbody.velocity.y + knockbackHeight, hitLeft.rigidbody.velocity.z);
                    }
                    hitLeft.transform.GetComponent<MovementScript>().gracePeriod = true;
                }
                hitDistance = hitLeft.distance;
            }
        }

        if (Physics.Raycast(transform.position, right * 15f, out hitRight))
        {
            if (hitRight.transform.tag == "Player")
            {
                playerInvincible = hitRight.transform.GetComponent<MovementScript>().invincible;
                playerSeen = true;

                if (delayDuration < delayTimer)
                {
                    if (hitRight.distance > meleeDistance)
                    {
                        if (ledgeDetected == true && ledgeDirection == "RFootDetection")
                        {

                        }
                        else
                        {
                            gameObject.transform.Translate( movementSpeed, 0, 0);
                        }
                    }
                }

                if (hitRight.distance < weaponActiveDistance)
                {
                    if (weaponOut == false)
                    {
                        GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 1f);
                        GetComponent<BoxCollider>().center = new Vector3(0, 0, 0);
                        delayTimer = 0;
                    }
                    weaponOut = true;
                }

                if (hitRight.distance > weaponActiveDistance)
                {
                    if (weaponOut == true)
                    {
                        GetComponent<BoxCollider>().size = new Vector3(1f, 0.5f, 1f);
                        GetComponent<BoxCollider>().center = new Vector3(0, -0.25f, 0);
                        delayTimer = 0;
                    }
                    weaponOut = false;
                }

                if (hitRight.distance <= meleeDistance)
                {
                    if (playerInvincible == false)
                    {
                        hitRight.transform.GetComponent<PlayerHealth>().shield -= 1;
                        hitRight.rigidbody.velocity = new Vector3(hitRight.rigidbody.velocity.x + knockbackDistance, hitRight.rigidbody.velocity.y + knockbackHeight, hitRight.rigidbody.velocity.z);
                    }
                    hitRight.transform.GetComponent<MovementScript>().gracePeriod = true;
                }
                hitDistance = hitRight.distance;
            }

            hitLeftGO = hitLeft.transform.gameObject;
            hitRightGO = hitRight.transform.gameObject;


            if ((hitRight.transform.tag == "Player") || (hitLeft.transform.tag == "Player"))
            {

            }

            else
            {
                playerSeen = false;
            }
        }

        if (playerSeen == false)
        {
            if (delayTimer > delayDuration)
            {
                weaponOut = false;
                delayTimer = 0;
            }
            GetComponent<Animator>().SetTrigger("playerGone");
        }

        if (hitRight.distance < weaponActiveDistance || hitLeft.distance < weaponActiveDistance)
        {
            GetComponent<Animator>().SetTrigger("weaponOut");
        }
    }

    
}
