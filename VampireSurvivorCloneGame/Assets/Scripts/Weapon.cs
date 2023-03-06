using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//poolmanager에서 받아온 bullet을 플레이어 주변을 공전하는 형태로 만들어주는 스크립트
public class Weapon : MonoBehaviour
{
    public int id; //이 무기는 몇번째 ID입니까?
    public int prefabId; //PoolManger의 몇번째 프리팹ID입니까?
    public float damage;
    public int count; //공전하는 근접무기를 몇개나 배치할겁니까?
    public float speed;

    void Update()
    {

    }

    //초기화방식이 ID에 따라 다르다
    public void Init()
    {
        switch (id)
        {
            case 0:
            speed = -150;
            Batch();

                break;
            default:
                break;
        }
    }

    //공전하는 삽을 플레이어 주변을 공전하는 함수
    void Batch()
    {
        for (int index = 0; index <count; index++)
        {
            //게임매니저에 등록해둔 prefab을 가져오는과정
            //Get(1) 을 등록해도 무기를 가져오겠지만 그러면 하드코딩이라 변수를 따로 설정한거임
            Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        }
    }
}
