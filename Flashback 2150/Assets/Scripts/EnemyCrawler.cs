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

	// Use this for initialization
	void Start ()
    {
        alive = true;
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

        RaycastHit hit;
        if (Physics.Raycast(transform.position, right * 15f, out hit))
        {
            if (hit.transform.tag == "Player")
            {
                playerInvincible = hit.transform.GetComponent<MovementScript>().invincible;
                playerSeen = true;

                if (delayDuration < delayTimer)
                {
                    if (hit.distance > meleeDistance)
                    {
                        gameObject.transform.Translate(movementSpeed, 0, 0);
                    }
                }

                if (hit.distance < weaponActiveDistance)
                {
                    if (weaponOut == false)
                    {
                        delayTimer = 0;
                    }
                    weaponOut = true;
                }

                if (hit.distance > weaponActiveDistance)
                {
                    if (weaponOut == true)
                    {
                        delayTimer = 0;
                    }
                    weaponOut = false;
                }

                if (hit.distance <= meleeDistance)
                {
                    if (playerInvincible == false)
                    {
                        hit.rigidbody.velocity = new Vector3(hit.rigidbody.velocity.x + knockbackDistance, hit.rigidbody.velocity.y + knockbackHeight, hit.rigidbody.velocity.z);
                    }
                    hit.transform.GetComponent<MovementScript>().gracePeriod = true;
                }
            }

            else
            {
                playerSeen = false;
            }

            hit.distance = hitDistance;
        }

        if (Physics.Raycast(transform.position, left * 15f, out hit))
        {
            if (hit.transform.tag == "Player")
            {
                playerInvincible = hit.transform.GetComponent<MovementScript>().invincible;
                playerSeen = true;

                if (delayDuration < delayTimer)
                {
                    if (hit.distance > meleeDistance)
                    {
                        gameObject.transform.Translate(-movementSpeed, 0, 0);
                    }
                }

                if (hit.distance < weaponActiveDistance)
                {
                    if (weaponOut == false)
                    {
                        delayTimer = 0;
                    }
                    weaponOut = true;
                }

                if (hit.distance > weaponActiveDistance)
                {
                    if (weaponOut == true)
                    {
                        delayTimer = 0;
                    }
                    weaponOut = false;
                }

                if (hit.distance <= meleeDistance)
                {
                    //hit.transform.Translate(new Vector3(hit.transform.position.x + knockbackDistance, hit.transform.position.y + knockbackHeight, hit.transform.position.z));
                    if (playerInvincible == false)
                    {
                        hit.rigidbody.velocity = new Vector3(hit.rigidbody.velocity.x - knockbackDistance, hit.rigidbody.velocity.y + knockbackHeight, hit.rigidbody.velocity.z);
                    }
                    hit.transform.GetComponent<MovementScript>().gracePeriod = true;
                }
            }

            else
            {
                playerSeen = false;
            }

            hitDistance = hit.distance;
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

        if (hit.distance < weaponActiveDistance)
        {
            GetComponent<Animator>().SetTrigger("weaponOut");
        }
    }
}
