using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CameraMove : MonoBehaviour
{
	// 定数
	const float STICK_SENSITIVITY   = 0.0001f;

    public GameObject Target;
    float MoveX;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //右に移動
        if (Input.GetKeyDown(KeyCode.D) || (Input.GetAxis("LStick_X") > STICK_SENSITIVITY))
        {
            float freeze_y = transform.position.y;
            MoveX=transform.position.x + 10;
            if(MoveX>=20)
            {
                MoveX = 20;
            }
            transform.position = new Vector3(MoveX, freeze_y, 0);
        }
        //左に移動
        if (Input.GetKeyDown(KeyCode.A) || (Input.GetAxis("LStick_X") < -STICK_SENSITIVITY))
        {
            float freeze_y = transform.position.y;
            MoveX = transform.position.x - 10;
            if (MoveX < 0)
            {
                MoveX = 0;
            }
            transform.position = new Vector3(MoveX, freeze_y, 0);
        }

        float PosX = MoveX;
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("A"))
        {
			if(PosX==0)
			{
			    SceneManager.LoadScene("GameScene");
			}
			if(PosX==10)
			{
				SceneManager.LoadScene("GameScene");
			}
        }

		Debug.Log(PosX);
    }
}