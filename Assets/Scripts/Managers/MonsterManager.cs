using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour {

    //储存怪物链表和怪物数量，提供产怪和激活方法

    public static MonsterManager instance;
    public List<GameObject> monsters;
    public int maxNum;
    public int num;
    public int nowNum;

    public bool isReal;

    //产怪位置相关
    public Vector3 playerPos;
    public float r1;
    public float r2;

    public Transform[] allPos;             //固定产怪点,在场景内拖

    public List<Transform> activePos;       //一次生怪中的所有位置
    public List<Transform> readyPos;        //一次生怪中的所有合法位置

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        Init();
    }

    public enum Type
    {
        NORMOL
    }

    public void Init()
    {
        monsters = new List<GameObject>();
        readyPos = new List<Transform>();
        activePos = new List<Transform>();
        maxNum = WorldManager.instance.maxMonsterNum;
        num = 0;
        r1 = 4;                                     //产怪点内半径
        r2 = 20;                                    //外半径
    }

   

    //创建对象
    public void SetMonstersByNum()                  //补充monsters链表
    {
        num = WorldManager.instance.monsterNum;
        nowNum = monsters.Count;
        for(int i = nowNum; i<num; i++)
        {
            GameObject monster = MonsterPool.instance.SetByPool();
            monsters.Add(monster);
        }
        nowNum = monsters.Count;
    }

    public void SetNewMonstersByNum()                  //用于里世界的怪物生成
    {
        num = WorldManager.instance.monsterNum;
        nowNum = monsters.Count;
        for (int i = nowNum; i < num; i++)
        {
            GameObject monster = MonsterPool.instance.SetByPool();
            monsters.Add(monster);
            Vector3 creatPos = SetCreatPos();       //设置随机位置
            monster.transform.position = creatPos;
            monster.SetActive(true);
        }
        nowNum = monsters.Count;
    }

    public Vector3 SetCreatPos()
    {
        Transform realPos;              //获取最终Transform
        readyPos.Clear();               //随机位置池

        playerPos = Player.instance.transform.position;

        foreach (Transform pos in activePos)            //对于每一个固定产怪点，若存在范围内，将其加入临时链表
        {
            if (Mathf.Abs((pos.position - playerPos).magnitude) > r1 &&
                Mathf.Abs((pos.position - playerPos).magnitude) < r2)
            {
                readyPos.Add(pos);          //将activePos中的合法对象添加到位置池中
            }
        }
        if (readyPos.Count > 0)
        {
            realPos = readyPos[Random.Range(0, readyPos.Count - 1)];          //随机选择池的位置对象
            activePos.Remove(realPos);                                        //防止后续SetPos重复
            return realPos.position;
        }
        else
            return new Vector3(0, 0, 0);         //没有合法位置，将其放入默认位置（需改进）
    }

    public void ResetPositions()                //重置monsters中对象的位置
    {
        activePos.Clear();
        activePos.AddRange(allPos);             //重置activePos
        foreach (GameObject monster in monsters)
        {
            Vector3 creatPos = SetCreatPos();       //设置随机位置
            monster.transform.position = creatPos;
        }
    }

    public void Active()
    {
        isReal = WorldManager.instance.isReal;
        if(isReal)
        {
            foreach(GameObject monster in monsters)
            {
                monster.SetActive(false);
            }
        }
        else
        {
            SetMonstersByNum();
            ResetPositions();
            foreach (GameObject monster in monsters)
            {
                monster.SetActive(true);
            }
        }
    }


    //添加/删除对象
    public void AddMonster(GameObject monster)
    {
        monsters.Add(monster);
    }
    public void AddMonster(GameObject[] monster)
    {
        monsters.AddRange(monster);
    }
    public void RemoveMonster(GameObject monster)
    {
        monsters.Remove(monster);
    }
}
