using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
//**********************************************************************
//
// データ
//
//**********************************************************************

	// 画像
	public Sprite timer_camera_image_;
	public Sprite lamp_image_;

	// メインカメラ時のUI
	GameObject item_ui_;
	GameObject main_camera_canvas_;

	// タイマーカメラ時UI
	GameObject timer_camera_canvas_;
	GameObject timer_gage_;
	GameObject timer_count_;

	// タイマー
	GameObject timer_text_;

	// スタートステートキャンバス
	GameObject start_state_canvas_;

	// チェンジタイムステートキャンバス
	GameObject change_time_canvas_;

	// ゲームオーバーステートキャンバス
	GameObject game_over_canvas_;

	// Fixd
	GameObject fixd_canvas_;



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
		// メインカメラUI
		item_ui_			 = GameObject.Find("ItemBack/Item");
		main_camera_canvas_  = GameObject.Find("MainCameraCanvas");

		// タイマーカメラUI
		timer_camera_canvas_ = GameObject.Find("TimerCameraCanvas");
		timer_gage_			 = GameObject.Find("TimerGage");
		timer_count_		 = GameObject.Find("TimerCount");

		// タイマー
		timer_text_ = GameObject.Find("Time");

		// スタートステートキャンバス
		start_state_canvas_ = GameObject.Find("StartStateCanvas");

		// チェンジタイムステートキャンバス
		change_time_canvas_ = GameObject.Find("ChangeTimeStateCanvas");

		// ゲームオーバーステートキャンバス
		game_over_canvas_ = GameObject.Find("GameOverCanvas");
		
		// Fixd
		fixd_canvas_ = GameObject.Find("FixedCanvas");
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
// [ カウントUI更新関数 ]
//
//================================================================================
	
	public void UpdateCountUI(float time)
	{
		timer_text_.GetComponent<Text>().text = "Time: " + time.ToString("F1") + "s";
	}



//================================================================================
//
// [ アイテムイメージ変更関数 ]
//
//================================================================================
	
	public void ChangeItemImage(ItemFactory.ItemNum select_item_num)
	{
		Image temp_image = item_ui_.GetComponent<Image>();

		switch(select_item_num)
		{
			case ItemFactory.ItemNum.TIMER_CAMERA :
			{
				
				temp_image.sprite = timer_camera_image_;
				break;
			}

			case ItemFactory.ItemNum.LAMP :
			{
				temp_image.sprite = lamp_image_;
				break;
			}
		}
	}



//================================================================================
//
// [ メインカメラUIOFF関数 ]
//
//================================================================================
	
	public void MainCameraUIOFF()
	{
		main_camera_canvas_.SetActive(false);
	}



//================================================================================
//
// [ メインカメラUION関数 ]
//
//================================================================================
	
	public void MainCameraUION()
	{
		main_camera_canvas_.SetActive(true);
	}



//================================================================================
//
// [ タイマーカメラUIOFF関数 ]
//
//================================================================================
	
	public void TimerCameraUIOFF()
	{
		timer_camera_canvas_.SetActive(false);
	}



//================================================================================
//
// [ タイマーカメラUION関数 ]
//
//================================================================================
	
	public void TimerCameraUION()
	{
		timer_camera_canvas_.SetActive(true);
	}



//================================================================================
//
// [ タイマーカメラゲージ設定関数 ]
//
//================================================================================
	
	public void SetTimerCameraGage(float gage_ratio)
	{
		timer_gage_.GetComponent<Image>().fillAmount = gage_ratio;
	}



//================================================================================
//
// [ タイマーカメラカウント設定関数 ]
//
//================================================================================
	
	public void SetTimerCameraCount(float time)
	{
		timer_count_.GetComponent<Text>().text = "†" + time.ToString("F1") + "s†";
	}



//================================================================================
//
// [ スタートステートキャンバスOFF関数 ]
//
//================================================================================
	
	public void StartStateCanvasOFF()
	{
		start_state_canvas_.SetActive(false);
	}



//================================================================================
//
// [ スタートステートキャンバスON関数 ]
//
//================================================================================
	
	public void StartStateCanvasON()
	{
		start_state_canvas_.SetActive(true);
	}



//================================================================================
//
// [ チェンジタイムステートキャンバスOFF関数 ]
//
//================================================================================
	
	public void ChangeTimeCanvasOFF()
	{
		change_time_canvas_.SetActive(false);
	}



//================================================================================
//
// [ チェンジタイムステートキャンバスON関数 ]
//
//================================================================================
	
	public void ChangeTimeCanvasON()
	{
		change_time_canvas_.SetActive(true);
	}



//================================================================================
//
// [ ゲームオーバーステートキャンバスOFF関数 ]
//
//================================================================================
	
	public void GameOverCanvasOFF()
	{
		game_over_canvas_.SetActive(false);
	}



//================================================================================
//
// [ ゲームオーバーステートキャンバスON関数 ]
//
//================================================================================
	
	public void GameOverCanvasON()
	{
		game_over_canvas_.SetActive(true);
	}


//================================================================================
//
// [ ゲームオーバーステートキャンバスOFF関数 ]
//
//================================================================================
	
	public void FixdCanvasOFF()
	{
		fixd_canvas_.SetActive(false);
	}



//================================================================================
//
// [ ゲームオーバーステートキャンバスON関数 ]
//
//================================================================================
	
	public void FixdCanvasON()
	{
		fixd_canvas_.SetActive(true);
	}
}
