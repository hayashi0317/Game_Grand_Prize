using System.Collections;
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

	// UI
	UIManager ui_manager_;

	// 子オブジェクト
	GameObject child_object_;
	Camera child_camera_;

	// UI
	GameObject timer_count_ui_;

	// 写真の枚数
    GameObject timer_num;
    int timer_num2;

    Camera cam2;


    GameObject obj;
    RenderTexture rendertex;

    //衝突用
    public GameObject koudai;
    public Collider koudaiCollider;
    private Plane[] planes;
    private bool judg_planes = false;
    public static bool judg_koudai = false;
    RaycastHit hit;
    public LayerMask mask;
    public RenderTexture RenderTextureRef;


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

		// UI
		ui_manager_ = GameObject.Find("UI").GetComponent<UIManager>();

		child_object_ = transform.Find("TimerCameraObject").gameObject;

		GameObject temp = transform.Find("SubCamera").gameObject;
		child_camera_   = temp.GetComponent<Camera>();

		timer_count_ui_ = transform.Find("TimerCameraObject/Canvas/ObjectTimerCount").gameObject;
		
		UpdataUI();

		timer_num = GameObject.Find("Player");

        //レンダ―テクスチャ生成
        RenderTextureRef = new RenderTexture(256, 256, 0, RenderTextureFormat.ARGB32);
        RenderTextureRef.Create();
        child_camera_.targetTexture = RenderTextureRef;
	}

//================================================================================
//
// [ タイマーカメラ初期化関数 ]
//
//================================================================================
	
	public void InitUI()
	{
		ui_manager_.SetTimerCameraCount(timer_count_);
		ui_manager_.SetTimerCameraGage(timer_count_ / MAX_TIMER_COUNT);
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
		//しすいだいのアタリ判定
        judgment_planes();
        if (judg_planes)//もしあったていたらrayとばして他のものと衝突していないか判定
        {
            //ray判定
            ray_judgment();
        }
        //rendertextureを一時的にtexture2Dにすると撮影
        change_texture2D();
        Debug.Log("撮影");
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

//================================================================================
//
// [ しすい台判定関数 ]
//
//================================================================================

	void judgment_planes()
    {
        //しすいだいアタリ判定
        planes = GeometryUtility.CalculateFrustumPlanes(child_camera_);
        koudai = GameObject.Find("koudai3D");
        koudaiCollider = koudai.GetComponent<Collider>();

        if (GeometryUtility.TestPlanesAABB(planes, koudaiCollider.bounds))
        {
            judg_planes = true;
        }
        else
        {
            judg_planes = false;
        }
    }

//================================================================================
//
// [ ray判定関数 ]
//
//================================================================================

	void ray_judgment()
    { 
            Ray ray = new Ray(child_camera_.transform.position, koudai.transform.position);
            if (Physics.Raycast(ray, out hit, Vector3.Distance(child_camera_.transform.position, koudai.transform.position), mask))
            {
                //広大以外がrayにヒットした場合
                if (hit.collider.tag == "koudai3D")
                {
                    judg_koudai = true;
                }
                else
                {
                    judg_koudai = false;
                }
            }
    }

//================================================================================
//
// [ Pngへの画像変換関数 ]
//
//================================================================================
    void change_texture2D()
    {


        Player player = timer_num.GetComponent<Player>();
        player.gettimer();
        timer_num2 = player.timer_camera_count;
        Debug.Log(timer_num2 + "撮影");

     

        Texture2D tex = new Texture2D(RenderTextureRef.width, RenderTextureRef.height, TextureFormat.RGB24, false);
        RenderTexture.active = RenderTextureRef;
        tex.ReadPixels(new Rect(0, 0, RenderTextureRef.width, RenderTextureRef.height), 0, 0);
        tex.Apply();

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        Object.Destroy(tex);

        //Write to a file in the project folder
        File.WriteAllBytes(Application.dataPath + "/SavedScreen" + this.timer_num2 + ".png", bytes);

    }

    public bool target_judgment()
    {
        return judg_koudai;
    }

}
