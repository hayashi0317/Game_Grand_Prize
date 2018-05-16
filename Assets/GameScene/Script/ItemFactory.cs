using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
//**********************************************************************
//
// 列挙型定義
//
//**********************************************************************

	public enum ItemNum
	{
		NONE = -1,
		TIMER_CAMERA,
		LAMP,
		MAX
	}



//**********************************************************************
//
// データ
//
//**********************************************************************

	// アイテムプレハブ
	[SerializeField]
	GameObject timer_camera_prefab_;

	[SerializeField]
	GameObject lamp_prefab_;

	

//**********************************************************************
//
// メソッド
//
//**********************************************************************



//================================================================================
//
// [ アイテム生成関数 ]
//
//================================================================================
	
	public GameObject ItemCreate(ItemNum item_num, Vector3 position, Quaternion rotation, float timer_count)
	{
		GameObject temp_object;
		switch (item_num)
		{
			case ItemNum.TIMER_CAMERA :
			{
				temp_object = Instantiate(timer_camera_prefab_) as GameObject;
				temp_object.GetComponent<TimerCameraController>().Init(timer_count);
				break;
			}
			case ItemNum.LAMP :
			{
				temp_object = Instantiate(lamp_prefab_) as GameObject;
				break;
			}
			default :
			{
				temp_object = null;
				break;
			}
		}

		// 形状初期化
		SetInitTransform(temp_object, position, rotation);

		return temp_object;
	}



//================================================================================
//
// [ 初期形状設定関数 ]
//
//================================================================================
	
	void SetInitTransform(GameObject game_object, Vector3 position, Quaternion rotation)
	{
		if (game_object == null) return;

		game_object.transform.position = position;
		game_object.transform.rotation = rotation;
	}
}



