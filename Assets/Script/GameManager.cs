using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Vector3 clickDownPos,clickUpPos;
    bool needChageStatus;
    bool needChangeWatchObj;
    public CameraControl cameraSelect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickDownPos = Input.mousePosition;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            clickUpPos = Input.mousePosition;
            CheckStatusChange();
        }
    }

    public void CheckStatusChange()
    {
        if (cameraSelect.status==CameraStatus.topView && clickDownPos.x < clickUpPos.x) //topView 切換成 sideView
        {
            Debug.Log("to status 0");
            cameraSelect.CameraStatusChange(1);
        }
        else if (cameraSelect.status == CameraStatus.sideViewOne /*&& clickDownPos.x < clickUpPos.x*/) // sideVieew 滑鼠右移
        {
            //cameraSelect.ChangeToNextInSide(1);
            float deltaX = Mathf.Abs(clickDownPos.x - clickUpPos.x);
            float deltaY = Mathf.Abs(clickDownPos.y - clickUpPos.y);
            float distane = (clickUpPos - clickDownPos).magnitude;
            Debug.Log("X = " + deltaX + ", Z = " + deltaY);
            //判斷是左右位移或者是上下位移
            if (deltaX > deltaY) //左右移動
            {
                if (clickDownPos.x - clickUpPos.x < 0) //滑鼠右移
                {

                    cameraSelect.ChangeToNextInSide(-1);
                }
                else //滑鼠左移
                {
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
            float deltaX = Mathf.Abs(clickDownPos.x - clickUpPos.x);
            float deltaY = Mathf.Abs(clickDownPos.y - clickUpPos.y);
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
