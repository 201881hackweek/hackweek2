using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerofplottoview : MonoBehaviour {


    private bool flag=false;//用于判断角色一开始有没有在触发范围内//暂定吧 不知道有没有人物一开始就staytrigger的情况.....大概没有吧...
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (flag) return;
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Player")
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }

        flag = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
