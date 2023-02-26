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
    //파일 이름, 경도, 위도를 string으로 읽음
    //위도 경도의 경우 double로 읽을 시 데이터 손실 발생
    public string filename;
    public string longitude, latitude;
}

public class ObjInfoList
{
    //obj들의 정보가 들어갈 array
    public Objspec[] objInfo;
}

[RequireComponent(typeof(CesiumForUnity.CesiumGlobeAnchor))]
public class Building : MonoBehaviour
{
    public GameObject prefab;

    void Start()
    {
        //Json 읽어오기
        TextAsset loadedJson = Resources.Load<TextAsset>("test");
        ObjInfoList objInfoList = JsonUtility.FromJson<ObjInfoList>(loadedJson.ToString());

        //각 obj 개체 마다
        foreach (Objspec ob in objInfoList.objInfo)
        {
            //prefab을 obj 번호에 맞게 지정 후 scene에 생성
            prefab = Resources.Load<GameObject>(ob.filename);
            GameObject go = Instantiate(prefab);
            go.transform.SetParent(GameObject.Find("CesiumGeoreference").transform);

            //위도, 경도 좌표 지정을 위한 component 추가
            go.AddComponent<CesiumForUnity.CesiumGlobeAnchor>();

            //String으로 저장된 위도와 경도를 double로 변환하여 위치 지정
            go.GetComponent<CesiumGlobeAnchor>().latitude = Convert.ToDouble(ob.latitude);
            go.GetComponent<CesiumGlobeAnchor>().longitude = Convert.ToDouble(ob.longitude);

            //rotation을 0으로 초기화, 안 할시 건물이 기움
            go.transform.rotation = UnityEngine.Quaternion.Euler(new UnityEngine.Vector3(0, 0, 0));
        }
    }
    void Update()
    {
        
    }
}
