using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Vector3 clickDownPos,clickUpPos;
    public CameraControl cameraSelect;
    public bool needSlide = false;
    public bool firstLoad = true;

    public float slideAngle, slideAngleMax,clickDownAngle ;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraSelect.status == CameraStatus.cameraMoving || cameraSelect.status == CameraStatus.idle)
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
                CheckStatusChange();
            }
           
        }

        if (needSlide && cameraSelect.status == CameraStatus.sideViewOne)
        {
            
            float deltaX = clickDownPos.x - Input.mousePosition.x; //紀錄滑鼠移動的距離 再以一定的比例竊換成角度旋轉
            slideAngle = (deltaX / 60f) * slideAngleMax;
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
        //cameraSelect.ChangeToNextInSide(1);
        float deltaX = Mathf.Abs(clickDownPos.x - clickUpPos.x);
        float deltaY = Mathf.Abs(clickDownPos.y - clickUpPos.y);
        float distane = (clickUpPos - clickDownPos).magnitude;

        if (cameraSelect.status==CameraStatus.topView && clickDownPos.x < clickUpPos.x) //topView 切換成 sideView
        {
            if (distane >1f) // X位移量夠大才會被判斷要切換mode
            {
                cameraSelect.CameraStatusChange(1);
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
                    cameraSelect.ChangeToNextInSide(-1);
                }
                else //滑鼠左移
                {//移動到定點轉場
                    PrefabAssign.instance.gameObject.transform.rotation = Quaternion.Euler(0f, (clickDownAngle + slideAngleMax * 180f / Mathf.PI), 0f); //角度移動
                    cameraSelect.ChangeToNextInSide(1);
                }
            }
            else if(distane <0.05f)//點擊
            {
                cameraSelect.CameraStatusChange(2);
            }

        }
        else if (cameraSelect.status == CameraStatus.closeUpView && clickDownPos.y < clickUpPos.y) // closeUpView 切換成 sideView
        {
            //cameraSelect.ChangeToNextInSide(1);
            //float deltaX = Mathf.Abs(clickDownPos.x - clickUpPos.x);
            //float deltaY = Mathf.Abs(clickDownPos.y - clickUpPos.y);
            if (deltaY > deltaX) // Y方向位移比X方向多
            {
                cameraSelect.CameraStatusChange(1);
            }
        }
        /*
        else if (cameraSelect.status == CameraStatus.sideViewOne && clickDownPos.x > clickUpPos.x) // sideVieew 滑鼠左移
        {
            cameraSelect.ChangeToNextInSide(-1);
        }
        else if (cameraSelect.status == CameraStatus.sideViewOne && clickDownPos.z < clickUpPos.z) // sideVieew 切換成 closeView
        {
            cameraSelect.CameraStatusChange(2);
        }
        */



    }
}
