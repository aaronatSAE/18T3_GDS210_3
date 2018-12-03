using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {

    public float shield;
    public string currentScene;

    public float deathTimer;
    public float deathDelay;

	// Use this for initialization
	void Start ()
    {
        shield = 5;
        currentScene = SceneManager.GetActiveScene().name;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (shield > 5)
        {
            shield = 5;
        }

        if (shield < 0)
        {
            deathTimer += Time.deltaTime;
            GetComponentInParent<Rigidbody>().isKinematic = true;
            GetComponentInParent<MovementScript>().enabled = false;
            if (deathTimer > deathDelay)
            {
                SceneManager.LoadScene(currentScene);
            }
        }
	}
}
