using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed; //몬스터 이속
    public Rigidbody2D target; //물리적으로 따라갈거기때문에 리지드바디를 타입으로 둠
    public bool isLive; //살았는지 죽었는지 확인함
    Rigidbody2D rigid; //내위치(몬스터위치)
    SpriteRenderer spriter;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

    //물리적인 추적을 할거기 때문에 일반 Update() 함수를 쓰지않고 FixedUpdate를 사용할것임
    void FixedUpdate()
    {
        if(!isLive)
            return; //플레이어가 죽어있으면 바로 코드를 종료시켜버리는 필터코드

        //타겟의 위치에서 나의 위치를 뺸 값
        Vector2 dirVec = target.position - rigid.position;
        //픽스드업데이트 내부에서 쓰는거니 델타타임도 fixed 붙여줌
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        //현재위치(rigid.postion)에 다음에 나아갈 방향(nextVec)을 더해준다
        rigid.MovePosition(rigid.position + nextVec);
        //리지드바디끼리 충돌했을때 누구 하나가 밀려나지 않도록 Velocity를 고정
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate() 
    {
        spriter.flipX = target.position.x < rigid.position.x;
    }
}
