using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop13 : MonoBehaviour
{
    //背包栏的自己的日记
    private Vector2 origin;
    private PropEventManager propeventmanager;

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
        propeventmanager.propnumber = 13;//出现自己日记内容
    }

}
