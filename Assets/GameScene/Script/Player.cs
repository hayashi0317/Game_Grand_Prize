using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

//**********************************************************************
//
// 列挙型
//
//**********************************************************************

	enum State
	{
		NONE = -1,
		MAIN_CAMERA_MOVE,
		TIMER_CAMERA_MOVE,
		TIMER_CAMERA_TIME_ADJUSTMENT,
		SHOT,
		MAX
	}



//**********************************************************************
//
// データ
//
//**********************************************************************

	// 定数
	const float TRANSLATION_SPEED   = 0.3f;
	const float ROTATE_SPEED_X      = 2.0f;
	const float ROTATE_SPEED_Y      = 1.0f;
	
	const float MAX_TIMER_COUNT = 60.0f;
	const float ADD_TIMER_COUNT = 0.8f;

	// ゲームパッド
	GamePad game_pad_;

	// アイテムマネージャ
	ItemManager item_manager_;

	// UI
	UIManager ui_manager_;
	
	// アイテム用形状保存
	Vector3    item_position_;
	Quaternion item_rotation_;

	// タイマーカウンター
	float timer_count_;

	// カメラの座標保存
	Vector3    save_camera_positon_;
	Quaternion save_camera_rotation_;

	// ステート
	State state_ = State.MAIN_CAMERA_MOVE;

	//カメラ撮影回数カウント
    public int timer_camera_count = 0;


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
		// ゲームパッドの取得
		game_pad_ = GameObject.Find("GamePad").GetComponent<GamePad>();
		
		// アイテムマネージャの取得
		item_manager_ = GameObject.Find("ItemManager").GetComponent<ItemManager>();
		
		// UI
		ui_manager_ = GameObject.Find("UI").GetComponent<UIManager>();

		// タイマーカウンターの初期化
		timer_count_ = 0.0f;

		// 状態の保存
		SeveTransform();
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
			case State.MAIN_CAMERA_MOVE :
			{
				// メインカメラ更新
				UpdateMainCamera();
				break;
			}
			case State.TIMER_CAMERA_MOVE :
			{
				// タイマーカメラ更新(移動)
				UpdateTimerCamera_Move();
				break;
			}
			case State.TIMER_CAMERA_TIME_ADJUSTMENT :
			{
				// タイマーカメラ更新(時間調整)
				UpdateTimerCamera_TimeAdjustment();
				break;
			}
		}

		Debug.Log("pos:" + item_position_);
		Debug.Log("rot:" + item_rotation_);
		Debug.Log("count:" + timer_count_);
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
		}

		if (game_pad_.LstickHold(GamePad.StickFlag.RIGHT))
		{
			temp_vector.x = 1.0f;
		}

		if (game_pad_.LstickHold(GamePad.StickFlag.UP))
		{
			temp_vector.z = 1.0f;
		}

		if (game_pad_.LstickHold(GamePad.StickFlag.DOWN))
		{
			temp_vector.z = -1.0f;
		}

		// 速度ベクトルの調整
		float l_stic_abs_x = Mathf.Abs(game_pad_.GetAnalogData(GamePad.AnalogDataFlag.L_STICK_X));
		float l_stic_abs_y = Mathf.Abs(game_pad_.GetAnalogData(GamePad.AnalogDataFlag.L_STICK_Y));
		float proportion   = (l_stic_abs_x > l_stic_abs_y ? l_stic_abs_x : l_stic_abs_y);

		temp_vector.Normalize();
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
		float r_stic_abs_x = Mathf.Abs(game_pad_.GetAnalogData(GamePad.AnalogDataFlag.R_STICK_X));
		float r_stic_abs_y = Mathf.Abs(game_pad_.GetAnalogData(GamePad.AnalogDataFlag.R_STICK_Y));
		float temp_speed   = (r_stic_abs_x > r_stic_abs_y ? r_stic_abs_x : r_stic_abs_y);
		

		if (game_pad_.RstickHold(GamePad.StickFlag.LEFT))
		{
			temp_speed *= ROTATE_SPEED_X;
			transform.Rotate(0.0f, -temp_speed, 0.0f, Space.World);
		}

		if (game_pad_.RstickHold(GamePad.StickFlag.RIGHT))
		{
			temp_speed *= ROTATE_SPEED_X;
			transform.Rotate(0.0f, temp_speed, 0.0f, Space.World);
		}

		if (game_pad_.RstickHold(GamePad.StickFlag.UP))
		{
			temp_speed *= ROTATE_SPEED_Y;
			transform.Rotate(-temp_speed, 0.0f, 0.0f);
		}

		if (game_pad_.RstickHold(GamePad.StickFlag.DOWN))
		{
			temp_speed *= ROTATE_SPEED_Y;
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
			UseItem(hit.collider.gameObject);
			
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
		if (!game_pad_.L2Hold()) return;

		if (game_pad_.ButtonTrigger("X") || Input.GetKeyDown(KeyCode.K))
		{
			
			item_manager_.SelectItem_Left();
		}

		if (game_pad_.ButtonTrigger("B") || Input.GetKeyDown(KeyCode.L))
		{
			item_manager_.SelectItem_Right();
		}

		// アイテムイメージUI変更
		ItemFactory.ItemNum temp_num = item_manager_.GetItemNum();
		ui_manager_.ChangeItemImage(temp_num);
	}



