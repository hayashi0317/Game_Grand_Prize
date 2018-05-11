using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CreatePhoto1 : MonoBehaviour {

    private int a = 0;

    // Use this for initialization
    void Start()
    {
        Texture2D tex = new Texture2D(1, 1);
        tex.LoadImage(LoadBin(Application.dataPath + "/screenshot1.png"));
        gameObject.GetComponent<Renderer>().material.mainTexture = tex;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Texture2D tex = new Texture2D(1, 1);
            tex.LoadImage(LoadBin(Application.dataPath + "/screenshot1.png"));
            gameObject.GetComponent<Renderer>().material.mainTexture = tex;

            StartCoroutine("Sample");
        }

    }

    byte[] LoadBin(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Open);
        BinaryReader br = new BinaryReader(fs);
        byte[] buf = br.ReadBytes((int)br.BaseStream.Length);
        return buf;
    }


    private IEnumerator Sample()
    {
        yield return new WaitForSeconds(2.0f);
    }

}
