using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePad : MonoBehaviour
{
//**********************************************************************
//
// 列挙型定義
//
//**********************************************************************

	public enum StickFlag
	{
		NONE = -1,
		UP,
		DOWN,
		LEFT,
		RIGHT,
		MAX
	}

	public enum AnalogDataFlag
	{
		NONE = -1,
		L_STICK_X,
		L_STICK_Y,
		R_STICK_X,
		R_STICK_Y,
		L2_BUTTON,
		R2_BUTTON,
		MAX
	}


//**********************************************************************
//
// 構造体定義
//
//**********************************************************************

	struct GamePadData
	{
		public Vector2 l_stick_;
		public Vector2 r_stick_;
		public Vector2 trigger_button_;
	}



//**********************************************************************
//
// データ
//
//**********************************************************************

	// 定数
	const float STICK_TRIGGER_SENSITIVITY  = 0.8f;
	const float STICK_HOLD_SENSITIVITY     = 0.0001f;
	const float TRIGGER_BUTTON_TRIGGER_RELEASE_SENSITIVITY = 0.8f;
	const float TRIGGER_BUTTON_HOLD_SENSITIVITY	   = 0.0001f; 

	// 毎フレームのGamePadのデータ保存用
	GamePadData game_pad_data_;
	GamePadData old_game_pad_data_;


	
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
		Update();
	}


	
//################################################################################
//
// [ 更新関数(Unityメインループ関数) ]
//
//################################################################################

	void Update()
	{
		// 値の保存
		old_game_pad_data_ = game_pad_data_;

		// Lスティック
		game_pad_data_.l_stick_.x = Input.GetAxis("LStick_X");
		game_pad_data_.l_stick_.y = Input.GetAxis("LStick_Y");

		// Rスティック
		game_pad_data_.r_stick_.x = Input.GetAxis("RStick_X");
		game_pad_data_.r_stick_.y = Input.GetAxis("RStick_Y");

		// トリガーボタン
		game_pad_data_.trigger_button_.x = Input.GetAxis("L2");
		game_pad_data_.trigger_button_.y = Input.GetAxis("R2");
	}


//================================================================================
//
// [ GamePadのボタン判定関数(Hold) ]
//
//================================================================================

	public bool ButtonHold(string button_name)
	{
		return Input.GetButton(button_name);
	}



//================================================================================
//
// [ GamePadのボタン判定関数(Trigger) ]
//
//================================================================================

	public bool ButtonTrigger(string button_name)
	{
		return Input.GetButtonDown(button_name);
	}



//================================================================================
//
// [ GamePadのボタン判定関数(Release) ]
//
//================================================================================

	public bool ButtonRelease(string button_name)
	{
		return Input.GetButtonUp(button_name);
	}



//================================================================================
//
// [ GamePadのLステック判定関数(Trigger) ]
//
//================================================================================

	public bool LstickTrigger(StickFlag flag)
	{
		switch (flag)
		{
			case StickFlag.UP :
			{
				if (game_pad_data_.l_stick_.y     <  STICK_TRIGGER_SENSITIVITY) return false;
				if (old_game_pad_data_.l_stick_.y >= STICK_TRIGGER_SENSITIVITY) return false;
				return true;
			}	
			case StickFlag.DOWN :
			{
				if (game_pad_data_.l_stick_.y     >  -STICK_TRIGGER_SENSITIVITY) return false;
				if (old_game_pad_data_.l_stick_.y <= -STICK_TRIGGER_SENSITIVITY) return false;
				return true;
			}
			case StickFlag.LEFT :
			{
				if (game_pad_data_.l_stick_.x     >  -STICK_TRIGGER_SENSITIVITY) return false;
				if (old_game_pad_data_.l_stick_.x <= -STICK_TRIGGER_SENSITIVITY) return false;
				return true;
			}
			case StickFlag.RIGHT :
			{
				if (game_pad_data_.l_stick_.x     <  STICK_TRIGGER_SENSITIVITY) return false;
				if (old_game_pad_data_.l_stick_.x >= STICK_TRIGGER_SENSITIVITY) return false;
				return true;
			}
		}

		return false;
	}



//================================================================================
//
// [ GamePadのLステック判定関数(Hold) ]
//
//================================================================================

	public bool LstickHold(StickFlag flag)
	{
		switch (flag)
		{
			case StickFlag.UP :
			{
				if (game_pad_data_.l_stick_.y < STICK_HOLD_SENSITIVITY) return false;
				return true;
			}	
			case StickFlag.DOWN :
			{
				if (game_pad_data_.l_stick_.y > -STICK_HOLD_SENSITIVITY) return false;
				return true;
			}
			case StickFlag.LEFT :
			{
				if (game_pad_data_.l_stick_.x > -STICK_HOLD_SENSITIVITY) return false;
				return true;
			}
			case StickFlag.RIGHT :
			{
				if (game_pad_data_.l_stick_.x < STICK_HOLD_SENSITIVITY) return false;
				return true;
			}
		}

		return false;
	}



