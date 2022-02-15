using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Vector3 clickDownPos,clickUpPos; //�ƹ����� �I�U/��} �ɪ��y��
    public CameraControl cameraSelect; // ��ά۾�
    public bool needSlide = false; //�bsideView�Ҧ��P�_�O�_�ݭn�ư�
    public bool firstLoad = true; //�P�_�O�_���즸���J����
    public Text modeText; //�۾��Ҧ�����
    public float slideAngle, slideAngleMax,clickDownAngle ; //���ਤ��(sideView �ưʷƹ���) �̤j�i���ਤ��
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraSelect.status == CameraStatus.cameraMoving || cameraSelect.status == CameraStatus.idle) // idle �� �۾����ʮ� ���P�_�ƹ��y��
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
                CheckStatusChange(); //�P�_�n�������ƻ�Ҧ�
            }
           
        }

        if (needSlide && cameraSelect.status == CameraStatus.sideViewOne) //sideView �� �ưʤ@�ʮɳy�������� prefab ���� 
        {
            float deltaX = clickDownPos.x - Input.mousePosition.x; //�����ƹ����ʪ��Z�� �A�H�@�w������Ѵ������ױ���
            slideAngle = (deltaX / 60f) * slideAngleMax; //�ưʶZ����������� ���ת��p����
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
        float deltaX = Mathf.Abs(clickDownPos.x - clickUpPos.x); //�ƹ�X��V�첾
        float deltaY = Mathf.Abs(clickDownPos.y - clickUpPos.y); //�ƹ�Y��V�첾
        float distane = (clickUpPos - clickDownPos).magnitude;  //�ƹ��첾�Z��

        if (cameraSelect.status==CameraStatus.topView && clickDownPos.x < clickUpPos.x) //topView ������ sideView
        {
            if (distane >1f) // X�첾�q���j�~�|�Q�P�_�n����mode
            {
                cameraSelect.CameraStatusChange(1); 
                modeText.text = "�����s��";
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
                    cameraSelect.ChangeToNextInSide(-1); //�����ܫe�@�Ӫ���
                }
                else //�ƹ�����
                {//���ʨ�w�I���
                    PrefabAssign.instance.gameObject.transform.rotation = Quaternion.Euler(0f, (clickDownAngle + slideAngleMax * 180f / Mathf.PI), 0f); //���ײ���
                    cameraSelect.ChangeToNextInSide(1); //�����ܫ�@�Ӫ���
                }
            }
            else if(distane <0.05f)//�I��
            {
                cameraSelect.CameraStatusChange(2);
                modeText.text = "�S�g";
            }
        }
        else if (cameraSelect.status == CameraStatus.closeUpView && clickDownPos.y < clickUpPos.y) // closeUpView ������ sideView
        {
            if (deltaY > deltaX) // Y��V�첾��X��V�h
            {
                cameraSelect.CameraStatusChange(1);
                modeText.text = "�����s��";
            }
        }
    }

    //�����{��
    public void Leave() 
    {
        Application.Quit();
    }

}
