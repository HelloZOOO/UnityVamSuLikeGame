using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
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
        level = Mathf.FloorToInt(GameManager.instance.gameTime / 10f);

        //타이머가 0.2f 초마다 몬스터 소환
        if(timer > (level == 0 ? 0.5f : 0.2f))
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
        GameObject enemy =  GameManager.instance.pool.Get(level);
        enemy.transform.position = spawnPoint[Random.Range(1,spawnPoint.Length)].position; //몬스터 생성 위치
    }
}
