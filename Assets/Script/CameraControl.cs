using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CameraStatus
{
    topView,
    sideViewOne,
    closeUpView
}
public class CameraControl : MonoBehaviour
{
    public  CameraStatus status;

    public Vector3 iniPos,iniAngle,sidePos, closePos;

    public List<int> prefabList;

    public int currentObj;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.A))
        {
            CameraStatusChange(0);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            CameraStatusChange(1);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CameraStatusChange(2);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeToNextInSide(1);
        }
        */
    }

    public void CameraStatusChange(int statusNum)  // 0 > topView, 1 > sideView, 2 > closeUpView
    {
        if (statusNum ==0)
        {
            status = CameraStatus.topView;
            gameObject.transform.position = iniPos; //�����ܪ�l��m
            gameObject.transform.rotation = Quaternion.Euler(iniAngle); //�����ܪ�l����
        }
        else if (statusNum == 1)
        {
            status = CameraStatus.sideViewOne;
            Debug.Log("currentObj = " + currentObj + "count = " + PrefabAssign.instance.gameObject.transform.childCount);
            SideViewSetUp(PrefabAssign.instance.gameObject.transform.GetChild(currentObj).gameObject.transform);
        }
        else if (statusNum == 2)
        {
            status = CameraStatus.closeUpView;
            CloseViewSetUp(PrefabAssign.instance.gameObject.transform.GetChild(currentObj).gameObject.transform);
        }
    }

    public void SideViewSetUp(Transform objTransform) 
    {
        Vector3 temp = new Vector3(objTransform.position.x * 2f, objTransform.position.y, objTransform.position.z * 2f); //�]�w�y�мƭ�
        gameObject.transform.position = temp; //�����y��

        //�]�w����
        gameObject.transform.LookAt(objTransform);
    }

    public void CloseViewSetUp(Transform objTransform)
    {
        Vector3 temp = new Vector3(objTransform.position.x * 1.3f, objTransform.position.y, objTransform.position.z * 1.3f); //�]�w�y�мƭ�
        gameObject.transform.position = temp; //�����y��

        //�]�w����
        gameObject.transform.LookAt(objTransform);
    }

    public void ChangeToNextInSide(int num) 
    {
        Debug.Log("num = " + currentObj);
        currentObj += num;
        Debug.Log("num = " + currentObj);
        if (currentObj >= prefabList.Count)
        {
            currentObj = 0;
            Debug.Log("numtop = " + currentObj);
        }
        else if (currentObj < 0 )
        {
            currentObj = prefabList.Count-1;
            Debug.Log("numbot = " + currentObj);
        }

        CameraStatusChange(1);
        PrefabAssign.instance.Damping();
    }

    public void PrefabPositionSet() 
    {
        iniPos = gameObject.transform.position; //�]�w��l�y��
        iniAngle = gameObject.transform.rotation.eulerAngles; //�]�w��l����
        for (int i = 0; i < PrefabAssign.instance.gameObject.transform.childCount; i++) //�O���Ҧ����έy�D�W�Ҧ�prefab
        {
            prefabList.Add(i);
        }
    }
}
