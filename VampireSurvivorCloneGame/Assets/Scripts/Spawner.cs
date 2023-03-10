using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    int level; //게임 난이도를 결정한 레벨설정
    float timer;
    void Awake() 
    {
        //하이어라키 Spawner내부에 생성한 포인트들의 위치값을 받아온다
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //gamTime을 특정 초마다 나눠서 레벨이 올라가게 할것이다 
        //ex) 50초가 흘렀고 10초마다 레벨이 올라가면 현재 레벨은 5
        //FloorToInt를 써서 소수점의 나머지를 없앤다
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1);

        //레벨이 0일떈 0.5초마다 소환합니다
        //타이머가 0.2f 초마다 몬스터 소환
        //스폰데이터 속 레벨에따라 스폰시간 결정
        //근데 여기엔 레벨(1~2 값이 들어오는데 여기서 10을 나눠버리면 몬스터가 너무 많이 스폰되잖아)
        if(timer > spawnData[level].spawnTime / 10f)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        //Instantiate 반환 값을 enemy변수에 넣어둡니다 재활용 할것이기 때문입니다
        //아래 코드에서 Random.Range가 1부터 시작하는 이유는12번줄에 겟컴포넌트를 할때 스스로도 포함하기때문에 0이 아니라 2로 시작해야한다
        //레벨에따라 바뀌는 몬스터 1레벨일땐 Enemy1 2레벨일땐 Enemy2... n초마다 등장하는 몬스터가 달라진다
        //스폰데이터에서 관리하기때문에 이제 몬스터타입이 하나만 있어 0을 넣습니다
        GameObject enemy =  GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1,spawnPoint.Length)].position; //몬스터 생성 위치
        
        //겟 컴포넌트에 내가 작성한 스크립트를 불러와서 그 안에있는 함수를 꺼내 쓸 수 있다
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime; //스폰타임
    public int spriteType; //스프라이트타입이 0일땐 해골 1일땐 좀비 이렇게 바뀌게 할것임
    public int health; //몬스터 피통
    public float speed; //몬스터 이속
}