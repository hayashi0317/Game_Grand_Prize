﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TimerCameraController : MonoBehaviour
{

//**********************************************************************
//
// データ
//
//**********************************************************************

	// 定数
	const float MAX_TIMER_COUNT = 60.0f;
	const float ADD_TIMER_COUNT = 1.0f;

	// 時間
	float timer_count_ = 0.0f;
	bool  is_count_down_ = false;

	// ゲームディレクター
	GameObject game_director_;

	// 子オブジェクト
	GameObject child_object_;

	// UI
	GameObject timer_count_ui_;

	// メインカメラ
	GameObject main_camera_;
	bool is_shot_ = false;

	


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
	}


	
//################################################################################
//
// [ 更新関数(Unityメインループ関数) ]
//
//################################################################################

	void Update()
	{
		if (is_shot_)
		{
			main_camera_.GetComponent<Player>().ChangeShotReverse();
			is_shot_ = false;
		}
		
		// タイマー更新
		UpdateTimer();
		
	}
	


//================================================================================
//
// [ タイマーカメラ初期化関数 ]
//
//================================================================================
	
	public void Init(float timer_count)
	{
		timer_count_ = timer_count;
		game_director_ = GameObject.Find("GameDirector");

		child_object_ = transform.Find("TimerCameraObject").gameObject;

		timer_count_ui_ = transform.Find("TimerCameraObject/Canvas/ObjectTimerCount").gameObject;

		main_camera_ = GameObject.Find("MainCamera");

		UpdataUI();
	}

//================================================================================
//
// [ タイマーカメラ初期化関数 ]
//
//================================================================================
	
	public void InitUI()
	{
		game_director_.GetComponent<GameDirector>().SetTimerCameraCount(timer_count_);
		game_director_.GetComponent<GameDirector>().SetTimerCameraGage(timer_count_ / MAX_TIMER_COUNT);
	}



//================================================================================
//
// [ タイマー更新関数 ]
//
//================================================================================
	
	void UpdateTimer()
	{
		if (!is_count_down_) return;

		UpdataUI();

		timer_count_ -= Time.deltaTime;

		if (timer_count_ <= 0.0f)
		{
			// 撮影
			Shooting();

			is_count_down_ = false;
		}
	}



//================================================================================
//
// [ 撮影関数 ]
//
//================================================================================
	
	void Shooting()
	{
		Debug.Log("撮影");
		main_camera_.GetComponent<Player>().ChangeShot(transform.position, transform.rotation);
		main_camera_.GetComponent<Player>().Judgment();

		ScreenCapture.CaptureScreenshot("Assets/screenshot" + game_director_.GetComponent<GameDirector>().screenShotCount + ".png");
        game_director_.GetComponent<GameDirector>().screenShotCount++;
        if (game_director_.GetComponent<GameDirector>().screenShotCount >= 3)
        {
			game_director_.GetComponent<GameDirector>().screenShotCount = 0;
        }

		is_shot_ = true;
	}



//================================================================================
//
// [ カウントダウン開始関数 ]
//
//================================================================================
	
	public void CountDownON()
	{
		is_count_down_ = true;
	}



//================================================================================
//
// [ UI変更関数 ]
//
//================================================================================
	
	void UpdataUI()
	{
		timer_count_ui_.GetComponent<Text>().text = timer_count_.ToString("F1") + "s";
	}
}
