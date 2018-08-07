using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {

    //怪物自身life<=0时要把自身setfalse而且从monsters中移除，并且WorldManager中monsterNum--;
    //负责里表世界切换，管理san值和产怪

    public static WorldManager instance;

    public int monsterNum;      //目前怪数
    public int maxMonsterNum;   //最大怪数
    public bool isReal;         //判断所在世界
    float timer;                //计时器
    float damagel;
    float creatVal;             //产怪乱数

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        isReal = true;
        timer = 0;
        damagel = 0.05f;
        monsterNum = 0;
        maxMonsterNum = 7;
       
    }

    void Update () {

        if (Input.GetKeyDown(KeyCode.X))
            ChangeWorld();                  //切换世界
   
        timer += Time.deltaTime;
        if (timer >= 2)        
        {
              timer = 0;
              ReadyCreat();        //产怪操作,增加num
              CheckNum();         //检测怪物数量，根据结果减少san值
        }
        if(!isReal)
        {
            Player.instance.ReduceLife(damagel * Time.deltaTime);
        }
	}
    
    public void ChangeWorld()
    {
        if (isReal)
        {
            isReal = false;
        }
        else
            isReal = true;
        MonsterManager.instance.SendMessage("Active");      //切换至里世界怪物激活，表世界怪物失效
    }

    //产怪相关
    public void SetCreatVal()                   //产生乱数
    {
        creatVal = Random.Range(0, 100);
    }

    public void ReadyCreat()
    {
        if (monsterNum >= maxMonsterNum)
        {
            monsterNum = maxMonsterNum;
            return;
        }
        if (isReal)      //表世界增怪
        {
            SetCreatVal();
            if (creatVal < 80)               
            {
                monsterNum++;
            }
        }
        else            //里世界增怪
        {
            
            //monsterNum++;
            {
                SetCreatVal();
                if(creatVal<50)
                {
                    monsterNum++;
                }
            }
            MonsterManager.instance.SetNewMonstersByNum();
        }
    }

    public void CheckNum()          //控制san值
    {
        if (isReal)
        {
            if (MonsterManager.instance.num > 10)
                Player.instance.SendMessage("ReduceSan");
            else
                Player.instance.SendMessage("AddSan");
        }
    }
}
