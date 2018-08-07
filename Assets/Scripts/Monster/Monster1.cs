using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1 : MonoBehaviour
{

    /*
         1.chase方法作为最基本的怪物AI，要改成横板的追击。
         添加一个是否追击的bool变量(默认false）该变量由管理器动态管理，该脚本不用动它，只需声明即可;
         2.改进chase方法，新声明chase1,chase2。比如chase1是玩家在怪物正前方时才追击
         可能需要用到的东西:射线检测。判断怪物的面朝向通过transform中的scale.x，1为向右，-1为向左。
         有什么问题尽管问。
         3.总之可以多写几种chase方法，自由发挥吧。
         4.加特殊动作，每个写一个方法。比如跳起。
         5.加判断方法，在update里调用。此方法控制怪物是 chase还是 用特殊动作。
         判断所用的bool变量统一用can前缀，具体参考Player脚本。

         6.自由发挥吧，可以多写一下有趣的方法，我最后把他们整理。
        
         一下是一些建议:
         1.怪物移动应该只有向左或者向右，(最基本的完成后可以加点其他方向的移动）
         所以在玩家相对位置被判断出来后，应该判断向左还是向右移动，在调用Move(Vector3.right/left);
         2.随便问我。
    */

    // 状态
    public int life;
    public int damage;
    public float speed;
    public float scaleX;
    public float PATROL_RATE;	    //巡逻时的速度比例
    public GameObject target;       //玩家对象
    public Vector3 chaseDir;        //移动方向

    // 追赶控制
    public float RUN_RATE;	        //暴走时的速度比例
    public float stayAngry;           //暴走时间倒计时（计数器）
    public float ANGRY_TIME;          //暴走时间限制（限制）

    // 攻击控制
    public bool canSee;             //检测发现角色
    public bool canAttack;          //检测能否攻击
    public int waitAttack;          //准备攻击倒计时（计数器）
    public int ATTACK_DELAY;        //攻击延迟的帧数（限制）（本来就没多少血，每帧打一次似乎太残暴了）   

    public bool canJump;
    public bool canRush;

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
        life = 2;
        damage = 1;
        speed = 5f;
        PATROL_RATE = 0.7f;
        RUN_RATE = 3f;

        ANGRY_TIME = 0.5f;
        stayAngry = ANGRY_TIME;    //暴走计时初始化，设定维持暴走的帧数
        ATTACK_DELAY = 5;
        waitAttack = ATTACK_DELAY; //攻击延迟初始化，设定多少帧攻击一次
    }

    void Update()
    {
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
                waitAttack--; //能攻击但是每到时候就需要倒计时
            }
        }
        else //如果不能攻击就追逐
        {
            if (stayAngry > 0) //如果处在刚看见角色的暴走状态
            {
                ChaseRun();
                stayAngry-=Time.deltaTime;
            }
            else //恢复常态追击
            {
                ChaseNormal();
            }
        }
    }

    // 巡逻控制方法：（2级方法）规定巡逻时的行为
    public void Patrol()
    {
        stayAngry = ANGRY_TIME;                 //暴走倒计时重新冷却，准备下一次暴走
        float patrolSpeed = speed * PATROL_RATE;
        inEdge = CheckEdge();                   //判断是否处于边缘
        inFront = CheckFront();	        //判断面前是否有障碍物或者怪物

        if (inEdge || inFront)	//处在边缘or有障碍，都要触发反向巡逻
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

        depth = 4f;
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
        float depth1,depth2;            //检测深度
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
        Debug.DrawRay(origin+direction*depth2, depth1 * direction, Color.green);        //显示射线

        inWall = Physics2D.Raycast(origin, direction, depth1, layerMask1);
        inMonster = Physics2D.Raycast(origin+direction*depth2, direction, depth1, layerMask2);
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
}

