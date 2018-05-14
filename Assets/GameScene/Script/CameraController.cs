using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{

//**********************************************************************
//
// 列挙型
//
//**********************************************************************

	enum State
	{
		STATE_NONE = -1,
		STATE_MAIN_CAMERA_MOVE,
		STATE_TIMER_CAMERA_MOVE,
		STATE_TIMER_CAMERA_TIME_ADJUSTMENT,
		STATE_SHOT,
		STATE_MAX
	}



//**********************************************************************
//
// データ
//
//**********************************************************************

	// 定数
	const float STICK_SENSITIVITY   = 0.0001f;
	const float TRIGGER_SENSITIVITY = 0.1f;
	const float TRANSLATION_SPEED   = 0.3f;
	const float ROTATE_SPEED        = 1.5f;


	// カメラの座標保存
	Vector3    save_camera_positon_;
	Quaternion save_camera_rotation_;
	State      state_ = State.STATE_MAIN_CAMERA_MOVE;

	// アイテム工場
	GameObject item_generator_;

	// ゲームディレクター
	GameObject game_director_;

	// タイマーカメラ
	GameObject timer_camera_;

	// ゲームパッド
	GamePad game_pad_;


	// 衝突用
	public GameObject obj1;
    public GameObject obj2;
    public Collider obj1Collider;
    public Collider obj2Collider;
    private Camera cam;
    private Plane[] planes;
    public bool judg1;
    public bool judg2;



//**********************************************************************
//
// メソッド
//
//**********************************************************************

//################################################################################
//
// [ スタート関数	(Unityメインループ関数) ]
//
//################################################################################

	void Start()
	{
		// 状態の保存
		SeveTransform();

		game_pad_       = GameObject.Find("GamePad").GetComponent<GamePad>();
		item_generator_ = GameObject.Find("ItemGenerator");
		game_director_  = GameObject.Find("GameDirector");
	}


	
//################################################################################
//
// [ 更新関数(Unityメインループ関数) ]
//
//################################################################################

	void Update()
	{
		switch(state_)
		{
			case State.STATE_MAIN_CAMERA_MOVE :
			{
				// メインカメラ更新
				UpdateMainCamera();
				break;
			}
			case State.STATE_TIMER_CAMERA_MOVE :
			{
				// タイマーカメラ更新(移動)
				UpdateTimerCamera_Move();
				break;
			}
			case State.STATE_TIMER_CAMERA_TIME_ADJUSTMENT :
			{
				// タイマーカメラ更新(時間調整)
				UpdateTimerCamera_TimeAdjustment();
				break;
			}
		}
	}



//================================================================================
//
// [ メインカメラ更新関数 ]
//
//================================================================================
	
	void UpdateMainCamera()
	{
		// 平行移動
		Translation();

		// 回転
		Rotation();

		// アイテム変更
		SelectItem();

		// 光線処理
		HitRay();
		
	}



//================================================================================
//
// [ タイマーカメラ更新関数(移動) ]
//
//================================================================================
	
	void UpdateTimerCamera_Move()
	{
		// 回転
		Rotation();

		// 時間調整へ移行
		if (game_pad_.R2Trigger())
		{
			ChangeTimerCamera_TimeAdjustment();
		}
	}



//================================================================================
//
// [ タイマーカメラ更新関数(時間調整) ]
//
//================================================================================
	
	void UpdateTimerCamera_TimeAdjustment()
	{
		// 時間設定
		SetTimer();

		// メインカメラへ移行
		if (game_pad_.R2Trigger())
		{
			ChangeMainCamera();
		}
	}



//================================================================================
//
// [ 平行移動関数 ]
//
//================================================================================

	void Translation()
	{
		Vector3 temp_vector = new Vector3(0.0f, 0.0f, 0.0f);
		
		if (game_pad_.LstickHold(GamePad.StickFlag.LEFT))
		{
			temp_vector.x = -1.0f;
			//temp_vector = new Vector3(-1.0f, 0.0f, 0.0f);
		}

		if (game_pad_.LstickHold(GamePad.StickFlag.RIGHT))
		{
			temp_vector.x = 1.0f;
			//temp_vector = new Vector3(1.0f, 0.0f, 0.0f);
		}

		if (game_pad_.LstickHold(GamePad.StickFlag.UP))
		{
			temp_vector.z = 1.0f;
			//temp_vector = new Vector3(0.0f, 0.0f, 1.0f);
		}

		if (game_pad_.LstickHold(GamePad.StickFlag.DOWN))
		{
			temp_vector.z = -1.0f;
			//temp_vector = new Vector3(0.0f, 0.0f, -1.0f);
		}

		// 速度ベクトルの調整
		temp_vector.Normalize();
		float proportion = (Mathf.Abs(Input.GetAxis("LStick_Y")) > Mathf.Abs(Input.GetAxis("LStick_X"))? 
							Mathf.Abs(Input.GetAxis("LStick_Y")) : Mathf.Abs(Input.GetAxis("LStick_X")));

		temp_vector = temp_vector * TRANSLATION_SPEED * proportion ;

		// Y座標は固定
		float temp_y = transform.position.y;
		transform.Translate(temp_vector.x, 0.0f, temp_vector.z);
		transform.position = new Vector3(transform.position.x, temp_y, transform.position.z);
	}



//================================================================================
//
// [ 回転関数 ]
//
//================================================================================

	void Rotation()
	{
		// 速度の調整
		float temp_speed = ROTATE_SPEED * 
						   (Mathf.Abs(Input.GetAxis("RStick_Y")) > Mathf.Abs(Input.GetAxis("RStick_X"))? 
							Mathf.Abs(Input.GetAxis("RStick_Y")) : Mathf.Abs(Input.GetAxis("RStick_X")));
		
		//temp_speed = ROTATE_SPEED;	// キーボードを使う場合のみ使用

		if (game_pad_.RstickHold(GamePad.StickFlag.LEFT))
		{
			transform.Rotate(0.0f, -temp_speed, 0.0f, Space.World);
		}

		if (game_pad_.RstickHold(GamePad.StickFlag.RIGHT))
		{
			transform.Rotate(0.0f, temp_speed, 0.0f, Space.World);
		}

		if (game_pad_.RstickHold(GamePad.StickFlag.UP))
		{
			transform.Rotate(-temp_speed, 0.0f, 0.0f);
		}

		if (game_pad_.RstickHold(GamePad.StickFlag.DOWN))
		{
			transform.Rotate(temp_speed, 0.0f, 0.0f);
		}
	}



//================================================================================
//
// [ 光線関数 ]
//
//================================================================================

	void HitRay()
	{
		// 画面の中央から光線を飛ばす
		Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

		// 光線とコリダーの当たり処理
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, Mathf.Infinity))
		{
			if (hit.collider.gameObject.tag != "ItemTile") return;

			hit.collider.gameObject.GetComponent<ItemTileController>().is_hit_ = true;

			// アイテム使用
			if (game_pad_.R2Trigger())
			{
				item_generator_.GetComponent<ItemGenerator>().UseItem(hit.collider.gameObject);
			}
			
			Debug.Log("name:" + hit.collider.gameObject.name);
		}
	}



