using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textJudg : MonoBehaviour {
    Text myText;
    GameObject obj1;

    // Use this for initialization
    void Start () {
        myText = GetComponentInChildren<Text>();

		obj1 = GameObject.Find("MainCamera");
	}

    // Update is called once per frame
    void Update()
    {

        CameraController J1 = obj1.GetComponent<CameraController>();
        bool TextJudg1 = J1.Is_Judg1();
        bool TextJudg2 = J1.Is_Judg2();

        if (TextJudg1 && TextJudg2)
        {
            myText.text = "●";
            myText.color = new Color(255,0,0);
        }
        else
        {
            myText.text = "○";
            myText.color = new Color(0, 0, 255);
        }
        
    }
}
