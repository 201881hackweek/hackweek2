using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //
    public GameObject end;
    public GameObject gun;
    public static Player instance;
    //状态值
    public float life;
    public int damage;
    public int san;


    //状态
    public bool isJumping;
    public bool isMovingX;
    public bool isStaticX;
    public bool isLanding;
    public bool isRunning;
    public bool isCorching;
    public bool isClimbing;

    public bool isOpending;
    public bool isClosing;

    public bool isShoting;
    public bool isShot;

    public bool isDead;
    public bool isDamaged;

    public bool isReal;


    public GameObject li;
    public GameObject an;

    //限制 (以下数值public方便观察，完成后改private
    public bool canJump;
    public bool canMove;
    public bool canRun;
    public bool canCorch;
    public bool canClimb;
    public bool canShot;
    public bool canBeDamaged;
    public bool hasGun;

    //值
    public float jumpVal;
    public float jumpSpeed;
    public float jumpTime;
    public float walkSpeed;
    public float runSpeed;
    public float moveVal;
    public float moveSpeed;
    public float verVal;
    public float scaleX;
    public float climbSpeed;
    public float climbVal;
    public float runTime;
    public float nowRunTime;
    public float runCD;
    public float nowRunCD;

    public float jumpTimer;
    public int audioIndex;
    public float damagedTimer;
    public float nowDamagedTimer;

    //组件
    public Rigidbody2D rigidbody2;
    public Animator animator;
    public CapsuleCollider2D collider2;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    // Use this for initialization
    void Start () {

        rigidbody2 = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        collider2 = gameObject.GetComponent<CapsuleCollider2D>();

        //值初始化
        life = 7;
        damage = 1;
        san = 10;
        jumpTimer = 0;

        //状态初始化
        isJumping = false;
        isMovingX = false;
        isRunning = false;
        isRunning = false;
        isCorching = false;
        isStaticX = true;
        isLanding = true;
        isClimbing = false;
        isShoting = false;
        isShot = false;

        isReal = true;
        //数据初始化
        canJump = true;
        canMove = true;
        canRun = true;
        canCorch = true;
        canClimb = false;
        canShot = true;
        hasGun = true;
        canBeDamaged = true;

        jumpSpeed = 18f;
        jumpTime = 1f;
        jumpTimer = 0;
        moveSpeed = 3f;
        walkSpeed = 3f;
        runSpeed = 6f;
        climbSpeed = 4f;
        runTime = 6f;
        runCD = 3f;
        nowRunCD = runCD;
        nowRunTime = runTime;

        damagedTimer = 0.7f;
        scaleX = transform.localScale.x;

        SoundManager.instance.SetMusic(1);
        SoundManager.instance.PlayMusic();
	}
	
    IEnumerator Dead()
    {
        ShowAnimation();
        float timer;
        timer = 2f;
        while(timer>0)
        {
            timer -= Time.deltaTime;
            return null;
        }
        //gameover;
        Debug.Log("7");
        return null;
    }
	// Update is called once per frame

    void RealDeath()
    {
        end.SetActive(true);
        //Time.timeScale = 0;
    }
	void Update ()
    {


        if (life <= 0)
        {
            if (isDead)
                return;
            isDead = true;
            StartCoroutine("Dead");
            Invoke("RealDeath", 2f);

            return;
        }


        if (Input.GetKeyDown(KeyCode.F))
            isReal = !isReal;

        if (isReal)
        {
            canShot = false;
            li.SetActive(true);
            an.SetActive(false);
        }
        else
        {
            canShot = true;
            li.SetActive(false);
            an.SetActive(true);
            life -= Time.deltaTime * 0.01f;
        }
        


       

        if (nowRunTime <= 0)
            canRun = false;
        else
            canRun = true;
        if (!isRunning && nowRunTime<runTime)
        {
            nowRunCD -= Time.deltaTime;
            if (nowRunCD <= 0)
                nowRunTime += 2 * Time.deltaTime;
        }
           

        if(isDamaged)
        {
            if (canBeDamaged)
            {
                ShowAnimation();
                life--;
                SoundManager.instance.SetEffect(3);
                SoundManager.instance.PlayEffect();
                nowDamagedTimer = damagedTimer;
                canBeDamaged = false;
                //ShowAnimation();
                return;
            }
            else
            {
                nowDamagedTimer -= Time.deltaTime;
                if(nowDamagedTimer<=0)
                {
                    isDamaged = false;
                    canBeDamaged = true;
                }
                return;
            }

        }


        jumpTimer += jumpTime * Time.deltaTime;
        animator.speed = 1;

        //检测
        CheckLanding();             //根据着地更新状态
        AlterByState();             //根据状态更新限制条件


        if (isShoting)              //瞄准状态检测
        {
            ShowAnimation();
            return;
        }

        if (canShot && hasGun)
            gun.SetActive(true);
        else
            gun.SetActive(false);

        //移动操作
        moveVal = Input.GetAxis("Horizontal");
        if (canMove && moveVal != 0)
            MoveX(moveVal);
        else if( moveVal == 0)
            StopMovingX();

        //下蹲操作
        verVal = Input.GetAxis("Vertical");
        if (canCorch && verVal < 0)
            Corch();
        else
            isCorching = false;

        //跳跃操作
        jumpVal = Input.GetAxis("Jump");
        if (canJump && jumpTimer > jumpTime && jumpVal > 0)
        {
            jumpTimer = 0;
            Jump();
        }

        //攀爬操作
        climbVal = Input.GetAxis("Vertical");
        if (canClimb && climbVal!=0)
            Climb();
        if(isClimbing)
        {
            if (climbVal == 0)
            {
                rigidbody2.velocity = new Vector2(rigidbody2.velocity.x, 0);
                animator.speed = 0;
            }
            
            if (isLanding)
                rigidbody2.isKinematic = false;
            else
                rigidbody2.isKinematic = true;
        }
        
	}

    //移动
    void MoveX(float val)
    {
        if (val < 0)
            transform.localScale = new Vector3(-1* scaleX, transform.localScale.y, transform.localScale.z);
        if (val > 0)
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);

        if (Input.GetKey(KeyCode.LeftShift) && canRun/*&& isLanding*/)
        {
            if (san <= 3)
                moveSpeed = runSpeed * 4 / 5;
            else
                moveSpeed = runSpeed;
            nowRunTime -= Time.deltaTime;
            nowRunCD = runCD;
            isRunning = true;
        }
        else
        {
            if (san <= 3)
                moveSpeed = walkSpeed * 4 / 5;
            else
                moveSpeed = walkSpeed;
            isRunning = false;
        }
        rigidbody2.velocity = new Vector2(val * moveSpeed, rigidbody2.velocity.y);

        isStaticX = false;
        isMovingX = true;
        isCorching = false;
        ShowAnimation();
        ShowAudio();
    }

    void StopMovingX()
    {
        rigidbody2.velocity = new Vector2(0, rigidbody2.velocity.y);

        isStaticX = true;
        isMovingX = false;
        isCorching = false;
        ShowAnimation();
        ShowAudio();
    }

    //跳跃
    void Jump()
    {
        rigidbody2.velocity = new Vector2(rigidbody2.velocity.x,jumpSpeed * jumpVal);

        isJumping = true;
        isCorching = false;
        isClimbing = false;
        ShowAnimation();
    }
    
    //蹲下
    void Corch()
    {
        isCorching = true;
        ShowAnimation();
    }

    //根据状态修改限制条件
    void AlterByState()
    {

        if (isMovingX)                                   //isMovingX和isStaticX刚好相反可以考虑取消其中一个
        {
            canCorch = false;
        }
        if (isLanding)
        {
            canJump = true;
            canCorch = true;
        }
        if (isJumping)
        {
            canJump = false;                            //不能二段跳
            canCorch = false;
        }
        
        if(isStaticX)
        {
            
        }

        if (isCorching)
        {
            canMove = false;
        }
        else
            canMove = true;
    }

    //检测着地状态
    public void CheckLanding()
    {
        Vector3 origin;         //检测起点
        Vector3 direction;      //检测方向
        LayerMask layerMask1;    //检测对象层
        LayerMask layerMask2;
        float depth;            //检测深度

        depth = 1.4f;
        origin = transform.position;
        direction = Vector3.down;                       
        layerMask1 = LayerMask.GetMask("Obstacle");      //对象层为障碍
        layerMask2 = LayerMask.GetMask("Monster");
   
        Debug.DrawRay(origin, depth * direction, Color.red);        //显示射线

        if (Physics2D.Raycast(origin, direction, depth, layerMask1)||Physics2D.Raycast(origin,direction,depth,layerMask2))
        {
            isLanding = true;              //着地状态
            isJumping = false;
        }
        else
            isLanding = false;
    }

    public bool CheckOnLanding()
    {
        Vector3 origin;         //检测起点
        Vector3 direction;      //检测方向
        LayerMask layerMask;    //检测对象层
        float depth;            //检测深度
        float offset;

        depth = 0.2f;
        offset = 1.6f;
        origin = transform.position + Vector3.down * offset;
        direction = Vector3.down;
        layerMask = LayerMask.GetMask("Obstacle");      //对象层为障碍


        Debug.DrawRay(origin, depth * direction, Color.red);        //显示射线

        if (Physics2D.Raycast(origin, direction, depth, layerMask))
            return true;
        else
            return false;
            
    }

    //生命值相关
    public void ReduceLife(float n)
    {
        life -= n;
        CheckLife();
    }
    void CheckLife()
    {
        if (life <= 0)
            Debug.Log("GameOver");
    }

    //动画相关
    public void ShowAnimation()
    {
        //0Idle1跳 2跑 3走
        if(isLanding&&isMovingX)
        {
            if (isRunning)
                animator.SetInteger("State", 2);
            else
                animator.SetInteger("State", 3);
        }
        if(isLanding&&isStaticX)
        { 
            animator.SetInteger("State", 0);
        }
        if(isCorching)
        {
            animator.SetInteger("State", 4);
        }
        if(isJumping)
        {
            animator.SetInteger("State", 1);
        }
        if(isClimbing)
        {
            animator.SetInteger("State", 5);
        }
        else
            rigidbody2.isKinematic = false;

        if(isOpending)
        {
            animator.SetInteger("State", 6);
        }
        if(isClosing)
        {
            animator.SetInteger("State", 7);
        }

        if(isShoting)
        {
            animator.SetInteger("State", 8);
        }

        if(isShot)
        {
            animator.SetInteger("State", 9);
            isShot = false;
        }

        if(isDamaged)
        {
            animator.SetInteger("State", 10);

        }

        if(isDead)
        {
            animator.SetInteger("State", 11);
        }
    }

    //音频相关
    public void ShowAudio()
    {
        

        if (isLanding && isMovingX)
        {
            if (isRunning)
            {
                if (audioIndex != 1)
                {
                    audioIndex = 1;
                    SoundManager.instance.SetEffect(audioIndex);
                    SoundManager.instance.SpeedEffect(1.5f);
                    SoundManager.instance.PlayEffect();
                }
            }
            else
            {
                if (audioIndex != 0)
                {
                    audioIndex = 0;
                    SoundManager.instance.SetEffect(audioIndex);
                    SoundManager.instance.SpeedEffect(1);
                    SoundManager.instance.PlayEffect();
                }
            }
        }

        if(isLanding && isStaticX)
        {
            audioIndex = -1;
            SoundManager.instance.StopEffect();
        }
        if (isJumping)
        {
            if (CheckOnLanding() && rigidbody2.velocity.y<0)
            {
                if (audioIndex != 2)
                {
                    audioIndex = 2;
                    SoundManager.instance.SetEffect(audioIndex);
                    SoundManager.instance.SpeedEffect(1);
                    SoundManager.instance.PlayEffect();
                }
            }
            else
            {
                audioIndex = -1;
                SoundManager.instance.StopEffect();
            }
        }
    }

    //san值相关
    public void ReduceSan()
    {
        san--;
        CheckSan();
    }
    public void AddSan()
    {
        san++;
        if (san > 10)
            san = 10;
        CheckSan();
    }
    public void CheckSan()
    {
        if (san <= 0)
            Debug.Log("GameOver");
    }

    //攀爬相关
    public void Climb()
    {
        rigidbody2.velocity = new Vector2(rigidbody2.velocity.x, climbSpeed * climbVal);
        isClimbing = true;
        isJumping = false;
        isCorching = false;
        ShowAnimation();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Climbed")
            canClimb = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Climbed")
        {
            canClimb = false;
            isClimbing = false;
        }
    }

    //开门相关
    public void ResetDoor()
    {
        isOpending = false;
        isClosing = false;
    }
    public void OpenDoor()
    {
        isOpending = true;
        ShowAnimation();
        Invoke("ResetDoor", 0.3f);
    }
    public void CloseDoor()
    {
        isClosing = true;
        ShowAnimation();
        Invoke("ResetDoor", 0.3f);
    }
}