//================================================================================
//
// [ アイテム選択関数 ]
//
//================================================================================

	void SelectItem()
	{
		if (Input.GetKeyDown(KeyCode.K) || Input.GetButtonDown("L1"))
		{
			item_generator_.GetComponent<ItemGenerator>().SelectItem_Left();
		}

		if (Input.GetKeyDown(KeyCode.L) || Input.GetButtonDown("R1"))
		{
			item_generator_.GetComponent<ItemGenerator>().SelectItem_Right();
		}

		// アイテムイメージUI変更
		int temp_num = item_generator_.GetComponent<ItemGenerator>().GetItemNum();
		game_director_.GetComponent<GameDirector>().ChangeItemImage(temp_num);
	}



//================================================================================
//
// [ 時間設定関数 ]
//
//================================================================================

	void SetTimer()
	{
		if (Input.GetKey(KeyCode.A) || (Input.GetAxis("LStick_X") < -STICK_SENSITIVITY))
		{
			timer_camera_.GetComponent<TimerCameraController>().DownTimerCount();
		}

		if (Input.GetKey(KeyCode.D) || (Input.GetAxis("LStick_X") > STICK_SENSITIVITY))
		{
			timer_camera_.GetComponent<TimerCameraController>().UpTimerCount();
		}
	}



