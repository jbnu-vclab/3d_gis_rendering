using CesiumForUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.SearchService;
using UnityEngine;

[System.Serializable]
public class Objspec
{
    //���� �̸�, �浵, ������ string���� ����
    //���� �浵�� ��� double�� ���� �� ������ �ս� �߻�
    public string filename;
    public string longitude, latitude;
}

public class ObjInfoList
{
    //obj���� ������ �� array
    public Objspec[] objInfo;
}

[RequireComponent(typeof(CesiumForUnity.CesiumGlobeAnchor))]
public class Building : MonoBehaviour
{
    public GameObject prefab;

    void Start()
    {
        //Json �о����
        TextAsset loadedJson = Resources.Load<TextAsset>("test");
        ObjInfoList objInfoList = JsonUtility.FromJson<ObjInfoList>(loadedJson.ToString());

        //�� obj ��ü ����
        foreach (Objspec ob in objInfoList.objInfo)
        {
            //prefab�� obj ��ȣ�� �°� ���� �� scene�� ����
            prefab = Resources.Load<GameObject>(ob.filename);
            GameObject go = Instantiate(prefab);
            go.transform.SetParent(GameObject.Find("CesiumGeoreference").transform);

            //����, �浵 ��ǥ ������ ���� component �߰�
            go.AddComponent<CesiumForUnity.CesiumGlobeAnchor>();

            //String���� ����� ������ �浵�� double�� ��ȯ�Ͽ� ��ġ ����
            go.GetComponent<CesiumGlobeAnchor>().latitude = Convert.ToDouble(ob.latitude);
            go.GetComponent<CesiumGlobeAnchor>().longitude = Convert.ToDouble(ob.longitude);

            //rotation�� 0���� �ʱ�ȭ, �� �ҽ� �ǹ��� ���
            go.transform.rotation = UnityEngine.Quaternion.Euler(new UnityEngine.Vector3(0, 0, 0));
        }
    }
    void Update()
    {
        
    }
}
