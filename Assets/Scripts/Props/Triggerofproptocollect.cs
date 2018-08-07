using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerofproptocollect : MonoBehaviour {

    private bool flag = false;//用于判断角色一开始有没有在道具的触发范围内//暂定吧 不知道有没有人物一开始就staytrigger的情况.....大概没有吧...
                              // Use this for initialization
                              //child(1)为替身 child(0)为真道具....
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (flag) return;
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            transform.GetChild(0).gameObject.SetActive(true);//真道具true 替身false
            transform.GetChild(1).gameObject.SetActive(false);
        }

        flag = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            transform.GetChild(0).gameObject.SetActive(false);//相反
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}


