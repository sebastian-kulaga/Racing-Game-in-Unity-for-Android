using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public AudioSource audiosource;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
}

    public void ExitGame()
    {
        Debug.Log("Wychodze!");
        Application.Quit();
    }

    //public void SliderValue(float newValue)
    //{

    //    float newVol;
    //    newVol = newValue;
    //    audiosource.volume = newVol;
    //    //ustawia przesyla wartosc slidera do nastepnej sceny
    //   PlayerPrefs.SetFloat("SliderValue", newVol);
    //}
}
