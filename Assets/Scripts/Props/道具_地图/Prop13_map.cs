using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop13_map : MonoBehaviour
{
    //地图里的13自己的日记
    private PropEventManager propeventmanager;
    public RectTransform Prop13_little;
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
            RectTransform prop13 = Instantiate(Prop13_little, new Vector3(), new Quaternion(0, 0, 0, 0));
            propeventmanager.Updategridposition(prop13);
            propeventmanager.Dialogbox.SetActive(true);
            propeventmanager.prepropnumber = 13;//编号就按道具号来了 不然太乱了...manager记得改
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
        RectTransform prop13 = Instantiate(Prop13_little, new Vector3(), new Quaternion(0, 0, 0, 0));
        propeventmanager.Updategridposition(prop13);
        propeventmanager.Dialogbox.SetActive(true);
        propeventmanager.prepropnumber = 13;
        Destroy(transform.parent.gameObject);
    }

}
