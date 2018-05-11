using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultDirector : MonoBehaviour {

    GameObject fade;

    // Use this for initialization
    void Start () {
        fade = GameObject.Find("FadeDirector");
       
    }
	
	// Update is called once per frame
	void Update () {
        Fade();
    }

    public void Fade()
    {
        FadeManager fade_start = fade.GetComponent<FadeManager>();
        fade_start.enableFade = true;
        fade_start.enableFadeIn = true;
        fade_start.FadeIn(fade_start.FadeImage);
    }
}
