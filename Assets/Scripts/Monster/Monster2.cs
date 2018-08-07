using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster2 : MonoBehaviour {


    /*
     *  canDodge表示能否闪避，建议在其为true的时候按照随机生成数字的奇偶来决定子弹射出的时候，是有或无碰撞体的子弹
     */

    // 状态
    public float MAX_LIFE;
    public float life;
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
    public float stayAngry;         //暴走时间倒计时（计数器）
    public float ANGRY_TIME;        //暴走时间限制（限制）

    // 攻击控制
    public bool canSee;             //检测发现角色
    public bool canAttack;          //检测能否攻击
    public float waitAttack;        //准备攻击倒计时（计数器）
    public float BASIC_ATTACK_DELAY;
    public float ATTACK_DELAY;      //攻击延迟的帧数（限制）（本来就没多少血，每帧打一次似乎太残暴了）
    public bool hasDamaged;         //上次追击角色造成过伤害

    public bool canDodge;

    public bool inEdge;             //检测边缘
    public bool inFront; 		    //检测正前方障碍物
    public bool inShelter;          //检测怪物视线遮挡
    public bool inVirsion;          //检测怪物视角
    public float virsionResult;


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
        MAX_LIFE = 2;
        life = MAX_LIFE;
        BASIC_DAMAGE = 1;                   //（可被hate改变的变量）
        damage = BASIC_DAMAGE;
        hate = 0;
        BASIC_SPEED = 2f;                   //（可被hate改变的变量）
        speed = BASIC_SPEED;
        PATROL_RATE = 0.7f;
        RUN_RATE = 6f;

        canDodge = false;

        ANGRY_TIME = 0.5f;
        stayAngry = ANGRY_TIME;    //暴走计时初始化，设定维持暴走的帧数
        BASIC_ATTACK_DELAY = 0.5f;
        ATTACK_DELAY = BASIC_ATTACK_DELAY;  //（可被hate改变的变量）
        waitAttack = ATTACK_DELAY; //攻击延迟初始化，设定多少帧攻击一次
        hasDamaged = true;
    }

    void Update()
    {
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
            ChaseAndAttack();
        }
        // 待机状态，巡逻
        else
        {
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
        layerMask = LayerMask.GetMask("Obstacle");
        Debug.DrawRay(origin, distance, Color.red);
        //此射线由怪物指向玩家，途中若碰到障碍则返回ture;
        inShelter = Physics2D.Raycast(origin, distance, distance.magnitude, layerMask);


        // 怪物面向与视线在同一侧，乘积必然为正
        virsionResult = (distance.x / distance.magnitude) * transform.localScale.x / scaleX;
        inVirsion = (virsionResult >= 0.5 && virsionResult <= 1);
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
            ChaseNormal();
        }

        canAttack = CheckNear();
        if (canAttack)
        {
            if (waitAttack > 0)//除非攻击倒计时清零
            {
                Attack();
                waitAttack = ATTACK_DELAY; //重置攻击倒计时
            }
            else
            {
                waitAttack -= Time.deltaTime; //能攻击但是每到时候就需要倒计时
            }
        }

    }

    // 巡逻控制方法：（2级方法）规定巡逻时的行为
    public void Patrol()
    {
        float patrolSpeed = speed * PATROL_RATE;
        inEdge = CheckEdge();                   //判断是否处于边缘
        inFront = CheckFront();	        //判断面前是否有障碍物或者怪物

        if (inEdge || inFront || stayAngry < ANGRY_TIME)	//处在边缘or有障碍or角色突然从视野中消失，都要触发反向巡逻
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
        gameObject.transform.Translate(dir * currentSpeed * Time.deltaTime);
    }

    //边缘判断方法：（3级方法）检测怪物是否到达了所在层的边缘
    public bool CheckEdge()
    {
        Vector3 origin;         //检测起点
        Vector3 direction = new Vector3(transform.localScale.x, -1, 0).normalized; // 面向俯角45度
        LayerMask layerMask;    //检测对象层
        float depth;            //检测深度

        depth = 3f;
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
        depth1 = 1f;
        depth2 = 1.2f;
        origin = transform.position;
        layerMask1 = LayerMask.GetMask("Wall");      //对象层为障碍
        layerMask2 = LayerMask.GetMask("Monster");       //对象层为其他怪物

        Debug.DrawRay(origin, depth1 * direction, Color.blue);        //显示射线
        Debug.DrawRay(origin, depth2 * direction, Color.green);

        inWall = Physics2D.Raycast(origin, direction, depth1, layerMask1);
        inMonster = Physics2D.Raycast(origin + direction * depth2, direction, depth2, layerMask2);
        return (inWall || inMonster);                      //有表示正前方有障碍物
    }

    //攻击判断方法：（3级方法）检测是否紧邻
    public bool CheckNear()
    {
        Vector3 origin;         //检测起点
        Vector3 direction = (target.transform.position - gameObject.transform.position).normalized;
        LayerMask layerMask;    //检测对象层
        float depth;            //检测深度

        depth = 0.5f;
        origin = transform.position;
        layerMask = LayerMask.GetMask("People");      //对象层为角色


        Debug.DrawRay(origin, depth * direction, Color.blue);        //显示射线

        return Physics2D.Raycast(origin, direction, depth, layerMask); //表示玩家处于攻击范围之内
    }

    //攻击方法：（）（占位）
    public void Attack()
    {
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
        Move(chaseDir, chaseSpeed);
    }

    //追逐-跳跃方法：有这个方法则角色不能通过调上平台的方式甩掉怪物
    public void ChaseJump()
    {

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
}
