using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed; //몬스터 이속
    public float health; //몬스터의 현체력
    public float maxHealth;
    public RuntimeAnimatorController[] animCon; //애니메이션 컨트롤러를 각 몬스터 타입에 맞게 넣기위해 배열로 선언
    public Rigidbody2D target; //물리적으로 따라갈거기때문에 리지드바디를 타입으로 둠
    public Animator anim;
    public bool isLive; //살았는지 죽었는지 확인함
    Rigidbody2D rigid; //내위치(몬스터위치)
    SpriteRenderer spriter;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
        if(!isLive)
            return; //플레이어가 죽어있으면 바로 코드를 종료시켜버리는 필터코드

        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable()
    {
        //게임매니저에 이미 플레이어가 저장되어있기떄문에 타겟을 저장할 수 있음
        //target의 Type은 Rigidbody2D라 플레이어 내부에 Rigidbody2D를 불러와야한다
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth; //objectPooling이 일어났을때 피를 원래대로 되돌리기 위해서 maxHealth를 넣어줌
    }

    //스폰데이터를 그대로 때려박음
    //변수 data에 SpawnData 스크립트를 넣음으로서 SpawnData에 접근할 수 있음
    public void Init(SpawnData data)
    {
        // ... 스프라이트 & 애니메이션 변경부분
        //스프라이트의 타입은 인덱스로만 쓴다 (어처피 배열이 애니메이터랑 동일하니까)
        //애니메이터의 컨트롤을 뭐로바꿀래? animCon[스프라이트배열] 로 바꿀게여!
        anim.runtimeAnimatorController = animCon[data.spriteType];

        // ... 스피드부분
        speed = data.speed;

        // ... 피통부분
        maxHealth = data.health;
        health = data.health; //최대피통이 올라갔기때문에 현피통도 초기화해준다
    }
}
