using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    //Initialize(초기화) 함수
    public void Init(float damage, int per)
    {
        //this는 곧 Bullet.cs
        //매개변수의 damage와 this.damage 는 엄연히 다른 변수
        this.damage = damage;
        this.per = per;
    }
}
