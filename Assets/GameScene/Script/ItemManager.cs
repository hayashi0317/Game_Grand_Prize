﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
//**********************************************************************
//
// データ
//
//**********************************************************************

	// 定数
	const int ITEM_ARRAY_NUM = 2;

	// アイテム番号
	ItemFactory.ItemNum[] item_;

	// アイテムインデックス
	int item_index_;

	// アイテムファクトリー
	ItemFactory item_factory_;

	



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
		// アイテム配列の初期化
		item_ = new ItemFactory.ItemNum[ITEM_ARRAY_NUM];

		for (int i = 0; i < (int)ItemFactory.ItemNum.MAX; i++)
		{
			item_[i] = (ItemFactory.ItemNum)i;
		}

		// アイテムインデックス
		item_index_ = 0;

		// アイテムファクトリー
		item_factory_ = transform.FindChild("ItemFactory").gameObject.GetComponent<ItemFactory>();
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
// [ アイテムの種類取得関数 ]
//
//================================================================================
	
	public ItemFactory.ItemNum GetItemNum()
	{
		return item_[item_index_];
	}



//================================================================================
//
// [ アイテム生成関数 ]
//
//================================================================================
	
	public void ItemCreate(Vector3 position, Quaternion rotation, float timer_count)
	{
		// アイテムの生成
		GameObject temp_object = item_factory_.ItemCreate(item_[item_index_], 
														  position, 
													      rotation,
														  timer_count);

		// アイテムの設置
	}

//================================================================================
//
// [ アイテム選択関数(左) ]
//
//================================================================================
	
	public void SelectItem_Left()
	{
		if (--item_index_ < 0) item_index_ = ITEM_ARRAY_NUM - 1;
	}



//================================================================================
//
// [ アイテム選択関数(右) ]
//
//================================================================================
	
	public void SelectItem_Right()
	{
		if (++item_index_ >= ITEM_ARRAY_NUM) item_index_ = 0;
	}
}
