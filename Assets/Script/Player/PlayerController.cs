using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //SIngleton
    private static PlayerController instance;

    public static PlayerController GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<PlayerController>();
        }
        return instance;
    }

    //Character Components
    public Transform myTrans;
    public Transform Player_target; // 적들이  플레이어 정중앙 조준할 수 있게 하는 변수
    private Rigidbody rb;
    public Animator anim;

    //Character Status
    public int lookRight = 1;
    //dash
    public bool isdashing = false;
    private float dashTimer;
    public float dashCd = 3f;
    public bool once = true;
    public int dashcnt = 1;
    private float dashpower;
    public float dasPw;
    public float minusPw;
    //jump
    private bool jump = false;
    public bool grounded = false;
    public int jumpcount;
    public float jumpForce = 4;
    //attack
    public bool isattacking = false;
    public int attackcnt = 0;
    float lastAttackTime = 0;
    float maxComboDelay = 1f;

    //Character Info
    public float speed = 3f;

    //Character State : FSM 으로 refactoring 필요
    public CharacterState c_state;

    public enum CharacterState
    {
        Idle,
        Attack,
        Jump,
        Gothit,
        Die
    };



    private void Awake()
    {
        //Singleton Check
        if (instance != null)
        {             
            if (instance != this)
            {      
                Destroy(gameObject);   
            }
        }

        jumpcount = 2;
        myTrans = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        c_state = CharacterState.Idle;
    }



    void Attack()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            anim.SetBool("attackup", true);
        }
        else if (Input.GetKey(KeyCode.DownArrow) && !grounded)
        {
            anim.SetBool("attackdown", true);
        }
        else
            anim.SetTrigger("attack");
    }

    void Jump()
    {
        rb.velocity = Vector3.up * jumpForce;
        jump = false;
        if (jumpcount == 1)
        {
            anim.SetBool("jump", true);
        }
        else if (jumpcount == 0)
        {
            anim.SetTrigger("doublejump");
        }
    }

    void Update()
    {
        // 대쉬 키 : C
        if (Input.GetKeyDown(KeyCode.C) && !isdashing&&grounded&&!isattacking)
        {
            isdashing = true;
            dashcnt=1;
            dashTimer = dashCd;
            dashpower = dasPw;
        }
        // 대쉬 쿨타임 타이머
        if (isdashing)
        {
            if (once)
            {
                once = false;
            }
            if (dashTimer > 0)
            {
                dashTimer -= Time.deltaTime;
            }
            else
            {
                isdashing = false;
                once = true;
            }
        }

        // 점프 키 : Z
        if (Input.GetKeyDown(KeyCode.Z) && jumpcount > 0 && !isdashing)
        {
            jump = true;
            jumpcount--;
            grounded = false;

        }

        // 공격 키 : X
        if (Input.GetKeyDown(KeyCode.X))
        {
            CameraShake.instance.shakefunc();
            if (Input.GetKey(KeyCode.UpArrow))
            {
                anim.SetBool("attackup", true);
            }
            else if (Input.GetKey(KeyCode.DownArrow) && !grounded)
            {
                anim.SetBool("attackdown", true);
            }
            else
                anim.SetTrigger("attack");
        }
    }


    private void FixedUpdate()
    {
        HandleDash();
        
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Roll"))
        {
            Move(Input.GetAxisRaw("Horizontal"));
            if (Input.GetAxis("Horizontal") > 0 && lookRight == -1)
            {
                Flip();
                lookRight = 1;
            }
            else if (Input.GetAxis("Horizontal") < 0 && lookRight == 1)
            {
                Flip();
                lookRight = -1;
            }
        }

        
        if (jump)
        {
           rb.velocity = Vector3.up * jumpForce;
            jump = false;
            if (jumpcount == 1)
            {
                anim.SetBool("jump", true);
            }
            else if (jumpcount == 0)
            {
               anim.SetTrigger("doublejump");
            }
        }
        CheckSpeed();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        { 
            anim.SetBool("jump", false);
            jumpcount = 2;
            grounded = true;
        }
    }

    //속도 값 구해서 animator한테 넘겨줌
    public void CheckSpeed()
    {
        anim.SetFloat("speed", Mathf.Abs(Input.GetAxisRaw("Horizontal")) * speed);
    }

    void Flip()
    {
        lookRight *= -1;
        Vector3 theScale = transform.localScale;
        theScale.z *= -1;
        transform.localScale = theScale;
    }

    public void Move(float horizontalInput)
    {
        if (horizontalInput > 0)
        {
            myTrans.position = new Vector2(myTrans.position.x + horizontalInput * Time.deltaTime * speed, myTrans.position.y);
        }
        else if (horizontalInput < 0)
        {
            myTrans.position = new Vector2(myTrans.position.x + horizontalInput * Time.deltaTime * speed, myTrans.position.y);
        }
    }

    private void HandleDash()
    {
        if (isdashing)
        {
            if (dashcnt == 1)
            {
                anim.SetTrigger("roll");
                
            }
            dashcnt = 0;
            if (lookRight == 1)
            {
                transform.position += new Vector3(1, 0).normalized * dashpower *Time.deltaTime;
            }
            else
            {
                transform.position += new Vector3(-1, 0).normalized * dashpower *Time.deltaTime;    
            }
            dashpower -= dashpower * minusPw * Time.deltaTime;
        }
    }

    IEnumerator DashLoop()
    {

        if (lookRight == 1)
        {
            for (int i = 0; i < 12; i++)
            {
                transform.position += new Vector3(1, 0).normalized * dashpower;
                //dashpower -= 0.05f;
                yield return new WaitForSeconds(0.05f);
            }
        }
        else
        {
            for (int i = 0; i < 12; i++)
            {
                transform.position += new Vector3(-1, 0).normalized * dashpower;
                //dashpower -= 0.05f;
                yield return new WaitForSeconds(0.05f);

            }
        }
    }

}
