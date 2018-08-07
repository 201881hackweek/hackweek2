using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop3 : MonoBehaviour {
    private Vector2 origin;
    private PropEventManager propeventmanager;
    private float Propwidth=0.5f;//测试用
    public RectTransform Prop8_little;//与dirtykey叠加出的光滑钥匙
                                      
    void Start () {
        
        origin = GetComponent<RectTransform>().anchoredPosition;
        GameObject propmanager = GameObject.Find("PropEventManager");
        propeventmanager = propmanager.GetComponent<PropEventManager>();


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //private void OnMouseDown()
    //{
    //    origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    //}


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
        transform.parent.transform.SetAsLastSibling();
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
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y+Propwidth), Vector2.up);
        if (hit&&hit.collider.gameObject.tag == "dirtykey")
        {
            propeventmanager.Dialogbox.SetActive(true);
            propeventmanager.addpropnumber=2;//第二次叠加
            for (int i = 0; i < 16; i++)
            {
                if (propeventmanager.grids[i].grid == transform.parent)
                {
                    propeventmanager.Emptygrid[i] = true;//自己所在格子重置为空
                    break;
                }
            }
            RectTransform prop8 = Instantiate(Prop8_little, new Vector3(), new Quaternion(0, 0, 0, 0));//在背包栏生成光滑钥匙，位置位于dirtykey
            prop8.SetParent(hit.collider.transform.parent);
            prop8.anchoredPosition = new Vector2(50, -50);
            prop8.transform.localScale = new Vector3(1, 1, 1);
            Destroy(hit.collider.gameObject);//销毁dirtykey
            Destroy(gameObject);//销毁自身
        }
        else
        {
            GetComponent<RectTransform>().anchoredPosition = origin;
        } //如果不是格子或没有检测到物体，则将物品放回到原来的格子内 
    }

}
