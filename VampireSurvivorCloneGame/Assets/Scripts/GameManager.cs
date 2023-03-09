using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //정적변수 static을 미리 설정해두면 즉시 클래스에서 부를 수 있다는 편리함이 있다
    public static GameManager instance;
    [Header("# Game Control")]
    //게임시간과 최대게임시간을 담당할 변수
    public float gameTime; //실제 게임타임
    public float maxGameTime = 2 * 10f; //최대 게임타임 (숫자는 '초')
    [Header("# Player Info")]
    //플레이어의 레벨 시스템을 저장하기 위한 변수
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = {3, 5, 10, 100, 150, 210, 280 , 360, 450, 600};
    [Header("# Game Object")]
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

    public void GetEXP()
    {
        exp++;

        if(exp == nextExp[level]) //해당 레벨업에 필요한 경험치와 현재 경험치가 같다면
        {
            level++; //레벨업
            exp = 0; //경험치는 다시0부터
        }
    }
}
