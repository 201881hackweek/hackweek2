using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialog2: MonoBehaviour
{
    private SceneEventManager sceneeventmanager;
    public Color coloroffset;
    private Color origincolor;
    private bool flag = false;//判断在黑屏结束后是否先按下space 再检测keyup 否则可能直接又开始黑屏
    // Use this for initialization
    void Start()
    {
        GameObject propmanager = GameObject.Find("SceneEventManager");
        sceneeventmanager = propmanager.GetComponent<SceneEventManager>();
        coloroffset = new Color(0.1f, 0.1f, 0.1f, 0);
        origincolor = GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if(!sceneeventmanager.Dialogbox.activeSelf&&Time.time>sceneeventmanager.generalfinishtime+0.1f)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<SpriteRenderer>().color = origincolor - coloroffset;
                flag = true;
            }


            if (flag&&Input.GetKeyUp(KeyCode.Space))
            {
                GetComponent<SpriteRenderer>().color = origincolor;
                sceneeventmanager.Dialogbox.SetActive(true);
                sceneeventmanager.space = true;
				sceneeventmanager.generalnumber = 2;
                flag = false;
            }
        }

    }

    private void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().color = origincolor - coloroffset;
    }

    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = origincolor;
    }

    private void OnMouseUp()
    {
        sceneeventmanager.Dialogbox.SetActive(true);
		sceneeventmanager.generalnumber = 2;
    }

}