//================================================================================
//
// [ アイテム使用関数 ]
//
//================================================================================

	void UseItem(GameObject game_object)
	{
		if (game_pad_.R2Trigger())
		{
			// アイテム形状の保存と変更
			item_position_ = game_object.transform.position;
			item_rotation_ = Quaternion.Euler(0.0f, transform.localEulerAngles.y, 0.0f);

			if (item_manager_.GetItemNum() == ItemFactory.ItemNum.TIMER_CAMERA)
			{
				// タイマーカメラを動かすステートに変更
				ChangeTimerCamera_Move(item_position_, item_rotation_);
			}						   
			else
			{
				// アイテム生成
				item_manager_.ItemCreate(item_position_, item_rotation_, 0.0f);
			}
		}
	}



//================================================================================
//
// [ 時間設定関数 ]
//
//================================================================================

	void SetTimer()
	{
		float l_stic_abs_x = Mathf.Abs(game_pad_.GetAnalogData(GamePad.AnalogDataFlag.L_STICK_X));
		
		if (game_pad_.LstickHold(GamePad.StickFlag.RIGHT))
		{
			if (timer_count_ >= MAX_TIMER_COUNT) return;
		
			timer_count_ += ADD_TIMER_COUNT * l_stic_abs_x;

			if (timer_count_ >= MAX_TIMER_COUNT)
			{
				timer_count_ = MAX_TIMER_COUNT;
			}
		
			ui_manager_.SetTimerCameraCount(timer_count_);
			ui_manager_.SetTimerCameraGage(timer_count_ / MAX_TIMER_COUNT);
		}

		if (game_pad_.LstickHold(GamePad.StickFlag.LEFT))
		{
			if (timer_count_ <= 0.0f) return;
		
			timer_count_ += -ADD_TIMER_COUNT * l_stic_abs_x;

			if (timer_count_ <= 0.0f)
			{
				timer_count_ = 0.0f;
			}

			ui_manager_.SetTimerCameraCount(timer_count_);
			ui_manager_.SetTimerCameraGage(timer_count_ / MAX_TIMER_COUNT);
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
		if (state_ != State.TIMER_CAMERA_TIME_ADJUSTMENT) return;

		// タイマーカメラ作成
		item_manager_.ItemCreate(item_position_, item_rotation_, timer_count_);

		// ステートの変更
		state_ = State.MAIN_CAMERA_MOVE;

		// 状態の読み込み
		LoadTransform();

		// UIの変更
		ui_manager_.MainCameraUION();
		ui_manager_.TimerCameraUIOFF();
		
	}



//================================================================================
//
// [ タイマーカメラ移動へ変更関数 ]
//
//================================================================================

	void ChangeTimerCamera_Move(Vector3 position, Quaternion rotation)
	{
		if (state_ != State.MAIN_CAMERA_MOVE) return;

		// 状態の保存
		SeveTransform();

		// ステートの変更
		state_ = State.TIMER_CAMERA_MOVE;

		// 状態の変更
		transform.position = position;
		transform.rotation = rotation;
	}



//================================================================================
//
// [ タイマーカメラ時間調整へ変更関数 ]
//
//================================================================================

	void ChangeTimerCamera_TimeAdjustment()
	{
		if (state_ != State.TIMER_CAMERA_MOVE) return;

		// ステートの変更
		state_ = State.TIMER_CAMERA_TIME_ADJUSTMENT;

		// ローテーションの変更
		item_rotation_ = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);

		// 時間の調整
		timer_count_ = 0.0f;

		// UIの変更
		ui_manager_.MainCameraUIOFF();
		ui_manager_.TimerCameraUION();
		ui_manager_.SetTimerCameraCount(timer_count_);
		ui_manager_.SetTimerCameraGage(timer_count_ / MAX_TIMER_COUNT);

	}



//================================================================================
//
// [ 撮影へ変更関数 ]
//
//================================================================================

	public void ChangeShot(Vector3 position, Quaternion rotation)
	{
		// ステートの変更
		state_ = State.SHOT;

		// 状態の読み込み
		SeveTransform();

		// 状態の変更
		transform.position = position;
		transform.rotation = rotation;

		// UIの変更
		ui_manager_.MainCameraUIOFF();
	}



//================================================================================
//
// [ 元の状態へ戻る関数 ]
//
//================================================================================

	public void ChangeShotReverse()
	{
		// ステートの変更
		state_ = State.MAIN_CAMERA_MOVE;

		// 状態の読み込み
		LoadTransform();

		// UIの変更
		ui_manager_.MainCameraUION();
		ui_manager_.TimerCameraUIOFF();
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

//================================================================================
//
// [ 撮影枚数管理関数 ]
//
//================================================================================

	public void gettimer()
    {
        timer_camera_count++;
    }

}



