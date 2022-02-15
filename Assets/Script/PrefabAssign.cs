using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabAssign : MonoBehaviour
{
    public static PrefabAssign instance;
    public GameObject[] prefabObjs;
    public GameObject[] removeButtonSet;
    public List<GameObject> circleObjs;
    public int prefabNum = 5;
    public int radius = 5;
    public float deltaX;
    public float angle;
    public CameraControl selectCamera;
    //public Material[] preSetMaterial; 

    private void Awake()
    {
        instance = this;
        //InitialSetUp();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void PositionSetUp() 
    {
        prefabNum = circleObjs.Count;

        angle = (360f / prefabNum) * (Mathf.PI / 180f); //����

        for (int i = 0; i < prefabNum; i++)
        {
            //GameObject temp = null;
            //temp = Instantiate(prefabObjs[i], gameObject.transform); //�إߪ��~
            Vector3 distance = new Vector3(Mathf.Cos(angle*i - 0.5f*Mathf.PI)*radius, 0f, Mathf.Sin(angle*i - 0.5f * Mathf.PI) *radius); //�]�w�y��
            circleObjs[i].transform.position = distance; //���� �]�w��w�]��m            
            circleObjs[i].SetActive(true); //��ܪ���
        }

        AngleCalculate(); // �p��i���ʨ���
    }

    public void AddPrefab( int index) 
    {
        GameObject temp = Instantiate(prefabObjs[index], gameObject.transform); //�إߪ��~
        temp.SetActive(false);
        
        circleObjs.Add(temp); //�K�[�ܲM��
        removeButtonSet[circleObjs.Count - 1].SetActive(true);
    }

    public void ReMovePrefab(int index)
    {
        removeButtonSet[circleObjs.Count - 1].SetActive(false);
        circleObjs.Remove(circleObjs[index]);
    }


    public void Rotate() 
    {

    }

    public void Damping()
    {
        StartCoroutine(Damping_IE());
    }

    public IEnumerator Damping_IE() 
    {
     //   angle = (360f / circleObjs.Count) *(Mathf.PI / 180f) * 0.8f; //in ����

        int counter = 0;
        //float iniAngle = gameObject.transform.rotation.eulerAngles.y;
        

        while (counter<40)
        {
            float variation = (1-counter / 40f)*angle;
            if (counter%2 ==1)
            {
                variation = -variation;
            }
            counter += 1;
            gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y+variation, gameObject.transform.rotation.eulerAngles.z);
            yield return new WaitForSeconds(0.1f);
            //Debug.Log("counter = " + counter + " angle = " + angle + " vari = " + variation);
        }

        //�T�w�̫��m
        float anleFix = Mathf.Round(gameObject.transform.rotation.eulerAngles.y / 90f)*90f;
        gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, anleFix, gameObject.transform.rotation.eulerAngles.z);
    }

    public void AngleCalculate()
    {
        angle = (360f / circleObjs.Count) * (Mathf.PI / 180f) * 0.8f; //in ����
    }
}
