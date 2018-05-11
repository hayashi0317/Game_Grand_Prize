using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koudai3D_Move : MonoBehaviour
{
    private float fPosX = 0;　//X軸移動量
    private float fPosY = 0;  //Y軸移動量
    private float fPosZ = 0;  //Z軸移動量
    private float fLimit_y = -20.0f;
    private bool bLockOn = false; //ロックオン判定
    private int bhorizon = 0;
    private int bfront = 0;
    private int nCnt = 0;
    public GameObject ball;

    Transform target;
    Vector3 targetPositon;

    // Use this for initialization
    void Start()
    {
        bhorizon = 1;
        bfront = 0;
        target = GameObject.Find("judge3").transform;
        //int rand;
        //rand = Random.Range();
    }

    void Update()
    {
        //------------------------------------------------------------------------
        //移動方向の判定と移動量
        //------------------------------------------------------------------------
        if (bLockOn == false)
        {
            switch (bhorizon)
            {
                case 1:
                    transform.position += new Vector3(Time.deltaTime * 5.0f, 0, 0);
                    break;
                case 2:
                    transform.position += new Vector3(Time.deltaTime * -5.0f, 0, 0);
                    break;
            }
            switch (bfront)
            {
                case 1:
                    transform.position += new Vector3(0, 0, Time.deltaTime * 5.0f);
                    break;
                case 2:
                    transform.position += new Vector3(0, 0, Time.deltaTime * -5.0f);
                    break;
            }
        }
        //------------------------------------------------------------------------
        //ターゲット追尾中処理
        //------------------------------------------------------------------------
        if (bLockOn == true)
        {
            float DifferencePosX = gameObject.transform.position.x - ball.transform.position.x;
            Debug.Log("LockOn ->" + ball.gameObject.name);
            if (DifferencePosX >= 0)
            {
                fPosX = Time.deltaTime * -2.0f;
            }
            else
            {
                fPosX = Time.deltaTime * 2.0f;
            }
            float DifferencePosZ = gameObject.transform.position.z - ball.transform.position.z;
            if (DifferencePosZ >= 0)
            {
                fPosZ = Time.deltaTime * -2.0f;
            }
            else
            {
                fPosZ = Time.deltaTime * 2.0f;
            }
            transform.Translate(fPosX, fPosY, fPosZ);
        }



        //------------------------------------------------------------------------------------
        //Quaternion.Slerpと併用して、指定したオブジェクトの方向になめらかに回転する
        //------------------------------------------------------------------------------------
        targetPositon = target.position;
        // 高さがずれていると体ごと上下を向いてしまうので便宜的に高さを統一
        if (transform.position.y != target.position.y)
        {
            targetPositon = new Vector3(target.position.x, transform.position.y, target.position.z);
        }
        Quaternion targetRotation = Quaternion.LookRotation(targetPositon - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);//Time.deltaTime*(int)で回転速度を調整する
        //------------------------------------------------------------------------
        //指定した方向に一気に回転する
        //------------------------------------------------------------------------
        //Vector3 relativePos = target.position - transform.position;
        //relativePos.y = 0; //上下方向の回転はしないように制御
        //transform.rotation = Quaternion.LookRotation(relativePos);
        //bRotation = false;
    }
    //-----------------------------------------------------------------------------------------
    //                   GetCollision
    //-----------------------------------------------------------------------------------------
    // 衝突した瞬間を検出
    void OnCollisionEnter(Collision other)
    {
        //------------------------------------------------------------------------------

    }
    // 衝突したオブジェクトが離れる瞬間を検出
    void OnCollisionExit(Collision other)
    {

    }

    // 衝突中を検出
    void OnCollisionStay(Collision other)
    {

        if (bLockOn == true)
        {
            if (other.gameObject.tag == "ball")
            {
                nCnt += 1;
                if (nCnt >= 150)
                {
                    Destroy(other.gameObject);
                    nCnt = 0;
                    bLockOn = false;
                }
            }
        }
    }

    //-----------------------------------------------------------------------------------------
    //          GetTrigger
    //-----------------------------------------------------------------------------------------
    //トリガーに接触した瞬間
    void OnTriggerEnter(Collider other)
    {
        if (bLockOn == false)
        {
            if (other.gameObject.tag == "ball")
            {
                ball = other.gameObject;
                bLockOn = true;
            }
        }

        if (other.gameObject.name == "judge")
        {
            int rand;
            rand = Random.Range(0, 2 + 1); //int : 第二引数は含まない->+1で調整
            if (rand == 0)
            {

            }
            if (rand == 1)
            {

                target = GameObject.Find("judge3").transform;
                bhorizon = 1; bfront = 0;
            }
            if (rand == 2)
            {

                target = GameObject.Find("judge8").transform;
                bhorizon = 0; bfront = 1;
            }
        }
        if (other.gameObject.name == "judge2")
        {
            int rand;
            rand = Random.Range(0, 1 + 1);
            if (rand == 0)
            {
                target = GameObject.Find("judge").transform;
                bfront = 0;
                bhorizon = 1;
            }
            if (rand == 1)
            {
                target = GameObject.Find("judge4").transform;
                bfront = 1;
                bhorizon = 0;
            }
        }
        if (other.gameObject.name == "judge3")
        {
            int rand;
            rand = Random.Range(0, 1 + 1);
            if (rand == 0)
            {
                target = GameObject.Find("judge").transform;
                bfront = 0;
                bhorizon = 2;
            }
            if (rand == 1)
            {
                target = GameObject.Find("judge6").transform;
                bfront = 1;
                bhorizon = 0;
            }
        }
        if (other.gameObject.name == "judge4")
        {
            int rand;
            rand = Random.Range(0, 2 + 1);
            if (rand == 0)
            {
                target = GameObject.Find("judge8").transform;
                bfront = 0;
                bhorizon = 1;
            }
            if (rand == 1)
            {
                target = GameObject.Find("judge5").transform;
                bfront = 1;
                bhorizon = 0;
            }
            if (rand == 2)
            {
                target = GameObject.Find("judge2").transform;
                bfront = 2;
                bhorizon = 0;
            }
        }
        if (other.gameObject.name == "judge5")
        {
            int rand;
            rand = Random.Range(0, 1 + 1);
            if (rand == 0)
            {
                target = GameObject.Find("judge4").transform;
                bfront = 2;
                bhorizon = 0;
            }
            if (rand == 1)
            {
                target = GameObject.Find("judge9").transform;
                bfront = 0;
                bhorizon = 1;
            }
        }
        if (other.gameObject.name == "judge6")
        {
            int rand;
            rand = Random.Range(0, 2 + 1);
            if (rand == 0)
            {
                target = GameObject.Find("judge8").transform;
                bfront = 0;
                bhorizon = 2;
            }
            if (rand == 1)
            {
                target = GameObject.Find("judge7").transform;
                bfront = 1;
                bhorizon = 0;
            }
            if (rand == 2)
            {
                target = GameObject.Find("judge3").transform;
                bfront = 2;
                bhorizon = 0;
            }
        }
        if (other.gameObject.name == "judge7")
        {
            int rand;
            rand = Random.Range(0, 1 + 1);
            if (rand == 0)
            {
                target = GameObject.Find("judge6").transform;
                bfront = 2;
                bhorizon = 0;
            }
            if (rand == 1)
            {
                target = GameObject.Find("judge9").transform;
                bfront = 0;
                bhorizon = 2;
            }
        }
        if (other.gameObject.name == "judge8")
        {
            int rand;
            rand = Random.Range(0, 3 + 1);
            if (rand == 0)
            {
                target = GameObject.Find("judge").transform;
                bfront = 2;
                bhorizon = 0;
            }
            if (rand == 1)
            {
                target = GameObject.Find("judge9").transform;
                bfront = 1;
                bhorizon = 0;
            }
            if (rand == 2)
            {
                target = GameObject.Find("judge6").transform;
                bfront = 0;
                bhorizon = 1;
            }
            if (rand == 3)
            {
                target = GameObject.Find("judge4").transform;
                bfront = 0;
                bhorizon = 2;
            }
        }
        if (other.gameObject.name == "judge9")
        {
            int rand;
            rand = Random.Range(0, 2 + 1);
            if (rand == 0)
            {
                target = GameObject.Find("judge7").transform;
                bfront = 0;
                bhorizon = 1;
            }
            if (rand == 1)
            {
                target = GameObject.Find("judge5").transform;
                bfront = 0;
                bhorizon = 2;
            }
            if (rand == 2)
            {
                target = GameObject.Find("judge8").transform;
                bfront = 2;
                bhorizon = 0;
            }
        }

    }

    //トリガーと離れた瞬間
    void OnTriggerExit(Collider other)
    {

    }

    //トリガーと接触してる間
    void OnTriggerStay(Collider other)
    {

    }
}
