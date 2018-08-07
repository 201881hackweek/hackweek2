using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop17 : MonoBehaviour
{
    //背包栏的17削骨刀
    private Vector2 origin;
    private PropEventManager propeventmanager;
    private float Propwidth = 0.5f;//测试用//后续调

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

    void OnMouseDrag()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);   // // 获取目标对象当前的世界坐标系位置，并将其转换为屏幕坐标系的点
        // 设置鼠标的屏幕坐标向量，用上面获得的Pos的z轴数据作为鼠标的z轴数据，使鼠标坐标 // 与目标对象坐标处于同一层面上
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, pos.z);
        // 用上面获取到的鼠标坐标转换为世界坐标系的点，并用其设置目标对象的当前位置
        transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }

    void OnMouseUp()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + Propwidth), Vector2.up);
        if (hit && hit.collider.gameObject.tag == "bathtub")//拖到浴缸烧碎片
        {
            propeventmanager.Dialogbox.SetActive(true);
            propeventmanager.propnumber = 17;//道具编号出不同信息
            ////////door加has opened!!!!!!!!!!!!!!!!!!
            for (int i = 0; i < 16; i++)
            {
                if (propeventmanager.grids[i].grid == transform.parent)
                {
                    propeventmanager.Emptygrid[i] = true;//自己所在格子重置为空
                    break;
                }
            }
            Destroy(gameObject);//销毁自身
        }
        else
        {
            GetComponent<RectTransform>().anchoredPosition = origin;
        } //如果不是格子或没有检测到物体，则将物品放回到原来的格子内 
    }

}
