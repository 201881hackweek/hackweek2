using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop6 : MonoBehaviour
{
    //背包栏的6有电的手电筒//一直存在 使用后无需销毁
    private Vector2 origin;
    private PropEventManager propeventmanager;
    private float Propwidth = 0.5f;//测试用//后续调
    private int usenumber = 0;
    private string useplace;

    void Start()
    {
        origin = GetComponent<RectTransform>().anchoredPosition;
        GameObject propmanager = GameObject.Find("PropEventManager");
        propeventmanager = propmanager.GetComponent<PropEventManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnMouseEnter()
    {
        GetComponent<RectTransform>().localScale = new Vector3(1.1f, 1.1f, 1);
    }

    void OnMouseExit()
    {
        GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1);
    }

    void OnMouseDown()
    {
        transform.parent.transform.SetAsLastSibling();//调渲染顺序的保证点击的东东放在最前面
        GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1);
    }


    void OnMouseUp()
    {
        propeventmanager.Dialogbox.SetActive(true);
        propeventmanager.propnumber = 6;//查看电筒文本
    }

}
