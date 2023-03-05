using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //정적변수 static을 미리 설정해두면 즉시 클래스에서 부를 수 있다는 편리함이 있다
    public static GameManager instance;

    //게임시간과 최대게임시간을 담당할 변수
    public float gameTime; //실제 게임타임
    public float maxGameTime = 2 * 10f; //최대 게임타임 (숫자는 '초')


    public PoolManager pool;
    public Player player;

    void Awake() 
    {
        instance = this;
    }

    void Update()
    {
        gameTime += Time.deltaTime;

        //현재 플레이타임이 최대 플레이타임을 초과한다면
            //현재 게임시간을 최대 게임시간으로 고정시킵니다
        if(gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }
}
