using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop15_map : MonoBehaviour
{
    //地图里15打火机
    private PropEventManager propeventmanager;
    public RectTransform Prop15_little;
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
            RectTransform prop15 = Instantiate(Prop15_little, new Vector3(), new Quaternion(0, 0, 0, 0));
            propeventmanager.Updategridposition(prop15);
            propeventmanager.Dialogbox.SetActive(true);
            propeventmanager.prepropnumber = 15;//捡拾道具后出现的提示信息
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
        RectTransform prop15 = Instantiate(Prop15_little, new Vector3(), new Quaternion(0, 0, 0, 0));
        propeventmanager.Updategridposition(prop15);
        propeventmanager.Dialogbox.SetActive(true);
        propeventmanager.prepropnumber = 15;//捡拾到道具后出现的提示信息
        Destroy(transform.parent.gameObject);
    }

}
