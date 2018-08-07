﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop2_map : MonoBehaviour
{
    //地图里的2水果刀
    private PropEventManager propeventmanager;
    public RectTransform Prop2_little;
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
            RectTransform prop2 = Instantiate(Prop2_little, new Vector3(), new Quaternion(0, 0, 0, 0));
            propeventmanager.Updategridposition(prop2);
            propeventmanager.Dialogbox.SetActive(true);
            propeventmanager.prepropnumber = 2;
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
        RectTransform prop2 = Instantiate(Prop2_little, new Vector3(), new Quaternion(0, 0, 0, 0));
        propeventmanager.Updategridposition(prop2);
        propeventmanager.Dialogbox.SetActive(true);
        propeventmanager.prepropnumber = 2;
        Destroy(transform.parent.gameObject);
    }

}
