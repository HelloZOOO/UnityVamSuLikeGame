using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer; //레이어 마스크를 생성
    public RaycastHit2D[] targets; //몬스터'들' 과 플레이어간의 거리를 계산하기 위해
    public Transform nearestTarget; //플레이어와 가장 가까운 몬스터

    void FixedUpdate() 
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest(); //가장 가까운 적을 찾기 위한 함수를 매 프레임 실행
    }

    //플레이어의 위치에서 가장 가까운 적을 구하는 함수
    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;
        
        // ... 반복문을 돌며 가져온 거리가 저장된 거리보다 작으면 고체
        //targets안에 CircleCastAll에 맞은에들 중에서
        //순차적으로 targets를 돌면서 Raycast를 하나하나 꺼내는 흐름
        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position; //내 위치
            Vector3 targetPos = target.transform.position; //레이케스트를 맞은 적의 위치
            //거리를 구해주자
            //Distance가 벡터2개의 거리를 알아서 구해준다
            float curDiff = Vector3.Distance(myPos, targetPos);

            // .. 현재 거리와 가져온 거리를 비교
            //지금 하나하나 가져온 target과 지금 우리가 가지고있는 최소한의 거리
            //가지고 온 거리가 더 작다면 diff에 그 거리를 넣어준다
            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }
        return result;
    }
}
