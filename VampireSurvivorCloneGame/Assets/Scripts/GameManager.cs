using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //정적변수 static을 미리 설정해두면 즉시 클래스에서 부를 수 있다는 편리함이 있다
    public static GameManager instance;
    public Player player;

    void Awake() 
    {
        instance = this;
        /*
        instance = GetComponent<GameManager>();
        player = GetComponent<Player>();
        */
    }
}
