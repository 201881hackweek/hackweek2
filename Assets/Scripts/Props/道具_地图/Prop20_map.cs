using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop20_map : MonoBehaviour
{
    //地图里的20枪（里世界）
    private PropEventManager propeventmanager;
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
            propeventmanager.Dialogbox.SetActive(true);
            propeventmanager.prepropnumber = 20;//捡到枪啦
            ////////player的hasgun设为true！！！！！！！！记得！！！！
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
        propeventmanager.Dialogbox.SetActive(true);
        propeventmanager.prepropnumber = 20;//捡到枪啦
        Destroy(transform.parent.gameObject);
    }

}
