using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenShot : MonoBehaviour {

    private int screenShotCount = 0;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Space)) 
		{
            ScreenCapture.CaptureScreenshot("Assets/screenshot" + screenShotCount + ".png");
            screenShotCount++;
            if (screenShotCount >= 3)
            {
                screenShotCount = 0;
            }

            StartCoroutine("Sample");

		}
      Debug.Log("カウント" + screenShotCount);
    }

    private IEnumerator Sample() {
        yield return new WaitForSeconds(2.0f);
    }
}
