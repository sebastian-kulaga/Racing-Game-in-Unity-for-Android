using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trigger : MonoBehaviour {
    public bool finished=true;
    public bool gracz = false;
    public bool ai = false;
    // Use this for initialization


    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        finished = true;
        //Debug.Log("Wygrałeś!");
        if (other.CompareTag("Player"))
        {
            gracz = true;
            //OnGUI();
        }
        if (other.CompareTag("Enemy"))
        {
            ai = true;
            //OnGUI();
        }
        
    }
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 70;
        style.normal.textColor = Color.black;
        if (finished)
        {
            if (gracz == true)
            {
                //Debug.Log("Wygrałeś!");
                GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 20, 100, 20), "Wygrałeś!", style);
            }
            if (ai == true)
            {
                GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 20, 100, 20), "Przegrałeś!", style);
            }

            if(GUI.Button(new Rect(Screen.width / 2 , Screen.height / 2 +100, 100, 60), "Restart Game"))
            {
                SceneManager.LoadScene("gra");
            }
        }
    }
}
