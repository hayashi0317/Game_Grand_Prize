using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{

//**********************************************************************
//
// データ
//
//**********************************************************************

	// 定数
	public const int TIMER_CAMERA = 0;
	public const int LAMP		  = 1;

	// アイテムプレハブ
	public GameObject timer_camera_prefab_;
	public GameObject lamp_prefab_;

	// アイテム番号
	int select_item_num_ = TIMER_CAMERA;

	// メインカメラ
	GameObject main_camera_;

	



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
		main_camera_ = GameObject.Find("MainCamera");
	}


	
//################################################################################
//
// [ 更新関数(Unityメインループ関数) ]
//
//################################################################################

	void Update()
	{

	}



//================================================================================
//
// [ アイテム番号取得関数 ]
//
//================================================================================
	
	public int GetItemNum()
	{
		return select_item_num_;
	}



//================================================================================
//
// [ アイテム選択関数(左) ]
//
//================================================================================
	
	public void SelectItem_Left()
	{
		select_item_num_ = (select_item_num_ - 1) < TIMER_CAMERA ? LAMP : --select_item_num_;
	}



//================================================================================
//
// [ アイテム選択関数(右) ]
//
//================================================================================
	
	public void SelectItem_Right()
	{
		select_item_num_ = (select_item_num_ + 1) > LAMP ? TIMER_CAMERA : ++select_item_num_;
	}



//================================================================================
//
// [ アイテム使用関数 ]
//
//================================================================================
	
	public void UseItem(GameObject game_object)
	{
		switch (select_item_num_)
		{
			case TIMER_CAMERA :
			{
				GameObject timer_camera = Instantiate(timer_camera_prefab_) as GameObject;
				timer_camera.GetComponent<TimerCameraController>().Init();
					timer_camera.GetComponent<TimerCameraController>().ObjectOff();
				Vector3 temp_position = game_object.transform.position;
				
				temp_position.y += 0.5f;
				
				timer_camera.transform.position = temp_position;
				main_camera_.GetComponent<CameraController>().ChangeTimerCamera_Move(timer_camera,
																					 temp_position, 
																					 game_object.transform.rotation);
				break;
			}

			case LAMP :
			{
				GameObject lamp = Instantiate(lamp_prefab_) as GameObject;
				
				Vector3 temp_position = game_object.transform.position;
				temp_position.y += 0.5f;
				lamp.transform.position = temp_position;

				break;
			}
		}
	}


}
