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
    Collider2D coll;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate(); //하나의 FixedUdate를 기다리는것이기때문에 매개변수는 따로 X
    }

    //물리적인 추적을 할거기 때문에 일반 Update() 함수를 쓰지않고 FixedUpdate를 사용할것임
    void FixedUpdate()
    {
        //GetCurrentAnimatorStateInfo(애니메이션레이어).애니메이션이름
        if(!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
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
        coll.enabled = true;
        rigid.simulated = true;;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
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

    void OnTriggerEnter2D(Collider2D collision) 
    {
        //플레이어의 무기에 충돌했을때만 코드가 실행
        //Bullet 태그와 충돌하지 않았으면 코드가 if문을 만나기때문에 코드가 종료됨
        //isLive를 킨 이유는 GameManager를 두번 불러올때 콜라이더가 작동된다면 중복으로 실행될 수 있기 때문이다
        if(!collision.CompareTag("Bullet") || !isLive)
            return;
        
        //자신과 닿은 콜라이더안에 컴포넌트 Bullet을 불러와 그 속에있는 damage변수의 크기만큼 자신의 피를 깎는다
        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            // ... 살아있음
            anim.SetTrigger("Hit");
        }
        else
        {
            // ... 죽음
            // PoolManager에서 재활용해야하기 때문에 다시 true로 변경해야한다
            isLive = false;
            coll.enabled = false; //콜라이더 비활성화해라
            rigid.simulated = false; //리지드바디도 비활성화해라 리지드바디는 simulated로 꺼야한다
            spriter.sortingOrder = 1; //죽고나서 게임오브젝트 레이어를 하나 낮춘다
            anim.SetBool("Dead", true); //죽음상태 애니메이션
            GameManager.instance.kill++;
            GameManager.instance.GetEXP();
        }
    }

    //넉백을 구현해봅시다
    IEnumerator KnockBack()
    {
        yield return wait; // 하나의 물리 프레임을 딜레이해줄것이다
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos; //현재위치 - 플레이어 위치
        //노멀라이즈를 해줘야 순수하게 방향값만 가진 값이 된다
        //리지드바디도 2D기 떄문에 ForceMode도 2D가 된다
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    void Dead()
    {
        //죽은상태는 곧 몬스터 프리팹의 비활성화
        //파괴를하면 안된다 프리팹은 계속 재활용할것이기 때문에
        gameObject.SetActive(false);
    }
}
