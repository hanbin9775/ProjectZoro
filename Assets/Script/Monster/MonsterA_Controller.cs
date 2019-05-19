using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterA_Controller : MonoBehaviour
{
    //컴포넌트 변수
    Rigidbody rb;
    Animator anim;

    //이동 변수
    public float speed;
    //Patrol 변수
    public float walkRange;
    Vector3 basePosition;
    Vector3 movePosition;

    //idle 변수
    public float waitBaseTime = 2.0f;
    float waitTime;

    //캐릭터 상태 변수
    int lookRight = 1;

    //state 변수
    State state;

    enum State
    {
        Idle,
        Patrol
    };

    private void Awake()
    {
        //초기 설정값
        basePosition = transform.position;
        state = State.Patrol;
        movePosition = basePosition + new Vector3(-walkRange, 0, 0);
        waitTime = waitBaseTime;

        //컴포넌트
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

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
        switch (state)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Idle:
                Wait();
                break;
        }       
    }

    void Patrol()
    {
        if (transform.position.x == movePosition.x) // == 이거 때문에 버그 생길수도?
        {
            Debug.Log("arrived!");
            state = State.Idle;
            anim.SetTrigger("Wait");
            waitTime = waitBaseTime;
        }
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(movePosition.x, transform.position.y, transform.position.z), speed * Time.deltaTime);
    }

    void Wait()
    {
       if(waitTime > 0.0f)
        {
            waitTime -= Time.deltaTime;
            if(waitTime <= 0.0f)
            {
                movePosition = basePosition + new Vector3(walkRange, 0, 0);
                Flip();
                walkRange *= -1;
                state = State.Patrol;
                anim.SetTrigger("Patrol");
            }
        }

    }
}
