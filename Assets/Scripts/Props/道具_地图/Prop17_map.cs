using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop17_map : MonoBehaviour
{
    //地图里的17削骨刀
    private PropEventManager propeventmanager;
    public RectTransform Prop17_little;
    public Color coloroffset;
    private Color origincolor;

    // Use this for initialization
    void Start()
    {
        GameObject propmanager = GameObject.Find("PropEventManager");
        propeventmanager = propmanager.GetComponent<PropEventManager>();
        coloroffset = new Color(0.1f, 0.1f, 0.1f, 0);
        origincolor = GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<SpriteRenderer>().color = origincolor - coloroffset;
        }


        if (Input.GetKeyUp(KeyCode.Space))
        {
            RectTransform prop17 = Instantiate(Prop17_little, new Vector3(), new Quaternion(0, 0, 0, 0));
            propeventmanager.Updategridposition(prop17);
            propeventmanager.Dialogbox.SetActive(true);
            propeventmanager.diaryupdate = true;//此时更新自己的日记内容
            propeventmanager.prepropnumber = 17;//捡拾到道具后出现的提示信息
            Destroy(transform.parent.gameObject);
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
        RectTransform prop17 = Instantiate(Prop17_little, new Vector3(), new Quaternion(0, 0, 0, 0));
        propeventmanager.Updategridposition(prop17);
        propeventmanager.Dialogbox.SetActive(true);
        propeventmanager.diaryupdate = true;//此时更新自己的日记内容
        propeventmanager.prepropnumber = 1;//捡拾到道具后出现的提示信息
        Destroy(transform.parent.gameObject);
    }

}
