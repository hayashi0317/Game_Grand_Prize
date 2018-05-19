using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Judg_Text : MonoBehaviour {

    GameObject timer_camera;
    Text myText;

	// Use this for initialization
	void Start () {

        timer_camera = GameObject.Find("TimerCamera"); 
        myText = GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {

        TimerCameraController tcc = timer_camera.GetComponent<TimerCameraController>();
        if (tcc.target_judgment())
        { 
            myText.text = "○";
        }
        if (!tcc.target_judgment())
        {
            myText.text = "×";
        }
    }
}
