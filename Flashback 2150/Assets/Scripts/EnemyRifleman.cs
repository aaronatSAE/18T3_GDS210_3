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

    public GameObject enemyLook;
    public GameObject nose;
    public Material noseMaterial;

    public float delayTimer;
    public float delayDuration;

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
        Vector3 gunHeight = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Vector3 crouchGunHeight = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z); 

        Debug.DrawRay(gunHeight, right * 15f, Color.blue);
        Debug.DrawRay(gunHeight, left * 15f, Color.green);
        Debug.DrawRay(crouchGunHeight, right * 15f, Color.green);
        Debug.DrawRay(crouchGunHeight, left * 15f, Color.blue);

        bool gunLineOfSight = Physics.Raycast(gunHeight, right, 15f) || Physics.Raycast(gunHeight, left, 15f) || Physics.Raycast(crouchGunHeight, right, 15f) || Physics.Raycast(crouchGunHeight, left, 15f);

        noseMaterial = nose.GetComponent<Renderer>().material;

        RaycastHit hit;
        if (Physics.Raycast(crouchGunHeight, right * 15f, out hit))
        {
            if (hit.transform.tag == "Player")
            {
                playerRight = true;

                if (hit.transform.GetComponent<MovementScript>().crouchOn == true)
                {
                    crouchOn = true;
                }
                else
                {
                    crouchOn = false;
                }
            }
            else
            {
                playerRight = false;
            }
        }

        if (Physics.Raycast(crouchGunHeight, left * 15f, out hit))
        {
            if (hit.transform.tag == "Player")
            {
                playerLeft = true;
                if (hit.transform.GetComponent<MovementScript>().crouchOn == true)
                {
                    crouchOn = true;
                    delayTimer = 0;
                }
                else
                {
                    crouchOn = false;
                    delayTimer = 0;
                }
            }
            else
            {
                playerLeft = false;
            }
        }

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

        if (playerRight == true)
        {
            lookLeft = false;
        }

        if (playerLeft == true)
        {
            lookLeft = true;
        }

        delayTimer += Time.deltaTime;

        if (playerLeft == true || playerRight == true)
        {    
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

        if (playerLeft == false && playerRight == false)
        {
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

        //if (playerCrouch == true)
        //{
        //    transform.localScale = new Vector3(1.1f, 1f, 1.1f);
        //    // Crouching scale
        //}

        //else
        //{
        //    transform.localScale = new Vector3(0.75f, 2f, 0.75f);
        //    // Standing scale
        //}

        //if (Physics.Raycast(gunHeight, left * 15f, out hit))
        //{
        //        if (hit.transform.tag == "Player")
        //        {
        //            transform.localScale = new Vector3(0.75f, 2, 0.75f);
        //            playerSeen = true;
        //            lookLeft = true;
        //            playerCrouch = true;
        //        }
        //        else
        //        {
        //            playerSeen = false;
        //            playerCrouch = true;
        //        }
        //    else
        //    {
        //        if (hit.transform.tag == "Player")
        //        {
        //            transform.localScale = new Vector3(1.1f, 1f, 1.1f);
        //            playerSeen = true;
        //            lookLeft = true;

        //            if (playerCrouch == true)
        //            {
        //                transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        //                playerCrouch = false;
        //            }
        //        }
        //        else
        //        {
        //            playerCrouch = true;
        //        }
        //    }
        //}


        if (playerSeen == false)
        {
            GetComponent<Animator>().SetTrigger("playerGone");
        }

        if (lookLeft == true)
        {
            enemyLook.GetComponent<Animator>().SetBool("left?", true);
        }

        else if (lookLeft == false)
        {
            enemyLook.GetComponent<Animator>().SetBool("left?", false);
        }
    }
}
