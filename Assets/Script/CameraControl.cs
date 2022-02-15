using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CameraStatus
{
    idle,
    topView,
    sideViewOne,
    closeUpView,
    cameraMoving
}
public class CameraControl : MonoBehaviour
{
    public  CameraStatus status;

    public Vector3 iniPos,iniAngle,sidePos, closePos;

    public List<int> prefabList;

    public int currentObj;

    public Vector3 origine;
    
    // Start is called before the first frame update
    void Start()
    {
        origine = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void CameraStatusChange(int statusNum)  // 0 > topView, 1 > sideView, 2 > closeUpView
    {
        Debug.Log("statusNum A = " + statusNum);
        if (statusNum ==0)
        {
            Debug.Log("statusNum B1 = " + statusNum);
            status = CameraStatus.topView;
            //StartCoroutine(SideToTop_IE(PrefabAssign.instance.gameObject.transform.GetChild(currentObj).gameObject.transform));
            
            gameObject.transform.position = iniPos; //切換至初始位置
            gameObject.transform.rotation = Quaternion.Euler(iniAngle); //切換至初始角度
            Debug.Log("statusNum B2 = " + statusNum);
        }
        else if (statusNum == 1)
        {
            //判斷需要哪種相機移動
            bool needCloseToSide = false;
            bool needTopToSide = false;
            if (status == CameraStatus.topView)
            {
                needTopToSide = true;
            }
            else if (status == CameraStatus.closeUpView)
            {
                needCloseToSide = true;
            }
            //切換成 sideView 模式
            status = CameraStatus.sideViewOne;
            if (needCloseToSide)
            {
                StartCoroutine(CloseUpViewOut_IE(PrefabAssign.instance.gameObject.transform.GetChild(currentObj).gameObject.transform));
            }
            else if (needTopToSide)
            {
                StartCoroutine(TopToSide_IE(PrefabAssign.instance.gameObject.transform.GetChild(currentObj).gameObject.transform));
                //SideViewSetUp(PrefabAssign.instance.gameObject.transform.GetChild(currentObj).gameObject.transform);
            }
        }
        else if (statusNum == 2)
        {
            status = CameraStatus.closeUpView;
            StartCoroutine(CloseUpViewIn_IE(PrefabAssign.instance.gameObject.transform.GetChild(currentObj).gameObject.transform));
            //CloseViewSetUp(PrefabAssign.instance.gameObject.transform.GetChild(currentObj).gameObject.transform);
        }
    }

    public void SideViewSetUp(Transform objTransform) 
    {
        Vector3 temp = new Vector3(objTransform.position.x * 2f, objTransform.position.y, objTransform.position.z * 2f); //設定座標數值
        gameObject.transform.position = temp; //切換座標

        //設定角度
        gameObject.transform.LookAt(objTransform);
    }

    public void CloseViewSetUp(Transform objTransform)
    {
        Vector3 temp = new Vector3(objTransform.position.x * 1.5f, objTransform.position.y, objTransform.position.z * 1.5f); //設定座標數值
        gameObject.transform.position = temp; //切換座標

        //設定角度
        gameObject.transform.LookAt(objTransform);
    }

    public void ChangeToNextInSide(int num) 
    {
        currentObj += num;
        if (currentObj >= prefabList.Count)
        {
            currentObj = 0;
        }
        else if (currentObj < 0 )
        {
            currentObj = prefabList.Count-1;
        }

        CameraStatusChange(1);
        PrefabAssign.instance.Damping();
    }

    public void PrefabPositionSet() 
    {
        iniPos = gameObject.transform.position; //設定初始座標
        iniAngle = gameObject.transform.rotation.eulerAngles; //設定初始角度
        for (int i = 0; i < PrefabAssign.instance.gameObject.transform.childCount; i++) //記錄所有環形軌道上所有prefab
        {
            prefabList.Add(i);
        }
        CameraStatusChange(0);
    }

    public IEnumerator CloseUpViewIn_IE(Transform objTransform) 
    {
        status = CameraStatus.cameraMoving;
        Vector3 closeViewV3 = new Vector3(objTransform.position.x * 1.5f, objTransform.position.y * 1.5f, objTransform.position.z * 1.5f); //closeView 相機位置
        Vector3 sideViewV3 = new Vector3(objTransform.position.x*2f, objTransform.position.y*2f, objTransform.position.z*2f); //sideView 相機位置
        
        Vector3 stepVector3 = new Vector3(closeViewV3.x - sideViewV3.x, closeViewV3.y - sideViewV3.y, closeViewV3.z - sideViewV3.z)/40f;
        int counter = 0;

        while (counter<40)
        {
            gameObject.transform.position += stepVector3;

            yield return new WaitForSeconds(0.05f);
            counter += 1;
            Debug.Log("counter =" + counter + "_" + gameObject.transform.position.x);
        }
        status = CameraStatus.closeUpView;
    }

    public void CloseUpToSide(Transform objTransform) 
    {
        StartCoroutine(CloseUpViewOut_IE(objTransform));
    }

    public IEnumerator CloseUpViewOut_IE(Transform objTransform)
    {
        status = CameraStatus.cameraMoving;
        Vector3 closeViewV3 = new Vector3(objTransform.position.x * 1.5f, objTransform.position.y * 1.5f, objTransform.position.z * 1.5f); //closeView 相機位置
        Vector3 sideViewV3 = new Vector3(objTransform.position.x * 2f, objTransform.position.y * 2f, objTransform.position.z * 2f); //sideView 相機位置

        Vector3 stepVector3 = new Vector3( sideViewV3.x - closeViewV3.x ,  sideViewV3.y - closeViewV3.y ,  sideViewV3.z - closeViewV3.z ) / 40f;
        int counter = 0;

        while (counter < 40)
        {
            gameObject.transform.position += stepVector3;

            yield return new WaitForSeconds(0.05f);
            counter += 1;
        }
        status = CameraStatus.sideViewOne;
    }

    public IEnumerator TopToSide_IE(Transform objTransform)
    {
        status = CameraStatus.cameraMoving;
        Vector3 sideViewV3 = new Vector3(objTransform.position.x * 2f, objTransform.position.y * 2f, objTransform.position.z * 2f); //sideView 相機位置

        Vector3 stepVector3 = new Vector3(sideViewV3.x - iniPos.x, sideViewV3.y -iniPos.y, sideViewV3.z - iniPos.z) / 40f;

        int counter = 0;

        while (counter < 40)
        {
            gameObject.transform.LookAt(origine);
            
            gameObject.transform.position += stepVector3;

            yield return new WaitForSeconds(0.05f);
            counter += 1;
        }
        status = CameraStatus.sideViewOne;
    }

    public IEnumerator SideToTop_IE(Transform objTransform)
    {
        status = CameraStatus.cameraMoving;
        Vector3 sideViewV3 = new Vector3(objTransform.position.x * 2f, objTransform.position.y * 2f, objTransform.position.z * 2f); //sideView 相機位置

        Vector3 stepVector3 = new Vector3(iniPos.x-sideViewV3.x, iniPos.y-sideViewV3.y, iniPos.z-sideViewV3.z) / 40f;

        int counter = 0;

        while (counter < 40)
        {
            gameObject.transform.LookAt(origine);

            gameObject.transform.position += stepVector3;

            yield return new WaitForSeconds(0.05f);
            counter += 1;
        }
        status = CameraStatus.topView;
    }
}
