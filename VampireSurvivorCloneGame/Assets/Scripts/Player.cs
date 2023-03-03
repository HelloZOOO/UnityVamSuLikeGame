using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; //새롭게 설치한 Player Input System 을 사용하기 위해 임포트

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }


    //FixedUdate는 물리 연산 프레임마다 호출되는 함수다
    void FixedUpdate() 
    {

        //어느 방향이든 벡터값을 1로 고정
        Vector2 nextVec = inputVec * speed * Time.deltaTime;

        //위치 이동
        //MovePostion은 위치 이동이라 현재 위치를 더해줘야함
        //이 코드에서 현재 위치는 rigid.postion 이다
        //인풋값과 현재위치를 더해주면 플레이어가 나아가야 할 방향을 계산한다
        rigid.MovePosition(rigid.position + nextVec);
    }

    void OnMove(InputValue value)
    {
        //아까 Player의 Input Action 속성의Move - Control Type으로 설정했던 Vector2 를 가져옴
        //해당 라이브러리를 통해서 벡터의 노멀라이즈는 이미 설정되어있기 때문에 기존 nextVec에서 normalized는 빼도 된다
        inputVec = value.Get<Vector2>();
    }

    void LateUpdate()
    {
        //magnitude : 순수하게 크기만 가지고있는 속성
        //움직이기만 하면 애니메이션이 작동되는거니까 Speed에 단순한 움직임값만 넣어주는것
        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0)
        {
            //좌측으로 이동하면 x는 - 값이 되니까 spriter.flipX는 true값이 된다
            //우측으로 이동하면 좌표상 x는 +가 되니까 true값을 가지며 기존대로 우측을 바라본다
            spriter.flipX = inputVec.x < 0;
        }
    }
}
