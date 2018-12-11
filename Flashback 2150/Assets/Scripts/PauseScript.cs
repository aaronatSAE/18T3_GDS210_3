using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour {

    public bool paused;
    public GameObject pauseUI;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused =! paused;
        }

        Time.timeScale = paused == true ? 0 : 1;
        pauseUI.SetActive(paused == true ? true : false);
	}
}
