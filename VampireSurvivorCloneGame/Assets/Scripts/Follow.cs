using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    RectTransform rect;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    //플레이어의 프레임 주기를 동기화하기위해 FixedUpdate로 사용
    void FixedUpdate() 
    {
        //월드 상의 오브젝트 위치를 스크린 좌표로 변환
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
    }
}
