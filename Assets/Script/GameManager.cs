using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Vector3 clickDownPos,clickUpPos; //滑鼠左鍵 點下/放開 時的座標
    public CameraControl cameraSelect; // 選用相機
    public bool needSlide = false; //在sideView模式判斷是否需要滑動
    public bool firstLoad = true; //判斷是否為初次載入場景
    public Text modeText; //相機模式提示
    public float slideAngle, slideAngleMax,clickDownAngle ; //旋轉角度(sideView 滑動滑鼠時) 最大可旋轉角度
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraSelect.status == CameraStatus.cameraMoving || cameraSelect.status == CameraStatus.idle) // idle 及 相機移動時 不判斷滑鼠座標
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();//終止所有協程 
            clickDownPos = Input.mousePosition; //紀錄滑鼠點擊位置
            slideAngleMax = PrefabAssign.instance.angle*1.25f; // sideViewMode 角度可被移動的上限
            clickDownAngle = PrefabAssign.instance.gameObject.transform.rotation.eulerAngles.y; // 記錄當前位置的角度
            needSlide = true; //切換成需要旋轉 in sideViewMode
            firstLoad = false;

        }
        else if(Input.GetMouseButtonUp(0))
        {
            if (firstLoad==false)
            {
                clickUpPos = Input.mousePosition; //紀錄滑鼠離開點擊位置
                needSlide = false; //切換成 不需要 旋轉 in sideViewMode
                CheckStatusChange(); //判斷要切換成甚麼模式
            }
           
        }

        if (needSlide && cameraSelect.status == CameraStatus.sideViewOne) //sideView 時 滑動一動時造成的環型 prefab 旋轉 
        {
            float deltaX = clickDownPos.x - Input.mousePosition.x; //紀錄滑鼠移動的距離 再以一定的比例竊換成角度旋轉
            slideAngle = (deltaX / 60f) * slideAngleMax; //滑動距離對應到旋轉 角度的計算比例
            if (slideAngle >= slideAngleMax) //最多只能轉到下一個/上一個 物件的位置角度
            {
                slideAngle = slideAngleMax;
            }
            else if (slideAngle <= -slideAngleMax)
            {
                slideAngle = -slideAngleMax;
            }
            PrefabAssign.instance.gameObject.transform.rotation = Quaternion.Euler(0f, (clickDownAngle + slideAngle * 180f / Mathf.PI), 0f); //角度移動切換
        }
    }

    public void CheckStatusChange()
    {
        float deltaX = Mathf.Abs(clickDownPos.x - clickUpPos.x); //滑鼠X方向位移
        float deltaY = Mathf.Abs(clickDownPos.y - clickUpPos.y); //滑鼠Y方向位移
        float distane = (clickUpPos - clickDownPos).magnitude;  //滑鼠位移距離

        if (cameraSelect.status==CameraStatus.topView && clickDownPos.x < clickUpPos.x) //topView 切換成 sideView
        {
            if (distane >1f) // X位移量夠大才會被判斷要切換mode
            {
                cameraSelect.CameraStatusChange(1); 
                modeText.text = "環形瀏覽";
            }
        }
        else if (cameraSelect.status == CameraStatus.sideViewOne /*&& clickDownPos.x < clickUpPos.x*/) // sideVieew 滑鼠右移
        {
            //判斷是左右位移或者是上下位移
            if (deltaX > deltaY) //左右移動
            {
                // Damping特效轉場
                if (clickDownPos.x - clickUpPos.x < 0) //滑鼠右移
                {
                    //移動到定點轉場
                    PrefabAssign.instance.gameObject.transform.rotation = Quaternion.Euler(0f, (clickDownAngle - slideAngleMax * 180f / Mathf.PI), 0f); //角度移動
                    cameraSelect.ChangeToNextInSide(-1); //切換至前一個物件
                }
                else //滑鼠左移
                {//移動到定點轉場
                    PrefabAssign.instance.gameObject.transform.rotation = Quaternion.Euler(0f, (clickDownAngle + slideAngleMax * 180f / Mathf.PI), 0f); //角度移動
                    cameraSelect.ChangeToNextInSide(1); //切換至後一個物件
                }
            }
            else if(distane <0.05f)//點擊
            {
                cameraSelect.CameraStatusChange(2);
                modeText.text = "特寫";
            }
        }
        else if (cameraSelect.status == CameraStatus.closeUpView && clickDownPos.y < clickUpPos.y) // closeUpView 切換成 sideView
        {
            if (deltaY > deltaX) // Y方向位移比X方向多
            {
                cameraSelect.CameraStatusChange(1);
                modeText.text = "環形瀏覽";
            }
        }
    }

    //關閉程式
    public void Leave() 
    {
        Application.Quit();
    }

}
