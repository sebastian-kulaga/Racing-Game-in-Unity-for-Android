using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour {
    public bool paused;
    public GameObject controls;
	//Use this for initialization
	void Start () {
        paused = false;
        //controls.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
        controls.SetActive(true);
    }
    public void Pause()
    {

        
        paused = !paused;
        
        if (paused)
        {
            Time.timeScale = 0;
        }
        else if (!paused)
        {
            Time.timeScale = 1;
        }
    }
    public void Exit()
    {
        Application.Quit();
    }
}
