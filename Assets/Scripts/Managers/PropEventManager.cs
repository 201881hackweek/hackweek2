using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PropEventManager : MonoBehaviour {
    public GameObject Dialogbox;
    public Text Dialogboxtext;
    public int propnumber = 0;//用于判断使用道具后（即把道具从背包栏拖拽到指定范围后）出现的文本内容
    public int prepropnumber = 0;//用于判断捡拾道具（即把道具从地图上拾取后）出现的提示信息
    public int addpropnumber = 0;//用于判断道具叠加弹出的提示信息//1代表第一次叠加得到的新道具的信息编号，2代表第二次，以此类推
    private int Textnumber = 0;//用于判断第几句话
    public bool flashlight = false;//用于判断是否拥有有电的手电筒
    public bool diaryupdate = false;//用于判断自己的日记是否要更新
    public int bathtubnumber = 0;//用于判断浴缸是否集齐3个要素达到烧碎片的条件：打火机 油 碎片
    //public Vector2 newposition;
    public Grids[] grids;
    public bool[] Emptygrid;


    //使用道具后出现的文本Propxtext
    //叠加道具后出现的文本Addpropxtext
    //捡拾道具后出现的文本Prepropxtext
    private string[] Prop1text = { "blablablabal", "balbalbal", "balbalbalb", "blablabalbalblablabl" };
    private string[] Prop2text = { "...", "......", "..........." };



    private string[] Prop7text = { "获得有点锈迹的钥匙：可为什么钥匙会在头发里……这些头发又是谁的？", "…这么长，应该是母亲的……" };
    private string[] Prop9text = { "啊打开了……父亲的工作资料全在这儿", "随意翻翻吧，说不定有什么意想不到的的收获" };
    private string Prop10text = "打开了……";
    private string[] Prop12text = {
        "母亲的日记写到：",
        "“……他可以随意出入我的房间，自己房间的钥匙却从来都是放在奇怪的地方",
        "……是什么地方呢？黑暗，粘稠，充满血液的地方？”……" };
    private string[] Prop13_1text = {
        "自己的日记写到：",
        "“疼痛。这对我而言不算陌生。",
        "伤害撞击在肉体上，就会产生这种强烈的电流。",
        "它们确实令人不快，但我更愿意用“麻烦”来形容。",
        "……不行。我的身体太虚弱了，没法承受太多的伤痕。",
        "这样下去，我一定会变成一团没有意识的血肉，失去一切活力和生机。",
        "……这里是我的地狱。是连逃跑的意志都会消磨殆尽的万丈深渊。",
        "所以——必须、要摆脱这一切不可。”",
        "……",
        "“今天父亲将母亲杀死了。",
        "头颅藏在一个箱子里。",
        "然后父亲将箱子放在了那个房间……那个房间是连结我和学校的唯一通道。",
        "尽管我不能上学，单是每天在窗边看着快乐的学生们，",
        "就能，唤起我仅存的，一点希望。”"};
    private string[] Prop13_2text =
    {
        "自己的日记写到：",
        "“顶楼的秘密房间里堆满了我的宝藏。”"
    };
    private string Prop14text = "好像起作用了……";
    private string[] Prop11text = {
        "父亲的日记写到：",
        "我看到儿子做的一切了。他以为将动物尸体藏在顶楼就没人发现了？",
        "上次去翻还发现了很多火柴盒……",
        "啊……有我这样一个父亲，他这一辈子，怕是无法见到光明了。",
        "哈哈哈哈哈哈哈哈哈哈……"
    };
    private string Prop17text = "打开了……";
    private string Prop25text = "……这样就没有痕迹了……";



    private string Preprop1text = "获得铅笔：平时用来写日记的笔";
    private string Preprop2text = "获得水果刀：父亲不会给零花钱，我没钱买削笔刀，所以平时我都是用家里的水果刀来削铅笔";
    private string Preprop4text = "获得手电筒：没有电了……";
    private string Preprop5text = "获得电池：南孚电池";
    private string Preprop7text = "获得梳子：以前母亲用来梳头发的。我头发短，一般抓两下就好了。";
    private string Preprop10text = "获得钥匙：好像是母亲房间的钥匙……父亲喜欢限制母亲的活动范围";
    private string Preprop11text = "获得父亲的日记";
    private string Preprop12text = "获得母亲的日记";
    private string Preprop13text = "获得自己的日记：我平时有写日记的习惯，虽然我的生活没什么好写的。父亲和母亲都写日记。写什么呢？记录这一家扭曲的生活吗？";
    private string Preprop14text = "获得酒：父亲酗酒。每次酗酒是他最疯狂的时候，也是他意识不清、反应迟钝的最脆弱的时候";
    private string Preprop15text = "获得打火机：令人感到亲切的东西。";
    private string Preprop16text = "获得油：我家几乎不做饭……所以一瓶油还是满的";
    private string Preprop17text = "获得削骨刀：今天醒来后发现自己手里拿着这个东西。当时把它扔地上没管了。现在觉得也许有点用处。";
    private string Preprop18text = "获得父亲的碎片：为什么尸块在这里？必须赶快销毁……有什么好的销毁方式吗?不留痕迹的那种？";
    private string Preprop19text = "获得钥匙：应该可以开父亲房间的门";


    private string Addprop1text = "获得铅灰：化学书上有说过，石墨可以用来润滑";
    private string Addprop2text = "获得光滑的钥匙：这下钥匙就可以用了";
    private string Addprop3text = "获得有电的手电筒：这下就可以去黑黑的房间里了";

    [System.Serializable]/////开心~~~可以直接输入想要的prefab个数啦
    public class Grids
    {
        public RectTransform grid;
    }
    // Use this for initialization
    void Start () {
        Emptygrid = new bool[16];//后续调个数
        for(int i=0;i<16;i++)
        {
            Emptygrid[i] = true;//格子一开始为空
        }

    }
	
	// Update is called once per frame
	void Update () {
        Predialogchoose();
        Dialogchoose();
        Adddialogchoose();
	}

    public void Updategridposition(RectTransform child)
    {
        for(int i=0;i<16;i++)
        {
            if(Emptygrid[i])
            {
               // newposition = grids[i].grid.anchoredPosition;
                child.SetParent(grids[i].grid);
                //grids[i].grid.transform.SetAsLastSibling();
                child.anchoredPosition = new Vector2(60,-60);
                child.transform.localScale = new Vector3(1, 1, 1);
                Emptygrid[i] = false;
                break;
            }
        }
    }

    public void Dialogchoose()
    {
        if (propnumber == 0) return;
        switch(propnumber)
        {
            case 7://7梳子
                Prop7next();
                break;

            case 9:
                Prop9next();
                break;

            case 10:
                Dialogboxtext.text = Prop10text;
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    Dialogbox.SetActive(false);
                    propnumber = 0;
                }
                break;

            case 13:
                if (!diaryupdate) Prop13_1next();//判断是否更新 若未更新就原始内容
                else
                {
                    Prop13_2next();
                    //13door has opened!!!!!!!!!!
                }
                break;

        }
    }

    public void Adddialogchoose()
    {
        if (addpropnumber == 0) return;
        switch(addpropnumber)
        {
            case 1:
                Dialogboxtext.text = Addprop1text;
                if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    Dialogbox.SetActive(false);
                    addpropnumber = 0;
                }
                break;

            case 2:
                Dialogboxtext.text = Addprop2text;
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    Dialogbox.SetActive(false);
                    addpropnumber = 0;
                }
                break;
            case 3:
                Dialogboxtext.text = Addprop3text;
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    Dialogbox.SetActive(false);
                    addpropnumber = 0;
                }
                break;

        }
    }




    public void Predialogchoose()
    {
        if (prepropnumber == 0) return;
        else
        {
            switch (prepropnumber)
            {
                case 1:
                    Dialogboxtext.text = Preprop1text;
                    break;

                case 2:
                    Dialogboxtext.text = Preprop2text;
                    break;

                case 4:
                    Dialogboxtext.text = Preprop4text;
                    break;

                case 5:
                    Dialogboxtext.text = Preprop5text;
                    break;

                case 7:
                    Dialogboxtext.text = Preprop7text;
                    break;

                case 10:
                    Dialogboxtext.text = Preprop10text;
                    break;

                case 13:
                    Dialogboxtext.text = Preprop13text;
                    break;

            }
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Dialogbox.SetActive(false);
                prepropnumber = 0;
            }
        }
    }

    void Prop13_1next()
    {
        if (Textnumber < 14)
        {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Dialogboxtext.text = Prop13_1text[Textnumber];
                Textnumber++;
            }
        }
        else
        {
            if(Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Dialogbox.SetActive(false);
                Textnumber = 0;
                propnumber = 0;
            }
        }

    }

    void Prop13_2next()
    {
        if (Textnumber < 2)
        {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Dialogboxtext.text = Prop13_2text[Textnumber];
                Textnumber++;
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Dialogbox.SetActive(false);
                Textnumber = 0;
                propnumber = 0;
            }
        }
    }

    void Prop7next()
    {
        if (Textnumber < 2)
        {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Dialogboxtext.text = Prop7text[Textnumber];
                Textnumber++;
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Dialogbox.SetActive(false);
                Textnumber = 0;
                propnumber = 0;
            }

        }

    }

    void Prop9next()
    {
        if (Textnumber < 2)
        {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Dialogboxtext.text = Prop9text[Textnumber];
                Textnumber++;
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Dialogbox.SetActive(false);
                Textnumber = 0;
                propnumber = 0;
            }

        }

    }




}
