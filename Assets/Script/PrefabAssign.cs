using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabAssign : MonoBehaviour
{
    public static PrefabAssign instance;
    public GameObject[] prefabObjs; // 可以被建立的物件樣品
    public GameObject[] removeButtonSet; //清除物件按鍵陣列
    public List<GameObject> circleObjs; //被添加的物件
    public int prefabNum; //物件數量
    public int radius = 5; //預設距離長度 form 中心
    public float angle;  //相鄰物件所隔出之角度
    public CameraControl selectCamera; //被選擇的相機
    public GameObject initialButton; //建立場景按鍵
    //public Material[] preSetMaterial; 

    private void Awake()
    {
        instance = this;
    }

    public void PositionSetUp() 
    {
        prefabNum = circleObjs.Count;

        angle = (360f / prefabNum) * (Mathf.PI / 180f); //弧度

        for (int i = 0; i < prefabNum; i++)
        {
            //GameObject temp = null;
            //temp = Instantiate(prefabObjs[i], gameObject.transform); //建立物品
            Vector3 distance = new Vector3(Mathf.Cos(angle*i - 0.5f*Mathf.PI)*radius, 0f, Mathf.Sin(angle*i - 0.5f * Mathf.PI) *radius); //設定座標
            circleObjs[i].transform.position = distance; //物件 設定到預設位置            
            circleObjs[i].SetActive(true); //顯示物件
        }

        AngleCalculate(); // 計算可移動角度
    }

    //-----------------------------------------------
    // 將物件加入清單
    public void AddPrefab( int index) 
    {
        if (circleObjs.Count<12)
        {
            GameObject temp = Instantiate(prefabObjs[index], gameObject.transform); //建立物品
            temp.SetActive(false); //不顯示物件

            circleObjs.Add(temp); //添加至清單
            removeButtonSet[circleObjs.Count - 1].SetActive(true); //移除按鍵設為顯示
            removeButtonSet[circleObjs.Count - 1].transform.GetChild(0).gameObject.GetComponent<Text>().text = "Remove " + circleObjs[circleObjs.Count - 1].name; //移除按鍵 文字賦予
            if (circleObjs.Count>=3) //物件夠多才能建立場景
            {
                initialButton.SetActive(true);
            }
        }
        
    }
    // 將已選物件移出清單
    public void ReMovePrefab(int index)
    {
        removeButtonSet[circleObjs.Count - 1].SetActive(false); //按鍵不顯示
        circleObjs.Remove(circleObjs[index]); //清除對應清單項目
        Destroy(gameObject.transform.GetChild(index).gameObject); //清除對應物件
        RemoveButtonUI(); //清除物件按鍵UI 文字更新

        if (circleObjs.Count < 3) //物件太少時無法建立場景
        {
            initialButton.SetActive(false);
        }
    }
    //清除物件按鍵UI 文字更新
    public void RemoveButtonUI() 
    {
        for (int i = 0; i < circleObjs.Count; i++)
        {
            removeButtonSet[i].transform.GetChild(0).gameObject.GetComponent<Text>().text = "Remove " + circleObjs[i].name;
        }
    }
    //---------------------------------------------
    
    
    // 晃動效果
    public void Damping()
    {
        StartCoroutine(Damping_IE());
    }
    // 晃動效果協程
    public IEnumerator Damping_IE() 
    {
        //damping 時 不可移動
        selectCamera.status = CameraStatus.cameraMoving;
        int counter = 0;        

        //物件左右晃動效果
        while (counter<40)
        {
            float variation = (1-counter / 40f)*angle;
            if (counter%2 ==1)
            {
                variation = -variation;
            }
            counter += 1;
            gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y+variation, gameObject.transform.rotation.eulerAngles.z);
            yield return new WaitForSeconds(0.025f);
        }

        //固定最後位置
        float angleFix = Mathf.Round( (360f/circleObjs.Count)) * selectCamera.currentObj;
        gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, angleFix, gameObject.transform.rotation.eulerAngles.z);
        //完成damping 時 可移動
        selectCamera.status = CameraStatus.sideViewOne;
    }

    public void AngleCalculate() //計算平均弧度
    {
        angle = (360f / circleObjs.Count) * (Mathf.PI / 180f) * 0.8f; //in 弧度
    }
}
