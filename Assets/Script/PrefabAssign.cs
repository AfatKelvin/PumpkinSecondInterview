using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabAssign : MonoBehaviour
{
    public static PrefabAssign instance;
    public GameObject[] prefabObjs; // �i�H�Q�إߪ�����˫~
    public GameObject[] removeButtonSet; //�M���������}�C
    public List<GameObject> circleObjs; //�Q�K�[������
    public int prefabNum; //����ƶq
    public int radius = 5; //�w�]�Z������ form ����
    public float angle;  //�۾F����ҹj�X������
    public CameraControl selectCamera; //�Q��ܪ��۾�
    public GameObject initialButton; //�إ߳�������
    //public Material[] preSetMaterial; 

    private void Awake()
    {
        instance = this;
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

    //-----------------------------------------------
    // �N����[�J�M��
    public void AddPrefab( int index) 
    {
        if (circleObjs.Count<12)
        {
            GameObject temp = Instantiate(prefabObjs[index], gameObject.transform); //�إߪ��~
            temp.SetActive(false); //����ܪ���

            circleObjs.Add(temp); //�K�[�ܲM��
            removeButtonSet[circleObjs.Count - 1].SetActive(true); //��������]�����
            removeButtonSet[circleObjs.Count - 1].transform.GetChild(0).gameObject.GetComponent<Text>().text = "Remove " + circleObjs[circleObjs.Count - 1].name; //�������� ��r�ᤩ
            if (circleObjs.Count>=3) //������h�~��إ߳���
            {
                initialButton.SetActive(true);
            }
        }
        
    }
    // �N�w�磌�󲾥X�M��
    public void ReMovePrefab(int index)
    {
        removeButtonSet[circleObjs.Count - 1].SetActive(false); //���䤣���
        circleObjs.Remove(circleObjs[index]); //�M�������M�涵��
        Destroy(gameObject.transform.GetChild(index).gameObject); //�M����������
        RemoveButtonUI(); //�M���������UI ��r��s

        if (circleObjs.Count < 3) //����Ӥ֮ɵL�k�إ߳���
        {
            initialButton.SetActive(false);
        }
    }
    //�M���������UI ��r��s
    public void RemoveButtonUI() 
    {
        for (int i = 0; i < circleObjs.Count; i++)
        {
            removeButtonSet[i].transform.GetChild(0).gameObject.GetComponent<Text>().text = "Remove " + circleObjs[i].name;
        }
    }
    //---------------------------------------------
    
    
    // �̰ʮĪG
    public void Damping()
    {
        StartCoroutine(Damping_IE());
    }
    // �̰ʮĪG��{
    public IEnumerator Damping_IE() 
    {
        //damping �� ���i����
        selectCamera.status = CameraStatus.cameraMoving;
        int counter = 0;        

        //���󥪥k�̰ʮĪG
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

        //�T�w�̫��m
        float angleFix = Mathf.Round( (360f/circleObjs.Count)) * selectCamera.currentObj;
        gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, angleFix, gameObject.transform.rotation.eulerAngles.z);
        //����damping �� �i����
        selectCamera.status = CameraStatus.sideViewOne;
    }

    public void AngleCalculate() //�p�⥭������
    {
        angle = (360f / circleObjs.Count) * (Mathf.PI / 180f) * 0.8f; //in ����
    }
}