//================================================================================
//
// [ GamePadのRステック判定関数(Trigger) ]
//
//================================================================================

	public bool RstickTrigger(StickFlag flag)
	{
		switch (flag)
		{
			case StickFlag.UP :
			{
				if (game_pad_data_.r_stick_.y     <  STICK_TRIGGER_SENSITIVITY) return false;
				if (old_game_pad_data_.r_stick_.y >= STICK_TRIGGER_SENSITIVITY) return false;
				return true;
			}	
			case StickFlag.DOWN :
			{
				if (game_pad_data_.r_stick_.y     >  -STICK_TRIGGER_SENSITIVITY) return false;
				if (old_game_pad_data_.r_stick_.y <= -STICK_TRIGGER_SENSITIVITY) return false;
				return true;
			}
			case StickFlag.LEFT :
			{
				if (game_pad_data_.r_stick_.x     >  -STICK_TRIGGER_SENSITIVITY) return false;
				if (old_game_pad_data_.r_stick_.x <= -STICK_TRIGGER_SENSITIVITY) return false;
				return true;
			}
			case StickFlag.RIGHT :
			{
				if (game_pad_data_.r_stick_.x     <  STICK_TRIGGER_SENSITIVITY) return false;
				if (old_game_pad_data_.r_stick_.x >= STICK_TRIGGER_SENSITIVITY) return false;
				return true;
			}
		}

		return false;
	}



//================================================================================
//
// [ GamePadのRステック判定関数(Hold) ]
//
//================================================================================

	public bool RstickHold(StickFlag flag)
	{
		switch (flag)
		{
			case StickFlag.UP :
			{
				if (game_pad_data_.r_stick_.y < STICK_HOLD_SENSITIVITY) return false;
				return true;
			}	
			case StickFlag.DOWN :
			{
				if (game_pad_data_.r_stick_.y > -STICK_HOLD_SENSITIVITY) return false;
				return true;
			}
			case StickFlag.LEFT :
			{
				if (game_pad_data_.r_stick_.x > -STICK_HOLD_SENSITIVITY) return false;
				return true;
			}
			case StickFlag.RIGHT :
			{
				if (game_pad_data_.r_stick_.x < STICK_HOLD_SENSITIVITY) return false;
				return true;
			}
		}

		return false;
	}



//================================================================================
//
// [ GamePadのL2判定関数(Trigger) ]
//
//================================================================================

	public bool L2Trigger()
	{
		if (game_pad_data_.trigger_button_.x     <  TRIGGER_BUTTON_TRIGGER_RELEASE_SENSITIVITY) return false;
		if (old_game_pad_data_.trigger_button_.x >= TRIGGER_BUTTON_TRIGGER_RELEASE_SENSITIVITY) return false;
		return true;
	}



//================================================================================
//
// [ GamePadのR2判定関数(Trigger) ]
//
//================================================================================

	public bool R2Trigger()
	{
		if (game_pad_data_.trigger_button_.y     >  -TRIGGER_BUTTON_TRIGGER_RELEASE_SENSITIVITY) return false;
		if (old_game_pad_data_.trigger_button_.y <= -TRIGGER_BUTTON_TRIGGER_RELEASE_SENSITIVITY) return false;
		return true;
	}



//================================================================================
//
// [ GamePadのL2判定関数(Hold) ]
//
//================================================================================

	public bool L2Hold()
	{
		if (game_pad_data_.trigger_button_.x < TRIGGER_BUTTON_HOLD_SENSITIVITY) return false;
		return true;
	}



//================================================================================
//
// [ GamePadのR2判定関数(Hold) ]
//
//================================================================================

	public bool R2Hold()
	{
		if (game_pad_data_.trigger_button_.y > -TRIGGER_BUTTON_HOLD_SENSITIVITY) return false;
		return true;
	}



//================================================================================
//
// [ GamePadのL2判定関数(Release) ]
//
//================================================================================

	public bool L2Release()
	{
		if (game_pad_data_.trigger_button_.x     >  TRIGGER_BUTTON_TRIGGER_RELEASE_SENSITIVITY) return false;
		if (old_game_pad_data_.trigger_button_.x <= TRIGGER_BUTTON_TRIGGER_RELEASE_SENSITIVITY) return false;
		return true;
	}



//================================================================================
//
// [ GamePadのR2判定関数(Release) ]
//
//================================================================================

	public bool R2Release()
	{
		if (game_pad_data_.trigger_button_.y     <  -TRIGGER_BUTTON_TRIGGER_RELEASE_SENSITIVITY) return false;
		if (old_game_pad_data_.trigger_button_.y >= -TRIGGER_BUTTON_TRIGGER_RELEASE_SENSITIVITY) return false;
		return true;
	}



//================================================================================
//
// [ GamePadのアナログデータ取得関数 ]
//
//================================================================================

	public float GetAnalogData(AnalogDataFlag flag)
	{
		switch (flag)
		{
			case AnalogDataFlag.L_STICK_X :
			{
				return game_pad_data_.l_stick_.x;
			}
			case AnalogDataFlag.L_STICK_Y :
			{
				return game_pad_data_.l_stick_.y;
			}
			case AnalogDataFlag.R_STICK_X :
			{
				return game_pad_data_.r_stick_.x;
			}
			case AnalogDataFlag.R_STICK_Y :
			{
				return game_pad_data_.r_stick_.y;
			}
			case AnalogDataFlag.L2_BUTTON :
			{
				return game_pad_data_.trigger_button_.x;
			}
			case AnalogDataFlag.R2_BUTTON :
			{
				return game_pad_data_.trigger_button_.y;
			}
		}

		return 0.0f;
	}
}
