using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
//**********************************************************************
//
// 列挙型定義
//
//**********************************************************************

	enum State
	{
		NONE = -1,
		COUNT_DOWN,
		COUNT_UP,
		MAX
	}

//**********************************************************************
//
// データ
//
//**********************************************************************

	// 定数
	const float MAX_CAMERA_SET_TIME = 30.0f;

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
	float time_;
	GameObject timer_text_;

	// キューブ
	GameObject cube_;

	// フラグ
	bool is_start_ = false;

	// 撮影
	public int screenShotCount = 0;

    //シーン
    public int time_scene = 0;
    GameObject fade;

	// ステート
	State state_ = State.COUNT_DOWN;



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

		// タイマーカメラUIOFF
		TimerCameraUIOFF();

		// タイマー
		timer_text_ = GameObject.Find("Time");

		// キューブ
		cube_ = GameObject.Find("koudai3D");
		cube_.SetActive(false);

        //シーン
        fade = GameObject.Find("FadeDirector");

		// タイマーの初期化
		time_ = MAX_CAMERA_SET_TIME;
	}


	
//################################################################################
//
// [ 更新関数(Unityメインループ関数) ]
//
//################################################################################

	void Update()
	{
		switch (state_)
		{
			case State.COUNT_DOWN :
			{
				
				break;
			}
			case State.COUNT_UP :
			{
				break;
			}
		}
		
		// カウントダウン
		CountDown();

		// カウントUI更新
		UpdateCountUI();

        scene_change();
	}



//================================================================================
//
// [ カウントダウン関数 ]
//
//================================================================================
	
	void CountDown()
	{
		if (time_ <= 0.0f) return;

		time_ -= Time.deltaTime;

		if (time_ <= 0.0f)
		{
			time_ = 0.0f;
			
			// スタート処理
			MoveStart();
		}
	}



//================================================================================
//
// [ カウントUI更新関数 ]
//
//================================================================================
	
	void UpdateCountUI()
	{
		timer_text_.GetComponent<Text>().text = "Time: " + time_.ToString("F1") + "s";
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
	
	public void SetTimerCameraCount(float timer_count)
	{
		timer_count_.GetComponent<Text>().text = "Count: " + timer_count.ToString("F1") + "s";
	}

//================================================================================
//
// [ 動きスタート関数 ]
//
//================================================================================
	
	void MoveStart()
	{
		cube_.SetActive(true);

		// GameObject型の配列cubesに、"box"タグのついたオブジェクトをすべて格納
		GameObject[] temp_cameras = GameObject.FindGameObjectsWithTag("TimerCamera");
 
		// foreachは配列の要素の数だけループします。
		foreach (GameObject camera in temp_cameras)
		{
			camera.GetComponent<TimerCameraController>().CountDownON();
		}
	}

    void scene_change()
    {
        //time_scene++;
        
        FadeManager fade_start = fade.GetComponent<FadeManager>();

		if (Input.GetButtonDown("Y"))
		{
			time_scene = 5000;
		}

        if (time_scene > 60 * 60)
        {
            fade_start.enableFade = true;
            fade_start.enableFadeOut = true;
            fade_start.FadeOut(fade_start.FadeImage);

			Debug.Log(fade_start.enableFade);

            if (!fade_start.enableFade)
            {
                SceneManager.LoadScene("Result");
            }
        }

    }

}
