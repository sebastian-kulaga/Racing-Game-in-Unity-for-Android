using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audio : MonoBehaviour {

    public Slider Volume;
    public AudioSource bgmusic;
	
	// Update is called once per frame
	void Update () {
        float vol = 0.5f;
        bgmusic.volume = Volume.value;
        vol = Volume.value;
        //ustawia przesyla wartosc slidera do nastepnej sceny
        PlayerPrefs.SetFloat("SliderValue", vol);
    }
}
