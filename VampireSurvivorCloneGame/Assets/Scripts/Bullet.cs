using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    //Initialize(초기화) 함수
    //Init(데미지,관통,방향)
    public void Init(float damage, int per, Vector3 dir)
    {
        //this는 곧 Bullet.cs
        //매개변수의 damage와 this.damage 는 엄연히 다른 변수
        this.damage = damage;
        this.per = per;

        //근접무기는 관통이 -1 (무한)
        //-1 보다 큰 관통을 가진 Bullet prefab은 관통무기라는것이기 때문에 방향값을 가지게된다
        if (per > -1)
        {
            //rigid의 방향값은 dir값이다
            //그러면 RigidBody2D의 특성에 의해 방향에맞게 rigid가 이동하겠지
            rigid.velocity = dir * 15f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision) 
    {
        //맞닿은 콜라이더가 에너미가 아니거나 관통수치가 -1(근접무기) 일경우 아래 코드는 실행할 이유가 없다
        if (!collision.CompareTag("Enemy") || per == -1)
            return;
        
        //관통력은 줄어든다
        per--;

        if (per == -1)
        {
            //PoolManager에서 추후에 재활용할것이기 때문에 리지드바디를 미리 초기화해주고
            //게임 오브젝트도 비활성화 시켜준다
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
