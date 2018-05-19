using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
		START,
		COUNT_DOWN,
		CHANGE_TIME,
		COUNT_UP,
		GAME_OVER,
		MAX
	}



//**********************************************************************
//
// データ
//
//**********************************************************************

	// 定数
	const float MAX_CAMERA_SET_TIME = 20.0f;

	// UI
	UIManager ui_manager_;

	// タイマー
	float time_;

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
	State state_ = State.START;

	// ゲームパッド
	GamePad game_pad_;



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
		// UI
		ui_manager_ = GameObject.Find("UI").GetComponent<UIManager>();
		// タイマーカメラOFF
		ui_manager_.TimerCameraUIOFF();
		ui_manager_.StartStateCanvasON();
		ui_manager_.MainCameraUIOFF();
		ui_manager_.ChangeTimeCanvasOFF();
		ui_manager_.GameOverCanvasOFF();
		ui_manager_.FixdCanvasOFF();

		// キューブ
		cube_ = GameObject.Find("koudai3D");
		cube_.SetActive(false);

        //シーン
        fade = GameObject.Find("FadeDirector");

		// タイマーの初期化
		time_ = MAX_CAMERA_SET_TIME;

		// ステート
		state_ = State.START;

		// ゲームパッドの取得
		game_pad_ = GameObject.Find("GamePad").GetComponent<GamePad>();
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
			case State.START :
			{
				StartStateUpdate();
				break;
			}
			case State.COUNT_DOWN :
			{
				CountDownStateUpdate();
				break;
			}
			case State.CHANGE_TIME :
			{
				ChangeTimeStateUpdate();
				break;
			}
			case State.COUNT_UP :
			{
				CountUpUpdate();
				break;
			}
			case State.GAME_OVER :
			{
				GameOverUpdate();
				break;
			}
		}
	}



//================================================================================
//
// [ スタートステート更新関数 ]
//
//================================================================================
	
	void StartStateUpdate()
	{
		if (game_pad_.ButtonTrigger("START"))
		{
			state_ = State.COUNT_DOWN;

			ui_manager_.MainCameraUION();
			ui_manager_.FixdCanvasON();
			ui_manager_.StartStateCanvasOFF();
		}
	}



//================================================================================
//
// [ カウントダウンステート更新関数 ]
//
//================================================================================
	
	void CountDownStateUpdate()
	{
		// カウントダウン
		if (!CountDown())
		{
			state_ = State.CHANGE_TIME;
			ui_manager_.MainCameraUIOFF();
			ui_manager_.FixdCanvasOFF();
			ui_manager_.ChangeTimeCanvasON();
		}

		ui_manager_.UpdateCountUI(time_);
	}



//================================================================================
//
// [ チェンジタイムステート更新関数 ]
//
//================================================================================
	
	void ChangeTimeStateUpdate()
	{
		if (game_pad_.ButtonTrigger("START"))
		{
			state_ = State.COUNT_UP;

			ui_manager_.MainCameraUION();
			ui_manager_.FixdCanvasON();
			ui_manager_.ChangeTimeCanvasOFF();
			
			// スタート処理
			MoveStart();
		}
	}


	
//================================================================================
//
// [ カウントアップ更新関数 ]
//
//================================================================================
	
	void CountUpUpdate()
	{
		if (cube_.GetComponent<Koudai3D_Move>().GetItem >= 2)
		{
			state_ = State.GAME_OVER;

			ui_manager_.MainCameraUIOFF();
			ui_manager_.FixdCanvasOFF();
			ui_manager_.GameOverCanvasON();
		}
	}



//================================================================================
//
// [ ゲーム終了更新関数 ]
//
//================================================================================
	
	void GameOverUpdate()
	{
		scene_change();
	}



//================================================================================
//
// [ カウントダウン関数 ]
//
//================================================================================
	
	bool CountDown()
	{
		time_ -= Time.deltaTime;

		if (time_ <= 0.0f)
		{
			time_ = 0.0f;
			
			return false;
		}

		return true;
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

//================================================================================
//
// [ リザルトヘ遷移関数 ]
//
//================================================================================

    void scene_change()
    {
        //time_scene++;
        
        FadeManager fade_start = fade.GetComponent<FadeManager>();

		if (game_pad_.ButtonTrigger("START"))
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