//================================================================================
//
// [ 状態保存関数 ]
//
//================================================================================

	void SeveTransform()
	{
		save_camera_positon_  = transform.position;
		save_camera_rotation_ = transform.rotation;
	}



//================================================================================
//
// [ 状態読み込み関数 ]
//
//================================================================================

	void LoadTransform()
	{
		transform.position = save_camera_positon_ ;
		transform.rotation = save_camera_rotation_;
	}



//================================================================================
//
// [ メインカメラへ変更関数 ]
//
//================================================================================

	public void ChangeMainCamera()
	{
		if (state_ != State.STATE_TIMER_CAMERA_TIME_ADJUSTMENT) return;

		// タイマーカメラの表示
		timer_camera_.GetComponent<TimerCameraController>().ObjectON(transform.position, transform.rotation);

		// ステートの変更
		state_ = State.STATE_MAIN_CAMERA_MOVE;

		// 状態の読み込み
		LoadTransform();

		// UIの変更
		game_director_.GetComponent<GameDirector>().MainCameraUION();
		game_director_.GetComponent<GameDirector>().TimerCameraUIOFF();
		
	}



//================================================================================
//
// [ タイマーカメラ移動へ変更関数 ]
//
//================================================================================

	public void ChangeTimerCamera_Move(GameObject timer_camera, Vector3 position, Quaternion rotation)
	{
		if (state_ != State.STATE_MAIN_CAMERA_MOVE) return;

		// 状態の保存
		SeveTransform();

		// ステートの変更
		state_ = State.STATE_TIMER_CAMERA_MOVE;

		// 状態の変更
		transform.position = position;
		transform.rotation = rotation;

		

		// タイマーカメラの変更
		timer_camera_ = timer_camera;
	}



//================================================================================
//
// [ タイマーカメラ時間調整へ変更関数 ]
//
//================================================================================

	public void ChangeTimerCamera_TimeAdjustment()
	{
		if (state_ != State.STATE_TIMER_CAMERA_MOVE) return;

		// ステートの変更
		state_ = State.STATE_TIMER_CAMERA_TIME_ADJUSTMENT;

		// UIの変更
		game_director_.GetComponent<GameDirector>().MainCameraUIOFF();
		game_director_.GetComponent<GameDirector>().TimerCameraUION();
		timer_camera_.GetComponent<TimerCameraController>().InitUI();


	}

//================================================================================
//
// [ 撮影へ変更関数 ]
//
//================================================================================

	public void ChangeShot(Vector3 position, Quaternion rotation)
	{
		// ステートの変更
		state_ = State.STATE_SHOT;

		// 状態の読み込み
		SeveTransform();

		// 状態の変更
		transform.position = position;
		transform.rotation = rotation;

		// UIの変更
		game_director_.GetComponent<GameDirector>().MainCameraUIOFF();
	}

//================================================================================
//
// [ 元の状態へ戻る関数 ]
//
//================================================================================

	public void ChangeShotReverse()
	{
		// ステートの変更
		state_ = State.STATE_MAIN_CAMERA_MOVE;

		// 状態の読み込み
		LoadTransform();

		// UIの変更
		game_director_.GetComponent<GameDirector>().MainCameraUION();
		game_director_.GetComponent<GameDirector>().TimerCameraUIOFF();
	}

//================================================================================
//
// [ カメラとの衝突判定関数 ]
//
//================================================================================
	
	public void Judgment()
    {
        planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        obj1 = GameObject.Find("koudai3D");
        obj1Collider = obj1.GetComponent<Collider>();
        obj2 = GameObject.Find("boll");
        obj2Collider = obj1.GetComponent<Collider>();


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

//================================================================================
//
// [ 衝突確認1関数 ]
//
//================================================================================
	
	public bool Is_Judg1()
    {
        return judg1;  
    }



//================================================================================
//
// [ 衝突確認2関数 ]
//
//================================================================================
	
	public bool Is_Judg2()
    {
        return judg2;  
    }

}



