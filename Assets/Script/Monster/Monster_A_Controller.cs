using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_A_Controller : MonoBehaviour
{
    //컴포넌트 변수
    Rigidbody rb;
    Animator anim;

    //이동 변수
    public float speed;

    //Move to position
    Vector3 basePosition;
    Vector3 movePosition;
    public Transform groundDetection;

    //Attack Delay
    public float delayBaseTime = 2.0f;
    float delayTime;

    public float DTI = 1.12f;
    float delayTimer_to_Idle; // idle로 가기 위한 타이머


    //캐릭터 상태 변수
    int lookRight = 1;
    Vector3 theScale;

    //State 변수
    State state;

    //Status
    public float hp = 3.0f;

    //공격
    public GameObject bullet;
    public Transform firepos;
    Transform Target;
    public int firecnt = 2;
    bool c_once = true;

    //Object Pooling
    ObjectPooler objectPooler;

    enum State
    {
        Idle,
        Move,
        Attack
    };

    private void Awake()
    {
        basePosition = transform.position;
        state = State.Move;
        delayTime = delayBaseTime;
        theScale = transform.localScale;

        //컴포넌트
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        
        //Pooling
        objectPooler = ObjectPooler.Instance;

    }

    void Flip()
    {
        lookRight *= -1;
        Vector3 theScale = transform.localScale;
        theScale.z *= -1;
        transform.localScale = theScale;
    }

    // Update is called once per frame
    void Update()
    {
        Target = PlayerController.GetInstance().Player_target;
        if (Target.position.x > gameObject.transform.position.x)
        {
            lookRight = 1;
        }
        else
        {
            lookRight = -1;
        }

        switch (lookRight)
        {
            case 1:
                theScale.z = 1;
                transform.localScale = theScale;
                break;
            case -1:
                theScale.z = -1;
                transform.localScale = theScale;
                break;
        }

        switch (state)
        {
            case State.Idle:
                Idle();
                break;
            case State.Move:
                Move();
                break;
            case State.Attack:
                Attack();
                break;
        }

        //죽음
        if (hp <= 0)
        {
            Monster_Instantiator.GetInstance().ins_cnt -= 1;
            Destroy(gameObject);
        }
    }

    void Move()
    {
        transform.Translate(Vector3.forward * lookRight * speed * Time.deltaTime);
        
        if(!Physics.Raycast(groundDetection.position, Vector3.down, 0.5f))
        {
            Debug.Log("Arrived!");
            state = State.Idle;
            anim.SetTrigger("idle");
        }
    }

    void Idle()
    {
        if (delayTime > 0.0f)
        {
            delayTime -= Time.deltaTime;
            if(delayTime <= 0.0f)
            {
                delayTimer_to_Idle = DTI;
                state = State.Attack;
                anim.SetTrigger("attack");
                c_once = true;
            }
        }
    }

    void Attack()
    {
        // 공격 함수, 탄환 생성 함수
        if (c_once)
        {
            delayTime = delayBaseTime;
            StartCoroutine(AttackTwice());
            c_once = false;
        }
        // 애니메이션 끝나면 state idle로 바뀜, onstatemachine에서
        if (delayTimer_to_Idle > 0.0f)
        {
            delayTimer_to_Idle -= Time.deltaTime;
            if (delayTimer_to_Idle <= 0.0f)
            {
                state = State.Idle;
            }
        }
    }

    IEnumerator AttackTwice()
    {
        Vector3 dir = (Target.position - firepos.position).normalized;
        for (int i = 0; i < firecnt; i++)
        {
            objectPooler.SpawnFromPool("Enemy_Bullet", firepos.position, firepos.rotation).GetComponent<Rigidbody>().AddForce(dir * 250.0f);
            
            yield return new WaitForSeconds(0.7f);
        }
    }

    public void SetIdleState()
    {
        state = State.Idle;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            hp -= 0.5f;
            Debug.Log("Moster_A hp : " + hp);
        }
    }

}
