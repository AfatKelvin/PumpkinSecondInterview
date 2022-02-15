using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CameraStatus //�۾����A
{
    idle, //��l���A
    topView, //�q���W���
    sideViewOne, //�����s���Ҧ�
    closeUpView, // �S�g�Ҧ�
    cameraMoving //�۾����ʤ��Ҧ�
}
public class CameraControl : MonoBehaviour
{
    public  CameraStatus status; //�۾����A

    public Vector3 iniPos, iniAngle;// ��l���� �y�� (top view mode) 

    //public List<int> prefabList; //����M��s�� 

    public int currentObj; // ��e�s���s��

    public Vector3 origine; //�����I
    
    // Start is called before the first frame update
    void Start()
    {
        origine = new Vector3(0f, 0f, 0f); //�۾��ؼг]�� �@�ɮy�Ф���
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void CameraStatusChange(int statusNum)  // 0 > topView, 1 > sideView, 2 > closeUpView
    {
        if (statusNum ==0) //������topView�Ҧ�
        {
            status = CameraStatus.topView; //������topView�Ҧ�
            gameObject.transform.position = iniPos; //�����ܪ�l��m
            gameObject.transform.rotation = Quaternion.Euler(iniAngle); //�����ܪ�l����
        }
        else if (statusNum == 1) //������sideView�Ҧ�
        {
            //�P�_�ݭn���ت��۾�����
            bool needCloseToSide = false; //�ݭn�q�S�g > sideView
            bool needTopToSide = false; //�ݭn�qtop > sideView
            // �̷ӷ�e���A�P�_ bool
            if (status == CameraStatus.topView)
            {
                needTopToSide = true;
            }
            else if (status == CameraStatus.closeUpView)
            {
                needCloseToSide = true;
            }
            //������ sideView �Ҧ�
            status = CameraStatus.sideViewOne;
            //����U�۬۾����ʼҦ�
            if (needCloseToSide)
            {
                StartCoroutine(CloseUpViewOut_IE(PrefabAssign.instance.gameObject.transform.GetChild(currentObj).gameObject.transform));
            }
            else if (needTopToSide)
            {
                StartCoroutine(TopToSide_IE(PrefabAssign.instance.gameObject.transform.GetChild(currentObj).gameObject.transform));
            }
        }
        else if (statusNum == 2) //������closeView�Ҧ�
        {
            status = CameraStatus.closeUpView; //������closeView�Ҧ�
            //����۾�����
            StartCoroutine(CloseUpViewIn_IE(PrefabAssign.instance.gameObject.transform.GetChild(currentObj).gameObject.transform)); 
        }
    }

    //sideView �Ҧ� ��ܥ��ƩΪ̥k�� �P�_
    public void ChangeToNextInSide(int num) 
    {
        currentObj += num; //������e�ݪ�����
        //����currentObj �쥿�T���Ʀr
        if (currentObj >= PrefabAssign.instance.circleObjs.Count)
        {
            currentObj = 0;
        }
        else if (currentObj < 0 )
        {
            currentObj = PrefabAssign.instance.circleObjs.Count - 1;
        }
        //������s������ + �_�ʯS��
        //CameraStatusChange(1);
        PrefabAssign.instance.Damping();
    }

    //�����إ�
    public void PrefabPositionSet() 
    {
        iniPos = gameObject.transform.position; //�]�w��l�y��
        iniAngle = gameObject.transform.rotation.eulerAngles; //�]�w��l����
        CameraStatusChange(0); //�����۾��Ҧ�
    }

    //�۾����� side > close 
    public IEnumerator CloseUpViewIn_IE(Transform objTransform) 
    {
        status = CameraStatus.cameraMoving;
        Vector3 closeViewV3 = new Vector3(objTransform.position.x * 1.5f, objTransform.position.y * 1.5f, objTransform.position.z * 1.5f); //closeView �۾���m
        Vector3 sideViewV3 = new Vector3(objTransform.position.x*2f, objTransform.position.y*2f, objTransform.position.z*2f); //sideView �۾���m
        Vector3 stepVector3 = new Vector3(closeViewV3.x - sideViewV3.x, closeViewV3.y - sideViewV3.y, closeViewV3.z - sideViewV3.z)/40f; //�C�����ʶZ��
        int counter = 0;

        while (counter<40)
        {
            gameObject.transform.position += stepVector3; //���ʬ۾�

            yield return new WaitForSeconds(0.05f);
            counter += 1;
        }
        status = CameraStatus.closeUpView;
    }

    public void CloseUpToSide(Transform objTransform) 
    {
        StartCoroutine(CloseUpViewOut_IE(objTransform));
    }
    //�۾����� close > side 
    public IEnumerator CloseUpViewOut_IE(Transform objTransform)
    {
        status = CameraStatus.cameraMoving;
        Vector3 closeViewV3 = new Vector3(objTransform.position.x * 1.5f, objTransform.position.y * 1.5f, objTransform.position.z * 1.5f); //closeView �۾���m
        Vector3 sideViewV3 = new Vector3(objTransform.position.x * 2f, objTransform.position.y * 2f, objTransform.position.z * 2f); //sideView �۾���m
        Vector3 stepVector3 = new Vector3( sideViewV3.x - closeViewV3.x ,  sideViewV3.y - closeViewV3.y ,  sideViewV3.z - closeViewV3.z ) / 40f; //�C�����ʶZ��
        int counter = 0;

        while (counter < 40)
        {
            gameObject.transform.position += stepVector3; //���ʬ۾�

            yield return new WaitForSeconds(0.05f);
            counter += 1;
        }
        status = CameraStatus.sideViewOne; //�����۾��Ҧ�
    }

    //�۾����� top > side 
    public IEnumerator TopToSide_IE(Transform objTransform)
    {
        status = CameraStatus.cameraMoving;
        Vector3 sideViewV3 = new Vector3(objTransform.position.x * 2f, objTransform.position.y * 2f, objTransform.position.z * 2f); //sideView �۾���m

        Vector3 stepVector3 = new Vector3(sideViewV3.x - iniPos.x, sideViewV3.y -iniPos.y, sideViewV3.z - iniPos.z) / 40f; //�C�����ʶZ��

        int counter = 0;

        while (counter < 40)
        {
            gameObject.transform.LookAt(origine); // �۾��]�w���ݦV�����I
            
            gameObject.transform.position += stepVector3; //���ʬ۾�

            yield return new WaitForSeconds(0.05f);
            counter += 1;
        }
        
        status = CameraStatus.sideViewOne; // �����۾��Ҧ�
        gameObject.transform.position = sideViewV3; // �T�w�̫��m
    }

    public IEnumerator SideToTop_IE(Transform objTransform)
    {
        status = CameraStatus.cameraMoving;
        Vector3 sideViewV3 = new Vector3(objTransform.position.x * 2f, objTransform.position.y * 2f, objTransform.position.z * 2f); //sideView �۾���m

        Vector3 stepVector3 = new Vector3(iniPos.x-sideViewV3.x, iniPos.y-sideViewV3.y, iniPos.z-sideViewV3.z) / 40f; //�]�w���ʶZ��

        int counter = 0;

        while (counter < 40)
        {
            gameObject.transform.LookAt(origine); // �۾��]�w���ݦV�����I

            gameObject.transform.position += stepVector3; //���ʬ۾�

            yield return new WaitForSeconds(0.05f);
            counter += 1;
        }
        status = CameraStatus.topView; // �����۾��Ҧ�
        gameObject.transform.position = iniPos; // �T�w�̫��m
    }
}
