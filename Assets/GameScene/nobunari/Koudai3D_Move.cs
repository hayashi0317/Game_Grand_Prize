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
    public int GetItem = 0;
    Transform target;
    Transform OldTatget;
    Vector3 targetPositon;
    GameObject TragetObjectObstacle;//障害ジャッジ用
    // Use this for initialization
    void Start()
    {
        target = GameObject.Find("judge3").transform;
    }

    void Update()
    {
        //------------------------------------------------------------------------
        //移動方向の判定と移動量
        //------------------------------------------------------------------------
        
        if (bLockOn == false)
        {
            float DifferencePosX = gameObject.transform.position.x - target.transform.position.x;
            float DifferencePosZ = gameObject.transform.position.z - target.transform.position.z;
            float MoveX = 0; float MoveZ = 0;

            if (DifferencePosX > 0.5f)
            { MoveX = Time.deltaTime * -2.0f; }
            else{MoveX = Time.deltaTime * 2.0f;}
            
            
            if (DifferencePosZ >= 0)
            { MoveZ = Time.deltaTime * -2.0f;}
            else{ MoveZ = Time.deltaTime * 2.0f;}
            transform.position += new Vector3(MoveX, 0, MoveZ);
        }
        //------------------------------------------------------------------------
        //ターゲット追尾中処理
        //------------------------------------------------------------------------
        if (bLockOn == true)
        {
            float DifferencePosX = gameObject.transform.position.x - target.transform.position.x;
            if (DifferencePosX >= 0)
            {
                transform.position += new Vector3(Time.deltaTime * -2.0f, 0, 0);
            }
            else
            {
                transform.position += new Vector3(Time.deltaTime * 2.0f, 0, 0);
            }
            float DifferencePosZ = gameObject.transform.position.z - target.transform.position.z;
            if (DifferencePosZ >= 0)
            {
                transform.position += new Vector3(0, 0, Time.deltaTime * -2.0f);
            }
            else
            {
                transform.position += new Vector3(0, 0, Time.deltaTime * 2.0f);
            }
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

        if(GetItem>=2)
        {
            Destroy(gameObject);
            //Application.LoadLevel();
        }
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
            if (other.gameObject.tag == "kanna")
            {
                nCnt += 1;
                if (nCnt >= 150)
                {
                    Destroy(other.gameObject);
                    nCnt = 0;
                    target = OldTatget;
                    bLockOn = false;
                    GetItem++;
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
        if(bLockOn==false)
        { 
            if (other.gameObject.name == "kanna")
            {
                ball = other.gameObject;
                bLockOn = true;
                OldTatget = target;
                target = GameObject.Find("kanna").transform;
            }
            if (other.gameObject.name == "kanna2")
            {
                ball = other.gameObject;
                bLockOn = true;
                OldTatget = target;
                target = GameObject.Find("kanna2").transform;
            }
            if (other.gameObject.name == "kanna3")
            {
                ball = other.gameObject;
                bLockOn = true;
                OldTatget = target;
                target = GameObject.Find("kanna3").transform;
            }
            if (other.gameObject.name == "kanna4")
            {
                ball = other.gameObject;
                bLockOn = true;
                OldTatget = target;
                target = GameObject.Find("kanna4").transform;
            }
        }
        if (other.gameObject.name == "judge")
        {
            int rand;
            rand = Random.Range(0, 2 + 1); //int : 第二引数は含まない->+1で調整
            if (rand == 0)
            {
                target = GameObject.Find("judge2").transform;
            }
            if (rand == 1)
            {
                target = GameObject.Find("judge3").transform;
            }
            if (rand == 2)
            {

                target = GameObject.Find("judge8").transform;
            }
        }
        if (other.gameObject.name == "judge2")
        {
            int rand;
            rand = Random.Range(0, 1 + 1);
            if (rand == 0)
            {
                target = GameObject.Find("judge").transform;
            }
            if (rand == 1)
            {
                target = GameObject.Find("judge4").transform;
            }
        }
        if (other.gameObject.name == "judge3")
        {
            int rand;
            rand = Random.Range(0, 1 + 1);
            if (rand == 0)
            {
                target = GameObject.Find("judge").transform;
            }
            if (rand == 1)
            {
                target = GameObject.Find("judge6").transform;
            }
        }
        if (other.gameObject.name == "judge4")
        {
            int rand;
            rand = Random.Range(0, 2 + 1);
            if (rand == 0)
            {
                target = GameObject.Find("judge8").transform;
            }
            if (rand == 1)
            {
                target = GameObject.Find("judge5").transform;
            }
            if (rand == 2)
            {
                target = GameObject.Find("judge2").transform;
            }
        }
        if (other.gameObject.name == "judge5")
        {
            int rand;
            rand = Random.Range(0, 1 + 1);
            if (rand == 0)
            {
                target = GameObject.Find("judge4").transform;
            }
            if (rand == 1)
            {
                target = GameObject.Find("judge9").transform;
            }
        }
        if (other.gameObject.name == "judge6")
        {
            int rand;
            rand = Random.Range(0, 2 + 1);
            if (rand == 0)
            {
                target = GameObject.Find("judge8").transform;
            }
            if (rand == 1)
            {
                target = GameObject.Find("judge7").transform;
            }
            if (rand == 2)
            {
                target = GameObject.Find("judge3").transform;
            }
        }
        if (other.gameObject.name == "judge7")
        {
            int rand;
            rand = Random.Range(0, 1 + 1);
            if (rand == 0)
            {
                target = GameObject.Find("judge6").transform;
            }
            if (rand == 1)
            {
                target = GameObject.Find("judge9").transform;
            }
        }
        if (other.gameObject.name == "judge8")
        {
            int rand;
            rand = Random.Range(0, 3 + 1);
            if (rand == 0)
            {
                target = GameObject.Find("judge").transform;
            }
            if (rand == 1)
            {
                target = GameObject.Find("judge9").transform;
            }
            if (rand == 2)
            {
                target = GameObject.Find("judge6").transform;
            }
            if (rand == 3)
            {
                target = GameObject.Find("judge4").transform;
            }
        }
        if (other.gameObject.name == "judge9")
        {
            int rand;
            rand = Random.Range(0, 2 + 1);
            if (rand == 0)
            {
                target = GameObject.Find("judge7").transform;
            }
            if (rand == 1)
            {
                target = GameObject.Find("judge5").transform;
            }
            if (rand == 2)
            {
                target = GameObject.Find("judge8").transform;
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
