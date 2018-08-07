using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Monsterm : MonoBehaviour
{

    /*
     *  1、闪避的处理方式：血量少于一半的时候会逃跑（Escape），canDodge表示能否闪避，
     *                  建议在其为true的时候按照随机生成数字的奇偶来决定子弹射出的时候，
     *                  是有或无碰撞体的子弹。
     *  2、报复的处理方式：设置为角色在进入到逃出怪物视野期间，怪物都没有造成任何一点伤害时，
     *                  怪物的仇恨值增加。
     *  3、中枪的处理方法：不用在player脚本中调用了，我直接写一个每帧检测生命值的判断，
     *                  减少了就使用TurnToPlayer面向角色那一边。
     *  4、自愈的处理方法：我的设计是血量少于一半的时候有50%闪避率并且会不管角色全力逃跑，
     *                  所以增加了一个自愈方法，这就要求角色必须在一定时间内杀死怪物。
     *  5、上楼的处理方法：只有一个怪物，不能让角色逃到别的楼层怪物就只会傻傻地在一层巡逻了，
     *                  我的设计是在patrol巡逻时，如果碰到楼梯，有50%的概率会往玩家所在的层走。
     */

    // 状态
    public float MAX_LIFE;
    public float beforeLife;
    public float life;
    public float RECOVER_LIFE;       //每次自愈恢复的生命值
    public float RECOVER_TIME;       //自愈的单位时间
    public float waitRecover;        //等待自愈的时间
    public float BASIC_DAMAGE;
    public float damage;
    public float BASIC_SPEED;
    public float speed;
    public int hate;
    public float scaleX;
    public float PATROL_RATE;	    //巡逻时的速度比例
    public GameObject target;       //玩家对象
    public Vector3 chaseDir;        //移动方向

    // 追赶控制
    public float RUN_RATE;	        //暴走时的速度比例
    public float ESCAPE_RATE;       //逃跑时的速度比例
    public float stayAngry;         //暴走时间倒计时（计数器）
    public float ANGRY_TIME;        //暴走时间限制（限制）

    // 攻击控制
    public bool canSee;             //检测发现角色
    public bool canAttack;          //检测能否攻击
    public float waitAttack;        //准备攻击倒计时（计数器）
    public float BASIC_ATTACK_DELAY;
    public float ATTACK_DELAY;      //攻击延迟的帧数（限制）（本来就没多少血，每帧打一次似乎太残暴了）
    public bool hasDamaged;         //上次追击角色造成过伤害
    public float attackCD;
    public float nowAttackCD;

    public bool canDodge;

    public bool inStairs;           //检测楼梯
    public bool inEdge;             //检测边缘
    public bool inFront; 		    //检测正前方障碍物
    public bool inShelter;          //检测怪物视线遮挡
    public bool inVirsion;          //检测怪物视角
    public float virsionResult;


    bool isWalking;
    bool isRunning;
    bool isDamaged;
    bool isDead;
    bool isAttacking;
    bool startAttack;
    bool hasDead;
    bool isEscape;
    

    public Animator animator;
    float timer;
    float nowTimer;

    public bool firstBehavior;

    // Use this for initialization
    void OnEnable()
    {
        scaleX = transform.localScale.x;
        target = GameObject.Find("Player");
        //所有数值的初始化放到Init()方法中，在这个脚本中Init()方法是abstract类型，不用写内容
        //不同类型的怪物继承这个脚本类再改写(override)Init()方法，从而在初始化时有不同的life,speed等;
        Init();
    }

    public void Init()
    {
        MAX_LIFE = 6f;
        life = MAX_LIFE;
        beforeLife = life;
        RECOVER_LIFE = 1f;
        RECOVER_TIME = 20f;                  // 暂时设定为20秒加一点血量
        waitRecover = RECOVER_TIME;
        BASIC_DAMAGE = 1;                   //（可被hate改变的变量）
        damage = BASIC_DAMAGE;
        hate = 0;
        BASIC_SPEED = 5f;                   //（可被hate改变的变量）
        speed = BASIC_SPEED;
        PATROL_RATE = 0.7f;
        RUN_RATE = 4f;
        ESCAPE_RATE = 3f;

        canDodge = false;

        ANGRY_TIME = 0.8f;
        stayAngry = ANGRY_TIME;    //暴走计时初始化，设定维持暴走的帧数
        BASIC_ATTACK_DELAY = 0.8f;
        ATTACK_DELAY = BASIC_ATTACK_DELAY;  //（可被hate改变的变量）
        waitAttack = ATTACK_DELAY; //攻击延迟初始化，设定多少帧攻击一次
        attackCD = 2f;
        nowAttackCD = 0;

        hasDamaged = true;

        timer = 1f;
        nowTimer = timer;

        animator = gameObject.GetComponent<Animator>();
        isDamaged = false;
        isWalking = true;
        isRunning = false;
        isDead = false;
        isAttacking = false;
        startAttack = false;
        hasDead = false;
        isEscape = false;
    }

    void Update()
    {

        if (hasDead)
            return;

        if(life <= 0)
        {
            StartCoroutine("CheckDead");
            hasDead = true;
        }
        
        if(isDamaged)
        {
            if(nowTimer==timer)
            {
                ShowAnimation();
            }
            if(nowTimer>0)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                nowTimer -= Time.deltaTime;
                return;
            }
            else
            {
                isDamaged = false;
                nowTimer = timer;
            }
        }


        if (life < beforeLife) //如果受到了伤害
        {
            beforeLife = life; //更新状态         
            TurnToPlayer();    //
        }

        Recover();             //生命值慢慢恢复 
        if (life <= MAX_LIFE * 0.5f)//判断能否闪避
        {
            canDodge = true;
        }
        else canDodge = false;

        // 怪物状态控制；检查是否看见角色
        canSee = CheckSee();
        // 激活状态，追击或攻击角色
        if (canSee)
        {
            if (life <= MAX_LIFE * 0.5f)
            {
                isEscape = true;
                Escape();
            }
            else
            {
                isEscape = false;
                ChaseAndAttack();
            }

        }
        // 待机状态，巡逻
        else
        {
            if (life > MAX_LIFE * 0.5f)
                isEscape = false;
            isRunning = false;
            Patrol();
        }
    }

    //视线判断方法：（2级方法）检测怪物是否看见了玩家
    public bool CheckSee()//对角色的距离向量
    {
        Vector3 origin;
        Vector3 distance;
        LayerMask layerMask;    //检测对象层

        origin = transform.position;
        distance = target.transform.position - gameObject.transform.position;
        //（看完后可以删掉）检测的是平台是否遮挡了视线，应该是同一个对象层，所以没有改
        layerMask = LayerMask.GetMask("Wall");
        //Debug.DrawRay(origin, distance, Color.red);
        //此射线由怪物指向玩家，途中若碰到障碍则返回ture;
        inShelter = Physics2D.Raycast(origin, distance, distance.magnitude, layerMask);


        // 怪物面向与视线在同一侧，乘积必然为正
        virsionResult = (distance.x / distance.magnitude) * transform.localScale.x / scaleX;
        inVirsion = (virsionResult >= 0.5f && virsionResult <= 1);
        if (inShelter || !inVirsion)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // 追击控制方法：（2级方法）控制各种追击模式	
    public void ChaseAndAttack()
    {
        

        canAttack = CheckNear();

        if (canAttack && nowAttackCD <= 0)
        {
            startAttack = true;
            nowAttackCD = 1f;
        }

        if(startAttack)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            isAttacking = true;

            ShowAnimation();

            if (waitAttack <= 0)//除非攻击倒计时清零
            {
                if (CheckAttack())
                {
                    Attack();
                    nowAttackCD = attackCD; //攻击冷却
                }
                isAttacking = false;
                ShowAnimation();
                waitAttack = ATTACK_DELAY; //重置攻击倒计时
                startAttack = false;
            }
            else
            {
                waitAttack -= Time.deltaTime; //能攻击但是每到时候就需要倒计时
            }
            return;
        }

        if (nowAttackCD > 0)
        {
            nowAttackCD -= Time.deltaTime; //攻击进入CD
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            return;
        }

        if (stayAngry == ANGRY_TIME)//如果是刚看见玩家设置“未造成伤害”
        {
            hasDamaged = false;
        }

        if (stayAngry > 0) //如果处在刚看见角色的暴走状态
        {
            ChaseRun();
            stayAngry -= Time.deltaTime;
        }
        else //恢复常态追击
        {
            isRunning = false;
            ChaseNormal();
        }


       

    }

    // 巡逻控制方法：（2级方法）规定巡逻时的行为
    public void Patrol()
    {
        float patrolSpeed = speed * PATROL_RATE;
        inEdge = CheckEdge();                   //判断是否处于边缘
        inFront = CheckFront();	        //判断面前是否有障碍物或者怪物

        if (( inFront)&&isEscape )
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            return;
        }
        if (( inFront ||  CheckNear2())&&!isEscape )	//处在边缘or有障碍or角色突然从视野中消失，都要触发反向巡逻
        {
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-scaleX, transform.localScale.y, transform.localScale.z);
                chaseDir = Vector3.left; //向反方向移动
                Move(chaseDir, patrolSpeed);
            }
            else
            {
                transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
                chaseDir = Vector3.right;
                Move(chaseDir, patrolSpeed);
            }
        }
        else
        {
            if (transform.localScale.x > 0)
            {
                chaseDir = Vector3.right; //面向移动
                Move(chaseDir, patrolSpeed);
            }
            else
            {
                chaseDir = Vector3.left;
                Move(chaseDir, patrolSpeed);
            }
        }

        if (stayAngry < ANGRY_TIME && !hasDamaged)//如果角色逃走迫使怪物进入巡逻状态，而怪物没有造成任何伤害
        {
            UpdateHate(10);//累计10点仇恨值
        }
        stayAngry = ANGRY_TIME;                 //暴走倒计时重新冷却，准备下一次暴走
    }

    //移动实现方法：（3级方法）
    public void Move(Vector3 dir, float currentSpeed)//增强方法的通用性，Move方法用于控制物体向dir方向移动。
    {
        waitAttack = ATTACK_DELAY;

        gameObject.GetComponent<Rigidbody2D>().velocity = dir * currentSpeed;

        isWalking = true;

        ShowAnimation();
    }

    //边缘判断方法：（3级方法）检测怪物是否到达了所在层的边缘
    public bool CheckEdge()
    {
        Vector3 origin;         //检测起点
        Vector3 direction = new Vector3(transform.localScale.x, -1, 0).normalized; // 面向俯角45度
        LayerMask layerMask;    //检测对象层
        float depth;            //检测深度

        depth = 5f;
        origin = transform.position;
        layerMask = LayerMask.GetMask("Obstacle");      //对象层为障碍


        Debug.DrawRay(origin, depth * direction, Color.red);        //显示射线

        return !Physics2D.Raycast(origin, direction, depth, layerMask); //没有检测到表示处在边缘
    }

    //障碍判断方法：（3级方法）检测怪物面前是否有墙壁或者怪物
    public bool CheckFront()
    {
        Vector3 origin;         //检测起点
        Vector3 direction; 		// 正前方
        LayerMask layerMask1;   //检测对象层
        LayerMask layerMask2;
        float depth1;            //检测深度
        float depth2;
        bool inWall;
        bool inMonster;

        if (transform.localScale.x > 0)
        {
            direction = Vector3.right;
        }
        else
        {
            direction = Vector3.left;
        }
        depth1 = 4f;
        depth2 = 1.2f;
        origin = transform.position;
        layerMask1 = LayerMask.GetMask("Wall");      //对象层为障碍
        layerMask2 = LayerMask.GetMask("Monster");       //对象层为其他怪物

        Debug.DrawRay(origin, depth1 * direction, Color.blue);        //显示射线
       // Debug.DrawRay(origin, depth2 * direction, Color.green);

        inWall = Physics2D.Raycast(origin, direction, depth1, layerMask1);
        inMonster = Physics2D.Raycast(origin+depth1*direction, direction, depth2, layerMask2);
        return (inWall);                      //有表示正前方有障碍物 我把inMonster删掉了
    }

    //攻击判断方法：（3级方法）检测是否紧邻
    public bool CheckNear()
    {
        Vector3 origin;         //检测起点
        Vector3 direction = (target.transform.position - gameObject.transform.position).normalized;
        LayerMask layerMask;    //检测对象层
        float depth;            //检测深度
        
        depth = 2.5f;
        origin = transform.position;
        layerMask = LayerMask.GetMask("People");      //对象层为角色


        Debug.DrawRay(origin, depth * direction, Color.blue);        //显示射线

        return Physics2D.Raycast(origin, direction, depth, layerMask); //表示玩家处于攻击范围之内
    }

    public bool CheckNear2()
    {
        Vector3 origin;         //检测起点
        Vector3 direction = (target.transform.position - gameObject.transform.position).normalized;
        LayerMask layerMask;    //检测对象层
        float depth;            //检测深度
        float offsetY;

        offsetY = 1.1f;
        depth = 3f;
        origin = transform.position;
        layerMask = LayerMask.GetMask("People");      //对象层为角色


        Debug.DrawRay(origin, depth * direction, Color.green);        //显示射线

        if (Mathf.Abs(target.transform.position.y - gameObject.transform.position.y) > offsetY)
        {
            return false;
        }

        return Physics2D.Raycast(origin, direction, depth, layerMask); //表示玩家处于攻击范围之内
    }

    //攻击命中判断
    public bool CheckAttack()
    {
        Vector3 origin;         //检测起点
        Vector3 direction = (target.transform.position - gameObject.transform.position).normalized;
        LayerMask layerMask;    //检测对象层
        float depth;            //检测深度

        depth = 5f;
        origin = transform.position;
        layerMask = LayerMask.GetMask("People");      //对象层为角色


        Debug.DrawRay(origin, depth * direction, Color.gray);        //显示射线

        return Physics2D.Raycast(origin, direction, depth, layerMask); //表示玩家处于攻击范围之内
    }

    //攻击方法：（）（占位）
    public void Attack()
    {
        if (Player.instance.isDamaged)
            return;

        SoundManager.instance.SetEffect(4);
        SoundManager.instance.PlayEffect();

        if (Player.instance.transform.localScale.x * transform.localScale.x > 0)
        {
            if (Player.instance.transform.localScale.x > 0)
                Player.instance.transform.localScale = new Vector3(-Player.instance.scaleX, Player.instance.transform.localScale.y, Player.instance.transform.localScale.z);
            else
                Player.instance.transform.localScale = new Vector3(Player.instance.scaleX, Player.instance.transform.localScale.y, Player.instance.transform.localScale.z);

        }
        Player.instance.isDamaged = true;
        
        //修改玩家的life属性，没看到修改的方法所以没写
        UpdateHate(-30);
    }

    //追逐-常规方法：(3级方法)玩家在哪边就往哪边追
    public void ChaseNormal()
    {
        float chaseSpeed = speed;

        if (target.transform.position.x - gameObject.transform.position.x > 0)
        {
            chaseDir = Vector3.right;
        }
        else
        {
            chaseDir = Vector3.left;
        }
        chaseDir.Normalize();          //方向向量应单位化
        Move(chaseDir, chaseSpeed);
    }

    //追逐-奔跑方法：（3级方法）向玩家奔跑，比如刚看见角色的"暴走”
    public void ChaseRun()
    {
        float chaseSpeed = speed * RUN_RATE;//考虑到只有一帧，那就三倍速？

        if (target.transform.position.x - gameObject.transform.position.x > 0)
        {
            chaseDir = Vector3.right;
        }
        else
        {
            chaseDir = Vector3.left;
        }
        chaseDir.Normalize();          //方向向量应单位化
        isRunning = true;
        Move(chaseDir, chaseSpeed);
    }


    //仇恨更新方法：在仇恨值改变的时候调用
    public void UpdateHate(int change)
    {
        float speedAddRate;
        float damageAddRate;
        float attackSpeedAddRate;

        hate += change;
        if (hate < 0)
        {
            hate = 0;
        }

        speedAddRate = 1f + hate / 200f;
        damageAddRate = 1f + hate / 500f;
        attackSpeedAddRate = 1f + hate / 1000f;
        if (speedAddRate > 1.5f)
        {
            speedAddRate = 1.5f;
        }
        if (damageAddRate > 1.2f)
        {
            damageAddRate = 1.2f;
        }
        if (attackSpeedAddRate > 1.3f)
        {
            attackSpeedAddRate = 1.3f;
        }

        speed = BASIC_SPEED * speedAddRate;
        damage = BASIC_DAMAGE * damageAddRate;
        ATTACK_DELAY = BASIC_ATTACK_DELAY / attackSpeedAddRate;
    }

    //逃跑方法：如果血量少于一半直接进入逃跑状态
    public void Escape()
    {
        Vector3 escapeDir;
        float escapeSpeed = speed * ESCAPE_RATE;

        //如果怪物面向右侧，也能看见角色，那么肯定是向左逃
        if (transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-scaleX, transform.localScale.y, transform.localScale.z);
            escapeDir = Vector3.left; //向反方向移动
            if (inEdge||inFront)
            {
                Debug.Log("4");
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                return;
            }
            Move(escapeDir, escapeSpeed);
            
        }
        else
        {
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
            escapeDir = Vector3.right;
            if (inEdge || inFront)
            {
                Debug.Log("4");
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                return;
            }
            Move(escapeDir, escapeSpeed);
        }
    }

    //面向角色方法：收到伤害之后会面向角色进入追击状态
    public void TurnToPlayer()
    {
        //收到伤害就面向角色
        if ((target.transform.position.x - gameObject.transform.position.x) * transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-scaleX, transform.localScale.y, transform.localScale.z);
        }
    }

    //自愈方法：一定时间会慢慢恢复血量
    public void Recover()
    {
        if (life == MAX_LIFE)
        {
            return;
        }
        if (waitRecover > 0)
        {
            waitRecover -= Time.deltaTime;
        }
        else
        {
            life += RECOVER_LIFE;
            beforeLife = life;
            waitRecover = RECOVER_TIME;
        }
    }

    IEnumerator CheckDead()
    {
        float deadTime = 3f;
        if(life<=0)
        {
            isDead = true;
            ShowAnimation();
            while(deadTime>=0)
            {
                deadTime -= Time.deltaTime;
                return null;
            }
            //在这里加上道具
            Debug.Log("dead");
            return null;
        }
        return null;
    }

    public void ShowAnimation()
    {
        if(isWalking)
        {
            animator.SetInteger("State",0);
        }
        if(isRunning)
        {
            animator.SetInteger("State", 1);
        }
        if(isAttacking)
        {
            animator.SetInteger("State", 2);
        }
        if(isDamaged)
        {
            animator.SetInteger("State", 3);
        }
        if(isDead)
        {
            animator.SetInteger("State", 4);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Bullet")
            return;
        isDamaged = true;
        life--;
    }
}

