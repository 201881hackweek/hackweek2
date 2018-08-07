using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door2 : MonoBehaviour
{

    public bool isOpended;
    public bool hasOpended;// 事件门需要先用钥匙开启
    public GameObject door;
    public GameObject parent;

    void Start()
    {
        hasOpended = true;      // 事件门改成false
        isOpended = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("1");
        if (!hasOpended)
            return;
        if (collision.tag != "Player")
            return;
        Debug.Log("2");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isOpended)
            {
                collision.SendMessage("OpenDoor");
                parent.transform.Rotate(90 * Vector3.up);
                parent.transform.position += new Vector3(0.5f, 0);
                door.SetActive(false);
                isOpended = true;
            }
            else
            {
                collision.SendMessage("CloseDoor");
                parent.transform.Rotate(-90 * Vector3.up);
                parent.transform.position += new Vector3(-0.5f, 0);
                door.SetActive(true);
                isOpended = false;
            }
        }
    }




    // Update is called once per frame
    void Update()
    {

    }
}
