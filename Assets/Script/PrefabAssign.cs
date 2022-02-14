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
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Damping();

        }
    }


    public void PositionSetUp() 
    {
        prefabNum = circleObjs.Count;

        float angle = (360f / prefabNum) * (Mathf.PI / 180f);

        for (int i = 0; i < prefabNum; i++)
        {
            //GameObject temp = null;
            //temp = Instantiate(prefabObjs[i], gameObject.transform); //�إߪ��~
            Vector3 distance = new Vector3(Mathf.Cos(angle*i)*radius, 0f, Mathf.Sin(angle*i)*radius); //�]�w�y��
            circleObjs[i].transform.position = distance; //���� �]�w��w�]��m            
            circleObjs[i].SetActive(true); //��ܪ���
        }
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
        float angle = (360f / circleObjs.Count) *(Mathf.PI / 180f) * 0.8f; //in ����

        int counter = 0;
        float iniAngle = gameObject.transform.rotation.eulerAngles.y;
        Debug.Log("angle = " + iniAngle);

        while (counter<40)
        {
            float variation = (1-counter / 40f)*angle + iniAngle;
            if (counter%2 ==1)
            {
                variation = -variation;
            }
            counter += 1;
            gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y+variation, gameObject.transform.rotation.eulerAngles.z);
            yield return new WaitForSeconds(0.1f);
            Debug.Log("counter = " + counter + " angle = " + angle + " vari = " + variation);
        }

    }
}
