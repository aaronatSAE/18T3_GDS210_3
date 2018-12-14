using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRifleman : MonoBehaviour {

    public bool alive;
    public bool playerSeen;
    public bool playerStand;
    public bool weaponOut;
    public bool weaponAim;
    public bool lookLeft;
    public bool playerCrouch;
    public bool crouchOn;
    public bool crouchToggle;

    public bool playerRight;
    public bool playerLeft;
    public bool playerInvincible;
    public float knockbackHeight;
    public float knockbackDistance;

    public GameObject enemyLook;
    public GameObject nose;
    public Material noseMaterial;

    public float delayTimer;
    public float delayDuration;

    public float shootTimer;
    public float shootDelay;

    public AudioClip shootSound;
    public AudioSource enemyHurt;

    // Use this for initialization
    void Start ()
    {
        alive = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Directional Vector3s for setting raycast directions.
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 left = transform.TransformDirection(Vector3.left);

        // Height Vector3s for setting raycast heights.
        Vector3 gunHeight = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Vector3 crouchGunHeight = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z); 

        // Draw debugs for left and right, gun height and crouch gun height.
        Debug.DrawRay(gunHeight, right * 15f, Color.blue);
        Debug.DrawRay(gunHeight, left * 15f, Color.green);
        Debug.DrawRay(crouchGunHeight, right * 15f, Color.green);
        Debug.DrawRay(crouchGunHeight, left * 15f, Color.blue);

        // Bool for when raycasts are hit, not currently being used.
        bool gunLineOfSight = Physics.Raycast(gunHeight, right, 15f) || Physics.Raycast(gunHeight, left, 15f) || Physics.Raycast(crouchGunHeight, right, 15f) || Physics.Raycast(crouchGunHeight, left, 15f);

        noseMaterial = nose.GetComponent<Renderer>().material;

        // Raycast right and left variables.
        RaycastHit hitRight;
        RaycastHit hitLeft;

        // Raycast for when player is on the right.
        if (Physics.Raycast(crouchGunHeight, right * 15f, out hitRight))
        {
            // If the object hit by raycast is tagged "Player", several functions play out.
            if (hitRight.transform.tag == "Player")
            {
                // Confirms that the player is on the right. Referenced later in the code.
                playerRight = true;

                // Detects if the player is crouching, and toggles enemy crouch to match.
                if (hitRight.transform.GetComponent<MovementScript>().crouchOn == true)
                {
                    crouchOn = true;
                }
                else
                {
                    crouchOn = false;
                }
            }
            // If the object hit by raycast isn't tagged "Player", player right is false.
            else
            {
                playerRight = false;
            }
        }

        // Raycast for when player is on the left.
        if (Physics.Raycast(crouchGunHeight, left * 15f, out hitLeft))
        {
            // If the object hit by raycast is tagged "Player", several functions play out.
            if (hitLeft.transform.tag == "Player")
            {
                // Confirms that the player is on the left. Referenced later in the code.
                playerLeft = true;

                // Detects if the player is crouching, and toggles enemy crouch to match.
                if (hitLeft.transform.GetComponent<MovementScript>().crouchOn == true)
                {
                    crouchOn = true;
                }
                else
                {
                    crouchOn = false;
                }
            } 
            // If the object hit by raycast isn't tagged "Player", player left is false
            else
            {
                playerLeft = false;
            }
        }

        // Checks whether the player is seen by the enemy.
        if (hitLeft.transform.tag == "Player" || hitRight.transform.tag == "Player")
        {
            playerSeen = true;
        }
        else
        {
            playerSeen = false;
        }

        // Controls scale of the enemy for crouching, currently as placeholder for crouching animation.
        if (crouchOn == true)
        {
            if (weaponAim == true)
            {
                transform.localScale = new Vector3(1.1f, 1f, 1.1f);
                if (crouchToggle == false)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
                    crouchToggle = true;
                }
            } 
        }
        else
        {
            transform.localScale = new Vector3(0.75f, 2f, 0.75f);
            if (crouchToggle == true)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                crouchToggle = false;
            }
        }

        // Links identification of player location with enemy rotation.
        if (playerRight == true)
        {
            lookLeft = false;
        }
        if (playerLeft == true)
        {
            lookLeft = true;
        }


        delayTimer += Time.deltaTime;

        // Series of timers that control transitions between the enemy idle state, gun out and gun aim states.
        if (playerSeen == true)
        {
            GetComponent<Animator>().SetBool("gunPrime", true);

            if (delayTimer > delayDuration)
            {
                if (weaponOut == false)
                {
                    noseMaterial.color = Color.yellow;
                    delayTimer = 0;
                    weaponOut = true;
                }

                if (delayTimer > delayDuration)
                {
                    if (weaponOut == true)
                    {
                        if (weaponAim == false)
                        {
                            noseMaterial.color = Color.red;
                            delayTimer = 0;
                            weaponAim = true;
                        }
                    }
                }
            }
        }

        // Series of timers that reverses the above transitions. Gun aim > gun out > idle.
        if (playerSeen == false)
        {
            GetComponent<Animator>().SetBool("gunPrime", true);

            if (delayTimer > delayDuration)
            {
                if (weaponAim == true)
                {
                    noseMaterial.color = Color.yellow;
                    delayTimer = 0;
                    weaponAim = false;
                }

                if (delayTimer > delayDuration)
                {
                    if (weaponAim == false)
                    {
                        if (weaponOut == true)
                        {
                            noseMaterial.color = Color.green;
                            delayTimer = 0;
                            weaponOut = false;
                        }
                    }
                }
            }
        }

        // Controls player knockback and damage. In effect, the shooting function.
        if (weaponAim == true)
        {
            shootTimer += Time.deltaTime;


            if (shootTimer > shootDelay)
            {
                AudioSource.PlayClipAtPoint(shootSound, transform.position);

                GetComponent<Animator>().SetTrigger("gunFire");

                if (playerLeft == true)
                {
                    playerInvincible = hitLeft.transform.GetComponent<MovementScript>().invincible;
                    if (playerInvincible == false)
                    {
                        hitLeft.transform.GetComponent<PlayerHealth>().shield -= 1;
                        hitLeft.rigidbody.velocity = new Vector3(hitLeft.rigidbody.velocity.x - knockbackDistance, hitLeft.rigidbody.velocity.y + knockbackHeight, hitLeft.rigidbody.velocity.z);
                    }
                    hitLeft.transform.GetComponent<MovementScript>().gracePeriod = true;
                }

                if (playerRight == true)
                {
                    playerInvincible = hitRight.transform.GetComponent<MovementScript>().invincible;
                    if (playerInvincible == false)
                    {
                        hitRight.transform.GetComponent<PlayerHealth>().shield -= 1;
                        hitRight.rigidbody.velocity = new Vector3(hitRight.rigidbody.velocity.x + knockbackDistance, hitRight.rigidbody.velocity.y + knockbackHeight, hitRight.rigidbody.velocity.z);
                    }
                    hitRight.transform.GetComponent<MovementScript>().gracePeriod = true;
                }

                shootTimer = 0;
            }
        }
        else
        {
            shootTimer = 0;
        }


        // Was intended to animate when the player stops aiming at player. No use as of yet.
        if (playerSeen == false)
        {
            GetComponentInParent<Animator>().SetTrigger("playerGone");
        }

        // Controls bool in the enemyLook animator that controls placeholder left and right look animations.
        if (lookLeft == true)
        {
            enemyLook.GetComponent<Animator>().SetBool("left?", true);
            GetComponentInParent<Animator>().SetBool("left?", true);
        }
        else if (lookLeft == false)
        {
            enemyLook.GetComponent<Animator>().SetBool("left?", false);
            GetComponentInParent<Animator>().SetBool("left?", false);

        }
    }
}
