using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    float timer;
    void Awake() 
    {
        //하이어라키 Spawner내부에 생성한 포인트들의 위치값을 받아온다
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //타이머가 0.2f 초마다 몬스터 소환
        if(timer > 0.2f)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        //n번은 Enemy(n)
        //Random 0,2 를 해줘야 0~1 둘 중 하나를 선택함
        //Instantiate 반환 값을 enemy변수에 넣어둡니다 재활용 할것이기 때문입니다
        //아래 코드에서 Random.Range가 1부터 시작하는 이유는12번줄에 겟컴포넌트를 할때 스스로도 포함하기때문에 0이 아니라 2로 시작해야한다
        GameObject enemy =  GameManager.instance.pool.Get(Random.Range(0,2));
        enemy.transform.position = spawnPoint[Random.Range(1,spawnPoint.Length)].position;
    }
}
