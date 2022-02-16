using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CameraStatus //相機狀態
{
    idle, //初始狀態
    topView, //從正上方看
    sideViewOne, //環形瀏覽模式
    closeUpView, // 特寫模式
    cameraMoving //相機移動中模式
}
public class CameraControl : MonoBehaviour
{
    public  CameraStatus status; //相機狀態

    public Vector3 iniPos, iniAngle;// 初始角度 座標 (top view mode) 

    //public List<int> prefabList; //物件清單編號 

    public int currentObj; // 當前瀏覽編號

    public Vector3 origine; //中心點

    public float midSite;

    public List<Vector3> midStep;

    public List<Vector3> midVector;

    public Vector3 currentVector;
    
    // Start is called before the first frame update
    void Start()
    {
        origine = new Vector3(0f, 0f, 0f); //世界中心座標
        iniPos = gameObject.transform.position; //設定初始座標
        iniAngle = gameObject.transform.rotation.eulerAngles; //設定初始角度
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void CameraStatusChange(int statusNum)  // 0 > topView, 1 > sideView, 2 > closeUpView
    {
        if (statusNum ==0) //切換成topView模式
        {
            status = CameraStatus.topView; //切換成topView模式
            gameObject.transform.position = iniPos; //切換至初始位置
            gameObject.transform.rotation = Quaternion.Euler(iniAngle); //切換至初始角度
        }
        else if (statusNum == 1) //切換成sideView模式
        {
            //判斷需要哪種的相機移動
            bool needCloseToSide = false; //需要從特寫 > sideView
            bool needTopToSide = false; //需要從top > sideView
            // 依照當前狀態判斷 bool
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
            //執行各自相機移動模式
            if (needCloseToSide)
            {
                StartCoroutine(CloseUpViewOut_IE(GameManager.instance.gameObject.transform.GetChild(currentObj).gameObject.transform));
            }
            else if (needTopToSide)
            {
                StartCoroutine(TopToSide_IE(GameManager.instance.gameObject.transform.GetChild(currentObj).gameObject.transform));
            }
        }
        else if (statusNum == 2) //切換成closeView模式
        {
            status = CameraStatus.closeUpView; //切換成closeView模式
            //執行相機移動
            StartCoroutine(CloseUpViewIn_IE(GameManager.instance.gameObject.transform.GetChild(currentObj).gameObject.transform)); 
        }
    }

    //sideView 模式 選擇左滑或者右滑 判斷
    public void ChangeToNextInSide(int num) 
    {
        Debug.Log("current1 = " + currentObj);
        currentObj += num; //切換當前看的物件
        //限制currentObj 到正確的數字
        if (currentObj >= GameManager.instance.circleObjs.Count)
        {
            currentObj = 0;
        }
        else if (currentObj < 0 )
        {
            currentObj = GameManager.instance.circleObjs.Count - 1;
        }
        Debug.Log("current2 = " + currentObj);

        //切換到新的物件 + 震動特效
        //CameraStatusChange(1);
        CameraSlideDamping();
        //切換到新的物件 + 震動特效
        //CameraStatusChange(1);
        //PrefabAssign.instance.Damping();
    }

    //場景建立
    public void PrefabPositionSet() 
    {
        iniPos = gameObject.transform.position; //設定初始座標
        iniAngle = gameObject.transform.rotation.eulerAngles; //設定初始角度
        CameraStatusChange(0); //切換相機模式
    }

    //相機移動 side > close 
    public IEnumerator CloseUpViewIn_IE(Transform objTransform) 
    {
        status = CameraStatus.cameraMoving;
        Vector3 closeViewV3 = new Vector3(objTransform.position.x * 1.5f, objTransform.position.y * 1.5f, objTransform.position.z * 1.5f); //closeView 相機位置
        Vector3 sideViewV3 = new Vector3(objTransform.position.x*2f, objTransform.position.y*2f, objTransform.position.z*2f); //sideView 相機位置
        Vector3 stepVector3 = new Vector3(closeViewV3.x - sideViewV3.x, closeViewV3.y - sideViewV3.y, closeViewV3.z - sideViewV3.z)/40f; //每次移動距離
        int counter = 0;

        while (counter<40)
        {
            gameObject.transform.position += stepVector3; //移動相機

            yield return new WaitForSeconds(0.05f);
            counter += 1;
        }
        status = CameraStatus.closeUpView;
    }

    public void CloseUpToSide(Transform objTransform) 
    {
        StartCoroutine(CloseUpViewOut_IE(objTransform));
    }
    //相機移動 close > side 
    public IEnumerator CloseUpViewOut_IE(Transform objTransform)
    {
        status = CameraStatus.cameraMoving;
        Vector3 closeViewV3 = new Vector3(objTransform.position.x * 1.5f, objTransform.position.y * 1.5f, objTransform.position.z * 1.5f); //closeView 相機位置
        Vector3 sideViewV3 = new Vector3(objTransform.position.x * 2f, objTransform.position.y * 2f, objTransform.position.z * 2f); //sideView 相機位置
        Vector3 stepVector3 = new Vector3( sideViewV3.x - closeViewV3.x ,  sideViewV3.y - closeViewV3.y ,  sideViewV3.z - closeViewV3.z ) / 40f; //每次移動距離
        int counter = 0;

        while (counter < 40)
        {
            gameObject.transform.position += stepVector3; //移動相機

            yield return new WaitForSeconds(0.05f);
            counter += 1;
        }
        status = CameraStatus.sideViewOne; //切換相機模式
    }

    //相機移動 top > side 
    public IEnumerator TopToSide_IE(Transform objTransform)
    {
        yield return new WaitForSeconds(0.01f);
        status = CameraStatus.cameraMoving;

        Vector3 sideViewV3 = new Vector3(objTransform.position.x * 2f, objTransform.position.y * 2f, objTransform.position.z * 2f); //sideView 相機位置 終點

        midStep.Clear();
        //建立中轉站
        for (int i = 0; i < midSite; i++)
        {
            Vector3 temp = (i + 1)/(midSite+1) * (sideViewV3) + (midSite+1 - i - 1) / (midSite+1) * (gameObject.transform.position); //座標
            Debug.Log(" temp " + temp.x + " y " + temp.y + " z " + temp.z + " i = " + i  + " _" + ((i + 1) / (midSite + 1)) + " _" +((midSite + 1 - i - 1) / (midSite + 1))  );

            temp = temp.normalized * 10f;
            midStep.Add(temp);
        }
        //建立向量
        for (int i = 0; i < midSite+1; i++)
        {
            if (i == 0 )
            {
                midVector.Clear();
                Vector3 calculateVector= midStep[i] - gameObject.transform.position;
                midVector.Add(calculateVector);
            }
            else if (i == midSite)
            {
                Vector3 calculateVector = sideViewV3 - midStep[(int)midSite - 1];
                midVector.Add(calculateVector);
            }
            else
            {
                Vector3 calculateVector = midStep[i] - midStep[i - 1];
                midVector.Add(calculateVector);
            }
        }
        int counter = 0;
        int seperateCounter = (int)Mathf.Ceil(120 / midVector.Count);
        
        while (counter < seperateCounter* midVector.Count)
        {
            for (int i = 0; i < midVector.Count; i++)
            {
                midVector[i] = midVector[i] / seperateCounter;
                for (int j = 0; j < seperateCounter ; j++)
                {
                    gameObject.transform.position += midVector[i];
                    counter += 1;
                    gameObject.transform.LookAt(origine);
                    yield return new WaitForSeconds(0.035f);
                }
            }
        }
        
        status = CameraStatus.sideViewOne; // 切換相機模式
        gameObject.transform.position = sideViewV3; // 固定最後位置
        //gameObject.transform.LookAt(origine);
    }

    public IEnumerator SideToTop_IE(Transform objTransform)
    {
        status = CameraStatus.cameraMoving;
        Vector3 sideViewV3 = new Vector3(objTransform.position.x * 2f, objTransform.position.y * 2f, objTransform.position.z * 2f); //sideView 相機位置

        Vector3 stepVector3 = new Vector3(iniPos.x-sideViewV3.x, iniPos.y-sideViewV3.y, iniPos.z-sideViewV3.z) / 40f; //設定移動距離

        int counter = 0;

        while (counter < 40)
        {
            gameObject.transform.LookAt(origine); // 相機設定為看向中心點

            gameObject.transform.position += stepVector3; //移動相機

            yield return new WaitForSeconds(0.05f);
            counter += 1;
        }
        status = CameraStatus.topView; // 切換相機模式
        gameObject.transform.position = iniPos; // 固定最後位置
    }


    //相機移動 (sideView 切換瀏覽項目時 移動相機 )
    public void CameraSlideDamping() 
    {
        StartCoroutine(CameraSlideDamping_IE());
    }

    //相機移動 (sideView 切換瀏覽項目時 移動相機 )
    public IEnumerator CameraSlideDamping_IE()
    {
        //damping 時 不可移動
        status = CameraStatus.cameraMoving;
        //Vector3 startPos = gameObject.transform.position;
        Vector3 goalPos = GameManager.instance.circleObjs[currentObj].transform.position*2f;

        status = CameraStatus.cameraMoving;


        midStep.Clear();
        //建立中轉站
        for (int i = 0; i < midSite; i++)
        {
            Vector3 temp = (i + 1) / (midSite + 1) * (goalPos) + (midSite + 1 - i - 1) / (midSite + 1) * (gameObject.transform.position); //座標
            Debug.Log(" temp " + temp.x + " y " + temp.y + " z " + temp.z + " i = " + i + " _" + ((i + 1) / (midSite + 1)) + " _" + ((midSite + 1 - i - 1) / (midSite + 1)));

            temp = temp.normalized * 10f;
            midStep.Add(temp);
        }
        //建立向量
        for (int i = 0; i < midSite + 1; i++)
        {
            if (i == 0)
            {
                midVector.Clear();
                Vector3 calculateVector = midStep[i] - gameObject.transform.position;
                midVector.Add(calculateVector);
            }
            else if (i == midSite)
            {
                Vector3 calculateVector = goalPos - midStep[(int)midSite - 1];
                midVector.Add(calculateVector);
            }
            else
            {
                Vector3 calculateVector = midStep[i] - midStep[i - 1];
                midVector.Add(calculateVector);
            }
        }
        int counter = 0;
        int seperateCounter = (int)Mathf.Ceil(120 / midVector.Count);

        while (counter < seperateCounter * midVector.Count)
        {
            for (int i = 0; i < midVector.Count; i++)
            {
                midVector[i] = midVector[i] / seperateCounter;
                for (int j = 0; j < seperateCounter; j++)
                {
                    gameObject.transform.position += midVector[i];
                    counter += 1;
                    gameObject.transform.LookAt(origine);
                    yield return new WaitForSeconds(0.035f);
                }
            }
        }

        //固定最後位置
        float angleFix = Mathf.Round((360f / GameManager.instance.circleObjs.Count)) * currentObj;
        gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, angleFix, gameObject.transform.rotation.eulerAngles.z);
        gameObject.transform.LookAt(origine);
        //完成damping 時 可移動
        status = CameraStatus.sideViewOne;
    }

}
