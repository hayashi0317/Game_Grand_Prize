using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class judgment : MonoBehaviour {

    public GameObject obj1;
    public GameObject obj2;
    public Collider obj1Collider;
    public Collider obj2Collider;
    private Camera cam;
    private Plane[] planes;
    public bool judg1;
    public bool judg2;
    public float[,] pos;

    // Use this for initialization
    void Start() {
        judg1 = false;
        judg2 = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Judgment()
    {
        cam = Camera.main;
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        obj1 = GameObject.Find("koudai3D");
        obj1Collider = obj1.GetComponent<Collider>();
        obj2 = GameObject.Find("boll");
        obj1Collider = obj1.GetComponent<Collider>();


        if (GeometryUtility.TestPlanesAABB(planes, obj1Collider.bounds))
        {
            judg1 = true;
        }
        else
        {
            judg1 = false;
        }

        if (GeometryUtility.TestPlanesAABB(planes, obj2Collider.bounds))
        {
            judg2 = true;
        }
        else
        {
            judg2 = false;
        }

        Debug.Log(judg1);
        Debug.Log(judg2);

    }

    public bool Is_Judg1()
    {
        return judg1;  
    }
    public bool Is_Judg2()
    {
        return judg2;
    }

}

