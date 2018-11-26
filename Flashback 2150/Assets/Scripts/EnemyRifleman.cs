using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRifleman : MonoBehaviour {

    public bool alive;
    public bool playerSeen;
    public bool weaponOut;
    public bool lookLeft;
    public bool crouchToggle;

    public GameObject enemyLook;

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

        delayTimer += Time.deltaTime;

        RaycastHit hit;
        if (Physics.Raycast(crouchGunHeight, right * 15f, out hit))
        {
            if (hit.collider.GetComponent<MovementScript>().crouchOn == false)
            {
                if (hit.transform.tag == "Player")
                {
                    transform.localScale = new Vector3(0.75f, 2, 0.75f);
                    playerSeen = true;
                    lookLeft = false;
                    crouchToggle = true;
                }
                else
                {
                    playerSeen = false;
                    crouchToggle = true;
                }
            }
            else
            {
                if (hit.transform.tag == "Player")
                {
                    transform.localScale = new Vector3(1.1f, 1f, 1.1f);
                    playerSeen = true;
                    lookLeft = false;

                    if (crouchToggle == true)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
                        crouchToggle = false;
                    }
                }
                else
                {
                    crouchToggle = true;
                }
            }
         }


        if (Physics.Raycast(crouchGunHeight, left * 15f, out hit))
        {

            if (hit.collider.GetComponent<MovementScript>().crouchOn == false)
            {
                if (hit.transform.tag == "Player")
                {
                    transform.localScale = new Vector3(0.75f, 2, 0.75f);
                    playerSeen = true;
                    lookLeft = true;
                    crouchToggle = true;
                }
                else
                {
                    playerSeen = false;
                    crouchToggle = true;
                }
            }
            else
            {
                if (hit.transform.tag == "Player")
                {
                    transform.localScale = new Vector3(1.1f, 1f, 1.1f);
                    playerSeen = true;
                    lookLeft = true;

                    if (crouchToggle == true)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
                        crouchToggle = false;
                    }
                }
                else
                {
                    crouchToggle = true;
                }
            }
        }
        

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
