using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리팹을 포관할 변수와 풀 리스트가 필요하다 (무언가를 담을것이기 때문에 수영장'풀' 이라고 함...)
    // 프리팹이 2개면 리스트도 2개여야함

    // ... 프리팹 보관할 변수
    public GameObject[] prefabs;

    // ... 풀 담당을 하는 리스트들
    //리스트는 꺽세를 만들고 그 안에 타입을 넣어준다
    //리스트도 프리팹의 개수만큼 생성되어야하니 배열로 선언해준다
    List<GameObject>[] pools;
    
    void Awake() 
    {
        //리스트기때문에 new를 만들어줘야함
        //List 배열의 크기는 prefabs배열과 동일하기때문에 배열에 Prefabs의 길이를 넣어준다
        pools = new List<GameObject>[prefabs.Length];

        //for문으로 배열 내부 오브젝트들을 모두 초기화해준다
        for (int index = 0; index < pools.Length; index++)
        {
            //풀을 담는 배열도 초기화해주고 각각의 리스트들도 전부 초기화해줌
            pools[index] = new List<GameObject>();
        }
    }
    //생성된 오브젝트를 반환해줄거임
    //어떤 Pools[]에 담겨있는 오브젝트를 가져올것이기때문에 매개변수도 넣어준다
    public GameObject Get(int index)
    {
        GameObject select = null; //null 로 초기화 (foreach문에서 오브젝트를 넣어줄거임)
        
        foreach (GameObject item in pools[index])
        {
            
            //item변수가 활성화되어있는지 스스로 확인하는 변수
            if(!item.activeSelf)
            {
                // ... 선택한 Pool[]의 놀고 있는 게임오브젝트에 접근
                    // ... 노는 오브젝트를 발견했을경우 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }
        
        // ... 못 찾았으면?(모든 오브젝트가 작동중이라면)
        if (!select) //값이 없으면 false가 반환되기 때문에
        {
            // ... 새롭게 생성하여 Select 변수에 할당
            //Instantiate(복사될게임오브젝트,위치값)
            select = Instantiate(prefabs[index], transform); //오브젝트 생성하고
            pools[index].Add(select); //복사된 오브젝트를 pools에 등록해준다
        }

        return select;
    }
}
