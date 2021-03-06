﻿using System.Collections;
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

    public AudioSource enemyHurt;
    public AudioClip zapNoise;
    public AudioClip enemyMoving;

    public float zapTimer;
    public float zapDelay;

    public float rSoundTimer;
    public float rSoundDelay;

    public GameObject animations;

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
        if (weaponOut == true)
        {
            animations.GetComponent<Animator>().SetBool("Unholstered", true);
        }
        else
        {
            animations.GetComponent<Animator>().SetBool("Unholstered", false);
        }

        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 left = transform.TransformDirection(Vector3.left);

        Vector3 detectionHeight = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

        Debug.DrawRay(detectionHeight, right * 15f, Color.blue);
        Debug.DrawRay(detectionHeight, left * 15f, Color.green);

        bool gunLineOfSight = Physics.Raycast(transform.position, left, 15f) || Physics.Raycast(transform.position, right, 15f);

        delayTimer += Time.deltaTime;

        RaycastHit hitLeft;
        RaycastHit hitRight;

        zapTimer += Time.deltaTime;
        rSoundTimer += Time.deltaTime;

        // Raycast checks left from enemy.
        if (Physics.Raycast(transform.position, left * 15f, out hitLeft))
        {
            // Checks if target hit has "Player" tag.
            if (hitLeft.transform.tag == "Player")
            {
                GetComponentInParent<Animator>().SetTrigger("turnLeft");
                GetComponentInParent<Animator>().ResetTrigger("turnRight");
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

                            if (rSoundTimer > rSoundDelay)
                            {
                                AudioSource.PlayClipAtPoint(enemyMoving, transform.position);
                                rSoundTimer = 0;
                            }
                        }
                    }
                }

                if (hitLeft.distance < weaponActiveDistance)
                {
                    if (weaponOut == false)
                    {
                        delayTimer = 0;
                    }
                    weaponOut = true;
                }

                if (hitLeft.distance > weaponActiveDistance)
                {
                    if (weaponOut == true)
                    {
                        delayTimer = 0;
                    }
                    weaponOut = false;
                }

                if (hitLeft.distance <= meleeDistance)
                {
                    //hit.transform.Translate(new Vector3(hit.transform.position.x + knockbackDistance, hit.transform.position.y + knockbackHeight, hit.transform.position.z));
                    if (zapTimer > zapDelay)
                    {
                        if (playerInvincible == false)
                        {
                            hitLeft.transform.GetComponent<PlayerHealth>().shield -= 1;
                            hitLeft.rigidbody.velocity = new Vector3(hitLeft.rigidbody.velocity.x - knockbackDistance, hitLeft.rigidbody.velocity.y + knockbackHeight, hitLeft.rigidbody.velocity.z);
                        }
                        hitLeft.transform.GetComponent<MovementScript>().gracePeriod = true;
                        zapTimer = 0;
                    }
                }
                hitDistance = hitLeft.distance;
            }
        }

        if (Physics.Raycast(transform.position, right * 15f, out hitRight))
        {
            if (hitRight.transform.tag == "Player")
            {
                GetComponentInParent<Animator>().ResetTrigger("turnLeft");
                GetComponentInParent<Animator>().SetTrigger("turnRight");

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

                            if (rSoundTimer > rSoundDelay)
                            {
                                AudioSource.PlayClipAtPoint(enemyMoving, transform.position);
                                rSoundTimer = 0;
                            }
                        }
                    }
                }

                if (hitRight.distance < weaponActiveDistance)
                {
                    if (weaponOut == false)
                    {
                        delayTimer = 0;
                    }
                    weaponOut = true;
                }

                if (hitRight.distance > weaponActiveDistance)
                {
                    if (weaponOut == true)
                    {
                        delayTimer = 0;
                    }
                    weaponOut = false;
                }

                if (hitRight.distance <= meleeDistance)
                {
                    if (zapTimer > zapDelay)
                    {
                        if (playerInvincible == false)
                        {
                            hitRight.transform.GetComponent<PlayerHealth>().shield -= 1;
                            hitRight.rigidbody.velocity = new Vector3(hitRight.rigidbody.velocity.x + knockbackDistance, hitRight.rigidbody.velocity.y + knockbackHeight, hitRight.rigidbody.velocity.z);
                            AudioSource.PlayClipAtPoint(zapNoise, transform.position);
                        }
                        hitRight.transform.GetComponent<MovementScript>().gracePeriod = true;
                        zapTimer = 0;
                    }
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
