using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repositon : MonoBehaviour
{
    //트리거가 체크된 콜라이더를 벗어났을때
    //이를 위해 플레이어에 Area를 만들었다 (충돌여부를 확인하기 위해)
    void OnTriggerExit2D(Collider2D collision)
    {
        //이 콜라이더의 주인
        //Area 태그가 '아닐때' 코드가 실행되도록함
        //만약 Area태그가 아닌다면 코드를 실행하지 않겠다 라는 뜻
        if(!collision.CompareTag("Area"))
            return; //코드 상단에 if return 필터를 넣음으로서 이 태그가 아닐경우 빨랑 코드가 나갈수있도록함

        //거리를 구하기 위해 플레이어 위치와 타일맵 위치를 미리 저장
        
        //플레이어 위치
        //플레이어 포지션을 가져오기위해 게임매니저에 저장해둔 플레이어에서 위치값을 가져오는 과정
        Vector3 playerPos = GameManager.instance.player.transform.position;
        
        //타일맵 위치
        //현재 이 스크립트가 들어있는 타일맵의 위치값
        Vector3 myPos = transform.position; 
        
        //x축과 y축 각각의 거리를 구하는 코드
        //Mathf 는 수학 라이브러리.. 절대값을 구하기 위해 Abs를 불러옴
        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);


        //플레이어의 이동 방향을 저장하기 위한 변수
        //플레이어에서 미리 만들어뒀던 inputVec을 불러온다
        Vector3 playerDir = GameManager.instance.player.inputVec;
        //Player에서 노멀라이즈를 했기때문에 1값이 안나오니 0보다 클때(좌우 이동할때) 값을 받아온다 
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        //나중에 적이 추가되었을때 적들도 재배치를 요구하기때문에 여기서 필터를 만들어준다
        switch (transform.tag)
        {
            case "Ground":
                if (diffX > diffY)
                {
                    //Translate : 지정된 값 만큼 현재 위치에서 이동한다
                    //Translate(이동방향,플레이어방향,Area의크기,)
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Enemy":
                break;
        }
    }
}
