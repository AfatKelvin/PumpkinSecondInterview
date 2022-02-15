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
            StopAllCoroutines();//�פ�Ҧ���{ 
            clickDownPos = Input.mousePosition; //�����ƹ��I����m
            slideAngleMax = PrefabAssign.instance.angle*1.25f; // sideViewMode ���ץi�Q���ʪ��W��
            clickDownAngle = PrefabAssign.instance.gameObject.transform.rotation.eulerAngles.y; // �O����e��m������
            needSlide = true; //�������ݭn���� in sideViewMode
            firstLoad = false;

        }
        else if(Input.GetMouseButtonUp(0))
        {
            if (firstLoad==false)
            {
                clickUpPos = Input.mousePosition; //�����ƹ����}�I����m
                needSlide = false; //������ ���ݭn ���� in sideViewMode
                CheckStatusChange();
            }
           
        }

        if (needSlide && cameraSelect.status == CameraStatus.sideViewOne)
        {
            
            float deltaX = clickDownPos.x - Input.mousePosition.x; //�����ƹ����ʪ��Z�� �A�H�@�w������Ѵ������ױ���
            slideAngle = (deltaX / 60f) * slideAngleMax;
            if (slideAngle >= slideAngleMax) //�̦h�u�����U�@��/�W�@�� ���󪺦�m����
            {
                slideAngle = slideAngleMax;
            }
            else if (slideAngle <= -slideAngleMax)
            {
                slideAngle = -slideAngleMax;
            }
            PrefabAssign.instance.gameObject.transform.rotation = Quaternion.Euler(0f, (clickDownAngle + slideAngle * 180f / Mathf.PI), 0f); //���ײ��ʤ���
        }
        
    }

    public void CheckStatusChange()
    {
        //cameraSelect.ChangeToNextInSide(1);
        float deltaX = Mathf.Abs(clickDownPos.x - clickUpPos.x);
        float deltaY = Mathf.Abs(clickDownPos.y - clickUpPos.y);
        float distane = (clickUpPos - clickDownPos).magnitude;

        if (cameraSelect.status==CameraStatus.topView && clickDownPos.x < clickUpPos.x) //topView ������ sideView
        {
            if (distane >1f) // X�첾�q���j�~�|�Q�P�_�n����mode
            {
                cameraSelect.CameraStatusChange(1);
            }
        }
        else if (cameraSelect.status == CameraStatus.sideViewOne /*&& clickDownPos.x < clickUpPos.x*/) // sideVieew �ƹ��k��
        {
            //�P�_�O���k�첾�Ϊ̬O�W�U�첾
            if (deltaX > deltaY) //���k����
            {
                
                // Damping�S�����
                if (clickDownPos.x - clickUpPos.x < 0) //�ƹ��k��
                {
                    //���ʨ�w�I���
                    PrefabAssign.instance.gameObject.transform.rotation = Quaternion.Euler(0f, (clickDownAngle - slideAngleMax * 180f / Mathf.PI), 0f); //���ײ���
                    cameraSelect.ChangeToNextInSide(-1);
                }
                else //�ƹ�����
                {//���ʨ�w�I���
                    PrefabAssign.instance.gameObject.transform.rotation = Quaternion.Euler(0f, (clickDownAngle + slideAngleMax * 180f / Mathf.PI), 0f); //���ײ���
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
            //float deltaX = Mathf.Abs(clickDownPos.x - clickUpPos.x);
            //float deltaY = Mathf.Abs(clickDownPos.y - clickUpPos.y);
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
