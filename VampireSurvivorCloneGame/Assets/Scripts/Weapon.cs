using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//poolmanager에서 받아온 bullet을 플레이어 주변을 공전하는 형태로 만들어주는 스크립트
public class Weapon : MonoBehaviour
{
    public int id; //이 무기는 몇번째 ID입니까?
    public int prefabId; //PoolManger의 몇번째 프리팹ID입니까?
    public float damage;
    public int count; //공전하는 근접무기를 몇개나 배치할겁니까?
    public float speed;
    float timer; //일정시간마다 원격공격하기위해\
    Player player; //부모인 Player를 변수화
    
    void Awake()
    {
        //부모 컴포넌트를 가져오는 방법
        player = GetComponentInParent<Player>();
    }
    void Start() 
    {
        Init();
    }

    void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;
                
                //경과한 시간이 공격속도보다 크다면
                //timer를 0초로 바꾸고 총알을 발사한다
                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        if (Input.GetButtonDown("Jump"))
            levelUp(2,1);
    }

    //초기화방식이 ID에 따라 다르다
    public void Init()
    {
        //id가 n번일때
        switch (id)
        {
            case 0:
            speed = -200; //근접무기 공전속도
            Batch();

                break;
            default:
                speed = 0.3f; //원거리무기 발사속도
                break;
        }
    }
    //레벨업하면 damage와 count(개수)가 늘어나도록
    public void levelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
            Batch();
    }

    //공전하는 삽을 플레이어 주변을 공전하는 함수
    void Batch()
    {
        for (int index = 0; index <count; index++)
        {
            Transform bullet;
            
            //기존 오브젝트를 먼저 활용하고 모자란것은 풀링에서 가져오는 시스템
            if (index < transform.childCount)
            {
                //현재 내가 가지고있는 인덱스를 가져와서 쓸것이다
                bullet = transform.GetChild(index);
            }
            else
            {
                //게임매니저에 등록해둔 prefab을 가져오는과정
                //Get(1) 을 등록해도 무기를 가져오겠지만 그러면 하드코딩이라 변수를 따로 설정한거임
                bullet = GameManager.instance.pool.Get(prefabId).transform;
            }
            //parent 속성을 통해 부모를 변경 가능하다
            bullet.parent = transform;

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity; //Quaternion의 zero값은 identity
            
            //만약 count가 10개라면 360도를 10으로 나눈값이 되겠지
            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            //Translate 함수로 자신의 위쪽으로 이동합니다
            //Bullet을 회전시킬때 Local방향을 이용한건 bullet.up때 이미 사용함
            //그러니 bullet을 이동시킬때는 World방향 기준으로 이동시킵니다
            bullet.Translate(bullet.up * 1.5f, Space.World);

            //Bullet.cs.의 데미지, 관통횟수인데 -1로두면 무한으로 관통시키겠다
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
        }
    }

    //총알을 발사하는 로직이며 그냥 풀매니저에서 총알을 가져올것이다
    void Fire()
    {
        //스캐너를 불러와 플레이어 가까이 있는 적을 타게팅한다
        //플레이어스크립트 내부 스캐너 내부에 가장가까운 타겟이 없다면(false) 그냥 return
        if (!player.scanner.nearestTarget)
            return;
        
        //이건 단순한 근처 적의 '위치값'
        Vector3 targetPos = player.scanner.nearestTarget.position;
        //위치값
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        //poolManger에서 프리팹아이디를 가져와서 bullet변수에 담는다
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position; //bullet의 시작 위치는 현재 플레이어의 시작위치
        //FromToRotation : 지정된 축을 중심으로 목표를 향해 회전하는 함수
        //(Vector3.up 이니까 z 축으로 dir방향값으로 돌린다)
        bullet.rotation = Quaternion.FromToRotation(Vector3.up,dir);
        //Bullet.cs.의 데미지, 관통횟수, 방향값
        bullet.GetComponent<Bullet>().Init(damage, 1, dir);

    }
}
