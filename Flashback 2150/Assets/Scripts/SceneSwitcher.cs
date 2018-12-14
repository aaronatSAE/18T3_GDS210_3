using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void Level2()
    {
        SceneManager.LoadScene("Level 2", LoadSceneMode.Additive);
    }

    void Endgame()
    {
        SceneManager.LoadScene("End Game Scene", LoadSceneMode.Additive);
    }

    public void Play()
    {
        SceneManager.LoadScene("Level 1 Rotation Fix");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
