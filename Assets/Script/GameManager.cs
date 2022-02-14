using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Vector3 clickDownPos,clickUpPos;
    public CameraControl cameraSelect;
    public bool needSlide = false;

    public float slideAngle, slideAngleMax,clickDownAngle ;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickDownPos = Input.mousePosition;
            slideAngleMax = PrefabAssign.instance.angle*1.2f;
            clickDownAngle = PrefabAssign.instance.gameObject.transform.rotation.eulerAngles.y;
            needSlide = true;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            clickUpPos = Input.mousePosition;
            needSlide = false;
            CheckStatusChange();
        }

        if (needSlide)
        {
            
            float deltaX = clickDownPos.x - Input.mousePosition.x;
            slideAngle = (deltaX / 60f) * slideAngleMax;
            if (slideAngle >= slideAngleMax)
            {
                slideAngle = slideAngleMax;
            }
            else if (slideAngle <= -slideAngleMax)
            {
                slideAngle = -slideAngleMax;
            }
            Debug.Log("in slde " + slideAngle+ " clickAngle " + clickDownAngle );
            PrefabAssign.instance.gameObject.transform.rotation = Quaternion.Euler(0f, (clickDownAngle + slideAngle * 180f / Mathf.PI), 0f);
        }
        
    }

    public void CheckStatusChange()
    {
        if (cameraSelect.status==CameraStatus.topView && clickDownPos.x < clickUpPos.x) //topView ������ sideView
        {
            Debug.Log("to status 0");
            cameraSelect.CameraStatusChange(1);
        }
        else if (cameraSelect.status == CameraStatus.sideViewOne /*&& clickDownPos.x < clickUpPos.x*/) // sideVieew �ƹ��k��
        {
            //cameraSelect.ChangeToNextInSide(1);
            float deltaX = Mathf.Abs(clickDownPos.x - clickUpPos.x);
            float deltaY = Mathf.Abs(clickDownPos.y - clickUpPos.y);
            float distane = (clickUpPos - clickDownPos).magnitude;
            Debug.Log("X = " + deltaX + ", Z = " + deltaY);
            //�P�_�O���k�첾�Ϊ̬O�W�U�첾
            if (deltaX > deltaY) //���k����
            {
                if (clickDownPos.x - clickUpPos.x < 0) //�ƹ��k��
                {

                    cameraSelect.ChangeToNextInSide(-1);
                }
                else //�ƹ�����
                {
                    cameraSelect.ChangeToNextInSide(1);
                }
            }
            else if(distane <0.05f)//�I��
            {
                cameraSelect.CameraStatusChange(2);
            }

        }
        else if (cameraSelect.status == CameraStatus.closeUpView && clickDownPos.y < clickUpPos.y) // closeUpView ������ sideView
        {
            //cameraSelect.ChangeToNextInSide(1);
            float deltaX = Mathf.Abs(clickDownPos.x - clickUpPos.x);
            float deltaY = Mathf.Abs(clickDownPos.y - clickUpPos.y);
            if (deltaY > deltaX) // Y��V�첾��X��V�h
            {
                cameraSelect.CameraStatusChange(1);
            }
            
        }
        /*
        else if (cameraSelect.status == CameraStatus.sideViewOne && clickDownPos.x > clickUpPos.x) // sideVieew �ƹ�����
        {
            cameraSelect.ChangeToNextInSide(-1);
        }
        else if (cameraSelect.status == CameraStatus.sideViewOne && clickDownPos.z < clickUpPos.z) // sideVieew ������ closeView
        {
            cameraSelect.CameraStatusChange(2);
        }
        */



    }
}
